using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TradeEngineService
    {
        private Thread _traderThread;

        private Func<float, float, string> _strategy;

        private CancellationTokenSource _cts;

        RealTimeDataService _realTimeDataService;

        public TradeEngineService(Func<float, float, string> strategy)
        {
            _strategy = strategy;
            Initialize();
        }

        public void Initialize()
        {
            _traderThread = new Thread(EnterTradeLoop);
            _realTimeDataService = new RealTimeDataService();
            _cts = new CancellationTokenSource();
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
            CancellationToken token = (CancellationToken)obj;
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Trade();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Trader stopped");
            }
        }

        private void Trade()
        {
            var price = _realTimeDataService.GetPrice();
            var volume = _realTimeDataService.GetVolume();
            //price is zero if _realTimeDataService not working
            if (price != 0)
            {
                var result = _strategy(price, volume);
                UpdateCli(price.ToString() ,volume.ToString(), result);

            }
            Thread.Sleep(1000);

            // CheckStrategyResult(result);
        }

        private void UpdateCli(string price,string volume, string result){
            Console.WriteLine($"Price: {price} Volume: {volume} Result: {result}");
        }

        private void CheckStrategyResult(string result)
        {
            if (result == "buy")
            {
                Console.WriteLine("buy");
            }
            else if (result == "sell")
            {
                Console.WriteLine("sell");
            }
            else if (result == "hold")
            {
                Console.WriteLine("hold");
            }
            else
            {
                throw new OperationCanceledException();
            }
        }
    }
}
