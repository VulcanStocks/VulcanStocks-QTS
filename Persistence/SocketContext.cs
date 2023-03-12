using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Websocket.Client;

namespace Persistence
{
    public class SocketContext
    {
        private WebsocketClient
            client =
                new WebsocketClient(new Uri("wss://ws.finnhub.io?token=cbpq442ad3ieg7faui80"));

        private ManualResetEvent exitEvent;

        public void Connect()
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
                    Console.WriteLine($"Message received: {msg}"));
            client.Start();

            Task
                .Run(() =>
                    client
                        .Send("{\"type\":\"subscribe\",\"symbol\":\"AAPL\"}"));

            exitEvent.WaitOne();
        }

        public void Disconnect()
        {
            if (client != null && client.IsRunning)
            {
                client.Stop(WebSocketCloseStatus.NormalClosure, "");
            }
        }
    }
}
