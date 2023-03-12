using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace Application.Services
{
    public class TraderService
    {
        private Thread _traderThread;

        private Func<float, string> _strategy;

        private CancellationTokenSource _cts;

        SocketContext _socketContext;

        public TraderService(Func<float, string> strategy)
        {            
            _strategy = strategy;
            Initialize();
        }

        public void Initialize()
        {
            _traderThread = new Thread(Trade);
            _socketContext = new SocketContext();
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
        }

        private void Trade(object obj)
        {
            CancellationToken token = (CancellationToken) obj;
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    //var result = _strategy(DataContext.GetPrice("d"));
                    //CheckStrategyResult(result);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Trader stopped");
            }
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
