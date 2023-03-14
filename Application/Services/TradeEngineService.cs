using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TradeEngineService
    {
        private Thread _traderThread;

        private Func<float, float, StrategyResult> _strategy;

        private CancellationTokenSource _cts;

        RealTimeDataService _realTimeDataService;
        SimulatedBrokerService _simulatedBrokerService;

        public enum StrategyResult { Buy, Sell, Hold }
        private readonly string _ticker;
        private readonly string _apiToken;
        private readonly float _timeFrame;
        private readonly bool _useSimulatedTrading;

        public TradeEngineService(Func<float, float, StrategyResult> strategy, string ticker, string apiToken, float timeFrame, bool useSimulatedTrading)
        {
            _useSimulatedTrading = useSimulatedTrading;
            _timeFrame = timeFrame;
            _apiToken = apiToken;
            _ticker = ticker;
            _strategy = strategy;
            Initialize();
        }

        public void Initialize()
        {
            _traderThread = new Thread(EnterTradeLoop);
            _realTimeDataService = new RealTimeDataService(_ticker, _apiToken);
            _cts = new CancellationTokenSource();
            if (_useSimulatedTrading)
            {
                _simulatedBrokerService = new SimulatedBrokerService();
            }
        }

        public void StartTrader()
        {
            if (!_traderThread.IsAlive)
            {
                if (_cts.IsCancellationRequested)
                {
                    Initialize();
                }
                _realTimeDataService.Start();
                _traderThread.Start(_cts.Token);
            }
            else
            {
                System.Console.WriteLine("Trader is already running");
            }
        }

        public void StopTrader()
        {
            _cts.Cancel();
            _realTimeDataService.Stop();


        }

        private void EnterTradeLoop(object obj)
        {
            float price = 0;
            float volume = 0;
            CancellationToken token = (CancellationToken)obj;
            try
            {
                while (true)
                {
                    price = _realTimeDataService.GetPrice();
                    volume = _realTimeDataService.GetVolume();
                    token.ThrowIfCancellationRequested();
                    Trade(price, volume);
                }
            }
            catch (OperationCanceledException)
            {
                if (_useSimulatedTrading)
                {
                    _simulatedBrokerService.Sell(price);
                    UpdateCli(price.ToString(), volume.ToString(), StrategyResult.Sell);
                }
                else
                {

                }
                Console.WriteLine("Trader stopped");
            }
        }

        private void Trade(float price, float volume)
        {

            //price is zero if _realTimeDataService not working
            if (price != 0)
            {
                var result = _strategy(price, volume);
                CheckStrategyResult(result, price);
                UpdateCli(price.ToString(), volume.ToString(), result);
            }
            Thread.Sleep((int)Math.Round((1000 * _timeFrame)));

        }

        private void UpdateCli(string price, string volume, StrategyResult result)
        {
            Console.WriteLine($"Price: {price} Volume: {volume} Result: {result}");

            if (_useSimulatedTrading)
            {
                System.Console.WriteLine($"Balance: {_simulatedBrokerService.Balance}");
            }
            else
            {

            }
        }

        private void CheckStrategyResult(StrategyResult result, float price)
        {
            if (result == StrategyResult.Buy)
            {
                Buy(price);
            }
            else if (result == StrategyResult.Sell)
            {
                Sell(price);
            }
            else if (result == StrategyResult.Hold)
            {
                Console.WriteLine("hold");
            }
            else
            {
                throw new OperationCanceledException();
            }
        }

        private void Buy(float price)
        {
            if (_useSimulatedTrading)
            {
                _simulatedBrokerService.Buy(price);
            }
            else
            {

            }
        }

        private void Sell(float price)
        {
            if (_useSimulatedTrading)
            {
                _simulatedBrokerService.Sell(price);
            }
            else
            {

            }
        }
    }
}
