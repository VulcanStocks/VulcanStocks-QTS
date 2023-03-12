using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TraderService
    {
        private Thread _traderThread;

        private Func<float, string> _strategy;

        private CancellationTokenSource _cts;

        public TraderService(Func<float, string> strategy)
        {
            _strategy = strategy;
            Initialize();
        }

        public void Initialize()
        {
            _traderThread = new Thread(Trade);

            _cts = new CancellationTokenSource();
        }

        public void StartTrader()
        {
            if (!_traderThread.IsAlive)
            {
                Initialize();
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

        public void Trade(object obj)
        {
            CancellationToken token = (CancellationToken) obj;
            try
            {
                while (true)
                {
                    Console.WriteLine("Trade");
                    token.ThrowIfCancellationRequested(); 
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Trader stopped");
            }
        }
    }
}
