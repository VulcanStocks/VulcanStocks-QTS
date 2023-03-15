using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using Websocket.Client;

namespace Application.Services
{
    public class RealTimeDataService
    {
        private WebsocketClient client;

        private ManualResetEvent exitEvent;

        private Thread _socketThread;

        private bool _isDisconnected = false;
        private bool _reconnecting = false;

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
                if (_isDisconnected)
                {
                    Initialize();
                    _isDisconnected = false;
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
            _isDisconnected = true;
        }

        private void Connect()
        {
            exitEvent = new ManualResetEvent(false);

            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            client
                .ReconnectionHappened
                .Subscribe(info =>
                {
                    Console.WriteLine($"Reconnection happened, type: {info.Type}");
                    if (!_reconnecting)
                    {
                        _reconnecting = true;
                        SubscribeToTicker();
                    }
                });

            client
                .MessageReceived
                .Subscribe(HandleMessageReceived);

            client.DisconnectionHappened.Subscribe(info =>
            {
                System.Console.WriteLine($"DisconnectionHappened, type: {info.Type}");
                if (info.Type == DisconnectionType.ByUser)
                {
                    _reconnecting = false;
                }
            });

            client.Start();

            SubscribeToTicker();

            exitEvent.WaitOne();
        }

        private void SubscribeToTicker()
        {
            string message = "{\"type\":\"subscribe\",\"symbol\":\"" + _ticker + "\"}";
            Task.Run(() => client.Send(message));
        }


        private void HandleMessageReceived(ResponseMessage msg)
        {
            if (msg.MessageType == WebSocketMessageType.Text && msg.Text.Contains("\"data\":"))
            {
                StockResponse stock = JsonConvert.DeserializeObject<StockResponse>(msg.Text);
                _currentPrice = stock.data[0].p;
                _currentVolume = stock.data[0].v;
            }
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
