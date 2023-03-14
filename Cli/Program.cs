// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using static Application.Services.TradeEngineService;

SimulatedBrokerService.InitSimulatedBroker(1000);
var rsi = new Rsi(14);

var trader = new TradeEngineService(strategy, "AAPL", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg", 1f, true);

StrategyResult strategy(float price, float volume)
{
    var rsiValue = rsi.TryGetValue(price, volume);
    if (rsiValue.Item2)
    {
        if (rsiValue.Item1 > 70)
        {
            return StrategyResult.Sell;
        }
        else if (rsiValue.Item1 < 30)
        {
            return StrategyResult.Buy;
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
