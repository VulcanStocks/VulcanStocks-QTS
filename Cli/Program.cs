// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using Cli.Strategies;
using static Application.Services.TradeEngineService;

SimulatedBrokerService.InitSimulatedBroker(100000, 2);
var hiddenBullishDivergenceStrategy = new HiddenBullishDivergenceStrategy(14, 5, 3);
//BINANCE:BTCUSDT"
float orderPrice = 0;
var trader = new TradeEngineService(strategy , 1f, new RealTimeDataService("BINANCE:BTCUSDT", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg"), new BrokerService(true));


StrategyResult strategy(float price, float volume)
{
    var div = hiddenBullishDivergenceStrategy.CheckHiddenBullishDivergence(price);
    if (div)
    {
        Console.WriteLine($"Hidden Bullish Divergence found: {price}");
        if (!SimulatedBrokerService.HasAsset)
        {
            orderPrice = price;
            return StrategyResult.Buy;
        }
        else
        {
            return StrategyResult.Hold;
        }

    }
    else if (price > orderPrice * (1 + (0.5 / 100)) || price < orderPrice * (0.25 + (1 / 100)) && orderPrice != 0)
    {
        if (SimulatedBrokerService.HasAsset)
        {
            orderPrice = 0;
            return StrategyResult.Sell;
        }
        else
        {
            return StrategyResult.Hold;
        }
    }
    else return StrategyResult.Hold;
}

System.Console.WriteLine("Press space to start:");
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
