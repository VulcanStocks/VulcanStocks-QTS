// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;

var macd = new MacD(7, 26, 4);

var trader = new TradeEngineService(strategy);
string strategy(float price, float volume){
    System.Console.WriteLine(macd.TryGetValue(price,volume)); 
    return "hold";
}

      

while (true){
    
    if(Console.ReadKey().KeyChar == 's'){
        trader.StartTrader();
    }else{
        trader.StopTrader();
    }
}
