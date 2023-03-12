using System.Net.WebSockets;
using Websocket.Client;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Domain;

namespace Application.Services
{
    public class RealTimeDataService
    {
        private WebsocketClient
            client =
                new WebsocketClient(new Uri("wss://ws.finnhub.io?token=cbpq442ad3ieg7faui80"));

        private ManualResetEvent exitEvent;

        private Thread _socketThread;

        private bool _isDisconected = false;

        private float _currentPrice;

        public RealTimeDataService()
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
                        .WriteLine($"Reconnection happened, type: {info.Type}"));

            client
                .MessageReceived
                .Subscribe(msg =>
                {
                    StockResponse stock = JsonConvert.DeserializeObject<StockResponse>(msg.ToString());
                    _currentPrice = stock.data[0].p;
                });

            client.Start();

            Task
                .Run(() =>
                    client
                        .Send("{\"type\":\"subscribe\",\"symbol\":\"BINANCE:BTCUSDT\"}"));

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
