// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using static Application.Services.TradeEngineService;

var macd = new MacD(7, 26, 4);

var trader = new TradeEngineService(strategy);

StrategyResult strategy(float price, float volume){
    macd.TryGetValue(price,volume);
    return StrategyResult.Sell;
}

      

while (true){
    
    if(Console.ReadKey().KeyChar == 's'){
        trader.StartTrader();
    }else{
        trader.StopTrader();
    }
}
