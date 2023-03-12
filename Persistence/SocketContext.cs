using System.Net.WebSockets;
using Websocket.Client;

namespace Persistence
{
    public class SocketContext
    {
        private WebsocketClient
            client =
                new WebsocketClient(new Uri("wss://ws.finnhub.io?token=cbpq442ad3ieg7faui80"));

        private ManualResetEvent exitEvent;

        private Thread _socketThread;

        private bool _isDisconected = false;

        private float _currentPrice;

        public SocketContext()
        {
            Initialize();
        }

        private void Initialize()
        {
            _socketThread = new Thread(Connect);
        }

        public float GetPrice()
        {
            return _currentPrice;
        }

        public void Start()
        {
            if (!_socketThread.IsAlive)
            {
                if (_isDisconected)
                {
                    Initialize();
                    _isDisconected = false;
                }
                _socketThread.Start();
            }
            else
            {
                System.Console.WriteLine("Trader is already running");
            }
        }

        public void Stop()
        {
            Disconnect();
            _isDisconected = true;
        }

        private void Connect()
        {
            exitEvent = new ManualResetEvent(false);

            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            client
                .ReconnectionHappened
                .Subscribe(info =>
                    Console
                        .WriteLine($"Reconnection happened, type: {
                            info.Type}"));

            client
                .MessageReceived
                .Subscribe(msg =>
                {
                    Console.WriteLine($"Message received: {msg}");
                    _currentPrice = 0;
                });

            client.Start();

            Task
                .Run(() =>
                    client
                        .Send("{\"type\":\"subscribe\",\"symbol\":\"AAPL\"}"));

            exitEvent.WaitOne();
        }

        private void Disconnect()
        {
            if (client != null && client.IsRunning)
            {
                client.Stop(WebSocketCloseStatus.NormalClosure, "");
                exitEvent.Set();
                System.Console.WriteLine("client disconnect");
            }
        }
    }
}
