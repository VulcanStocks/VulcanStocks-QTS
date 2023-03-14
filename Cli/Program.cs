// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using static Application.Services.TradeEngineService;

SimulatedBrokerService.InitSimulatedBroker(1000);
var rsi = new Rsi(14);
var obv = new Obv();
var trader = new TradeEngineService(strategy, "AAPL", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg", 1f, true);

StrategyResult strategy(float price, float volume) 
{
    var rsiValue = rsi.TryGetValue(price, volume);
    var obvValue = obv.TryGetValue(price, volume);
    
    System.Console.WriteLine($"RSI: {rsiValue}, OBV: {obvValue}");
    
    if (rsiValue.Item2 && obvValue.Item2) 
    {
        if (rsiValue.Item1 < 30 && obvValue.Item1 > obv.GetResistanceLevel()) 
        {
            return StrategyResult.Buy;
        } 
        else if (rsiValue.Item1 > 70 && obvValue.Item1 < obv.GetResistanceLevel()) 
        {
            return StrategyResult.Sell;
        }
    }
    
    return StrategyResult.Hold;
}
while(true){
    if(Console.ReadKey().KeyChar == ' '){
        if(trader.IsAlive){
            trader.StopTrader();
        }
        else{
            trader.StartTrader();
        }
    }
}
