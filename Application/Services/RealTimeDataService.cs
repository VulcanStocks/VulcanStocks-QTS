using System.Net.WebSockets;
using Websocket.Client;
using Newtonsoft.Json;
using Domain;

namespace Application.Services
{
    public class RealTimeDataService
    {
        private WebsocketClient client;


        private ManualResetEvent exitEvent;

        private Thread _socketThread;

        private bool _isDisconected = false;

        private float _currentPrice;
        private float _currentVolume;

        private readonly string _ticker;

        public RealTimeDataService(string ticker, string apiToken)
        {
            client = new WebsocketClient(new Uri("wss://ws.finnhub.io?token=" + apiToken));
            _ticker = ticker;
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

        public float GetVolume()
        {
            return _currentVolume;
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
                    _currentVolume = stock.data[0].v;
                });

            client.Start();

            string message = "{\"type\":\"subscribe\",\"symbol\":\"" + _ticker + "\"}";
            Task
                .Run(() =>
                    client
                        .Send(message));

            exitEvent.WaitOne();
        }

        private void Disconnect()
        {
            if (client != null && client.IsRunning)
            {
                client.Stop(WebSocketCloseStatus.NormalClosure, "");
                exitEvent.Set();
                System.Console.WriteLine("Client disconnect");
            }
        }
    }



}
