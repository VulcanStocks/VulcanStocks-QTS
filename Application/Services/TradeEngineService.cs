using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.IServices;

namespace Application.Services
{

    public class TradeEngineService : ITradeEngineService
    {
        private Thread _traderThread;
        private Func<float, float, StrategyResult> _strategy;
        private CancellationTokenSource _cts;
        private IRealTimeDataService _realTimeDataService;
        private IBrokerService _brokerService;

        private readonly float _timeFrame;
        public bool IsAlive { get; set; }

        public TradeEngineService(Func<float, float, StrategyResult> strategy, float timeFrame, IRealTimeDataService realTimeDataService, IBrokerService brokerService)
        {
            _timeFrame = timeFrame;
            _strategy = strategy;
            _realTimeDataService = realTimeDataService;
            _brokerService = brokerService;
            Initialize();
        }

        private void Initialize()
        {
            _traderThread = new Thread(EnterTradeLoop);
            _cts = new CancellationTokenSource();
        }

        public void StartTrader()
        {
            if (!_traderThread.IsAlive)
            {
                RestartTraderIfNeeded();
                _realTimeDataService.Start();
                _traderThread.Start(_cts.Token);
                IsAlive = true;
            }
            else
            {
                Console.WriteLine("Trader is already running");
            }
        }

        private void RestartTraderIfNeeded()
        {
            if (_cts.IsCancellationRequested)
            {
                Initialize();
            }
        }

        public void StopTrader()
        {
            _cts.Cancel();
            _realTimeDataService.Stop();
            IsAlive = false;
        }

        private void EnterTradeLoop(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            try
            {
                while (true)
                {
                    float price = _realTimeDataService.GetPrice();
                    float volume = _realTimeDataService.GetVolume();
                    token.ThrowIfCancellationRequested();
                    Trade(price, volume);
                }
            }
            catch (OperationCanceledException)
            {
                HandleTraderCancellation();
            }
        }

        private void HandleTraderCancellation()
        {
            float price = _realTimeDataService.GetPrice();
            _brokerService.Sell(price);
            UpdateCli(price, _realTimeDataService.GetVolume(), StrategyResult.Sell);
            Console.WriteLine("Trader stopped");
        }

        private void Trade(float price, float volume)
        {
            if (price != 0)
            {
                StrategyResult result = _strategy(price, volume);
                ExecuteStrategy(result, price);
                UpdateCli(price, volume, result);
            }
            Thread.Sleep((int)Math.Round((1000 * _timeFrame)));
        }

        private void ExecuteStrategy(StrategyResult result, float price)
        {
            switch (result)
            {
                case StrategyResult.Buy:
                    _brokerService.Buy(price);
                    break;
                case StrategyResult.Sell:
                    _brokerService.Sell(price);
                    break;
                case StrategyResult.Hold:
                    Console.WriteLine("Hold");
                    break;
                default:
                    throw new OperationCanceledException();
            }
        }

        private void UpdateCli(float price, float volume, StrategyResult result)
        {
            Console.WriteLine($"Price: {price} Volume: {volume} Result: {result}");
            Console.WriteLine($"Balance: {_brokerService.GetBalance()}");
            Console.WriteLine("--------------------------");
        }

        public bool IsTraderRunning()
        {
            return IsAlive;
        }
    }
}