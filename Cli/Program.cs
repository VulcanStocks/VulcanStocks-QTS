// See https://aka.ms/new-console-template for more information
using Application.Services;
using Persistence;
using Websocket.Client;

var client = new SocketContext();

client.Start();

System.Threading.Thread.Sleep(1000);
System.Console.WriteLine("Hello");

client.Stop();


/*
var trader = new TraderService(strategy);

string strategy(float price){
    IndicatorService.TryCalculateRsi(price);
    return "hold";
}

      


while (true){
    
    if(Console.ReadKey().KeyChar == 's'){
        trader.StartTrader();
    }else{
        trader.StopTrader();
    }
}
*/