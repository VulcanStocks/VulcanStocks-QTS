using System;
using Domain;
using Domain.IServices;
using Domain.Models;

using static Application.Services.TradeEngineService;

namespace Application.Services
{
    public class TradingOrchestrator
    {
        private readonly ITradeEngineService _tradeEngineService;

        private readonly IBacktesterService _backtesterService;

        Func<float, float, StrategyResult> _strategy;

        public TradingOrchestrator(
            Func<float, float, StrategyResult> strategy,
            float timeFrame,
            IRealTimeDataService realTimeDataService,
            IBrokerService brokerService
        )
        {
            _strategy = strategy;
            _tradeEngineService =
                new TradeEngineService(strategy,
                    timeFrame,
                    realTimeDataService,
                    brokerService);
            _backtesterService =
                new BacktesterService(new HistoricalDataService());
        }

        public void InitSimulatedBroker(
            float balance,
            float amountOfAssetsToBuy
        )
        {
            SimulatedBrokerService.InitSimulatedBroker (
                balance,
                amountOfAssetsToBuy
            );
        }

        public void StartTrader()
        {
            _tradeEngineService.StartTrader();
        }

        public void StopTrader()
        {
            _tradeEngineService.StopTrader();
        }

        public async Task<BacktestResult>
        RunBacktest(
            int balance,
            int amountOfAssetsToBuy,
            string symbol,
            string interval,
            string apikey
        )
        {
            return await _backtesterService
                .RunBacktest(_strategy,
                balance,
                amountOfAssetsToBuy,
                symbol,
                interval,
                apikey);
        }

        public bool SimulatedBrokerHasAsset()
        {
            return SimulatedBrokerService.HasAsset;
        }

        public bool IsTraderRunning()
        {
            return _tradeEngineService.IsTraderRunning();
        }
    }
}
