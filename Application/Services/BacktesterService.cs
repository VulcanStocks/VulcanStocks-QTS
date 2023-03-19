using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.IServices;
using Domain.Models;
using static Application.Services.TradeEngineService;

namespace Application.Services
{
    public class BacktesterService : IBacktesterService
    {
        private readonly IHistoricalDataService _historicalDataService;
        List<HistoricalDataModel> _data;
        public static int TradesWon { get; set; }
        public static int TradesLost { get; set; }
        public static int TradesTaken { get; set; }
        IBrokerService _brokerService;
        public BacktesterService(IHistoricalDataService historicalDataService)
        {
            _historicalDataService = historicalDataService;
            _brokerService = new BrokerService(true);
        }

        public async Task<BacktestResult> RunBacktest(Func<float, float, StrategyResult> strategy, int balance, int amountOfAssetsToBuy, string symbol, string interval, string apikey)
        {
            await Init(balance, amountOfAssetsToBuy, symbol, interval, apikey);
            int amountOfTests = EnterTradeLoop(strategy);
            return GetBacktestResult(balance, amountOfTests);
        }

        private int EnterTradeLoop(Func<float, float, StrategyResult> strategy)
        {
            int amountOfTests = 0;
            foreach (var item in _data)
            {
                Trade(item.Price, item.Volume, strategy);
                amountOfTests++;
            }
            return amountOfTests;
        }

        private async Task Init(float balance, float amountOfAssetsToBuy, string symbol, string interval, string apikey)
        {
            SimulatedBrokerService.InitSimulatedBroker(balance, amountOfAssetsToBuy);
            _data = await _historicalDataService.GetDataListAsync(symbol, interval, apikey);
            System.Console.WriteLine(_data);
        }


        private void Trade(float price, float volume, Func<float, float, StrategyResult> strategy)
        {
            if (price != 0)
            {
                var result = strategy(price, volume);
                ExecuteStrategy(result, price);
            }
        }

        private void ExecuteStrategy(StrategyResult result, float price)
        {
            switch (result)
            {
                case StrategyResult.Buy:

                    _brokerService.Buy(price);
                    TradesTaken++;
                    break;
                case StrategyResult.Sell:
                    _brokerService.Sell(price);
                    break;
                case StrategyResult.Hold:
                    break;
                default:
                    throw new OperationCanceledException();
            }
        }

        private BacktestResult GetBacktestResult(int initailBalance, int amountOfTests)
        {
            var result = new BacktestResult();
            result.Balance = SimulatedBrokerService.Balance;
            result.Profit = SimulatedBrokerService.Balance - initailBalance;
            result.AmountOfTests = amountOfTests;
            result.TradesTaken = TradesTaken;
            result.WinLossRatio = (float)SimulatedBrokerService.TotalWins/(float)SimulatedBrokerService.TotalLosses;

            return result;
        }

    }
}