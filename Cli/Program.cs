// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using static Application.Services.TradeEngineService;

SimulatedBrokerService.InitSimulatedBroker(1000);

var trader = new TradeEngineService(strategy, "AAPL", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg", 1f, true);
var shortSma = new Sma(50);
var longSma = new Sma(200);

StrategyResult strategy(float price, float volume)
{
    var shortSmaValue = shortSma.TryGetValue(price, volume);
    var longSmaValue = longSma.TryGetValue(price, volume);

    System.Console.WriteLine($"Short SMA: {shortSmaValue.Item1}, Long SMA: {longSmaValue.Item1}");

    if (shortSmaValue.Item2 && longSmaValue.Item2)
    {
        if (shortSmaValue.Item1 > longSmaValue.Item1)
        {
            return StrategyResult.Buy;
        }
        else if (shortSmaValue.Item1 < longSmaValue.Item1)
        {
            return StrategyResult.Sell;
        }
    }

    return StrategyResult.Hold;
}
while (true)
{
    if (Console.ReadKey().KeyChar == ' ')
    {
        if (trader.IsAlive)
        {
            trader.StopTrader();
        }
        else
        {
            trader.StartTrader();
        }
    }
}
