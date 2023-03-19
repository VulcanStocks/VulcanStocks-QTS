using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.IServices;
using Newtonsoft.Json;
using Websocket.Client;
using Websocket.Client.Models;

namespace Application.Services
{
    public class RealTimeDataService : IRealTimeDataService
    {
        private WebsocketClient _client;
        private ManualResetEvent _exitEvent;
        private Thread _socketThread;
        private bool _isDisconnected;
        private bool _reconnecting;
        private float _currentPrice;
        private float _currentVolume;
        private readonly string _ticker;

        public RealTimeDataService(string ticker, string apiToken)
        {
            _client = new WebsocketClient(new Uri("wss://ws.finnhub.io?token=" + apiToken));
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
                RestartIfDisconnected();
                _socketThread.Start();
            }
            else
            {
                Console.WriteLine("Trader is already running");
            }
        }

        private void RestartIfDisconnected()
        {
            if (_isDisconnected)
            {
                Initialize();
                _isDisconnected = false;
            }
        }

        public void Stop()
        {
            Disconnect();
            _isDisconnected = true;
        }

        private void Connect()
        {
            _exitEvent = new ManualResetEvent(false);
            ConfigureClient();
            _client.Start();
            SubscribeToTicker();
            _exitEvent.WaitOne();
        }

        private void ConfigureClient()
        {
            _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            _client.ReconnectionHappened.Subscribe(HandleReconnection);
            _client.MessageReceived.Subscribe(HandleMessageReceived);
            _client.DisconnectionHappened.Subscribe(HandleDisconnection);
        }

        private void HandleReconnection(ReconnectionInfo info)
        {
            Console.WriteLine($"Reconnection happened, type: {info.Type}");
            if (!_reconnecting)
            {
                _reconnecting = true;
                SubscribeToTicker();
            }
        }

        private void HandleDisconnection(DisconnectionInfo info)
        {
            Console.WriteLine($"Disconnection happened, type: {info.Type}");
            if (info.Type == DisconnectionType.ByUser)
            {
                _reconnecting = false;
            }
        }

        private void SubscribeToTicker()
        {
            var message = "{\"type\":\"subscribe\",\"symbol\":\"" + _ticker + "\"}";
            Task.Run(() => _client.Send(message));
        }

        private void HandleMessageReceived(ResponseMessage msg)
        {
            if (msg.MessageType == WebSocketMessageType.Text && msg.Text.Contains("\"data\":"))
            {
                var stock = JsonConvert.DeserializeObject<StockResponse>(msg.Text);
                _currentPrice = stock.data[0].p;
                _currentVolume = stock.data[0].v;
            }
        }

        private void Disconnect()
        {
            if (_client != null && _client.IsRunning)
            {
                _client.Stop(WebSocketCloseStatus.NormalClosure, "");
                _exitEvent.Set();
                Console.WriteLine("Client disconnected");
            }
        }
    }
}
