// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;

var ema = new Ema(2);

var trader = new TradeEngineService(strategy);
string strategy(float price, float volume){
    ema.TryGetValue(price,volume);
    return "hold";
}

      

while (true){
    
    if(Console.ReadKey().KeyChar == 's'){
        trader.StartTrader();
    }else{
        trader.StopTrader();
    }
}
