// See https://aka.ms/new-console-template for more information

using Application.Services;
using Cli.Strategies;
using Domain;

var hiddenBullishDivergenceStrategy = new HiddenBullishDivergenceStrategy(14, 5, 3);
//BINANCE:BTCUSDT"
float orderPrice = 0;

var trader = new TradingOrchestrator(strategy, 1f, new RealTimeDataService("BINANCE:BTCUSDT", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg"), new BrokerService(true));


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
    else if (price > orderPrice * (1 + (0.05 / 100)) || price < orderPrice * (0.025 + (1 / 100)) && orderPrice != 0)
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


var result = await trader.RunBacktest(100000, 2, "AAPL", "5min", "9HU1R3FPJHV0PEKT");

System.Console.WriteLine(result);




/*
System.Console.WriteLine("Press space to start:");
while (true)
{
    if (Console.ReadKey().KeyChar == ' ')
    {
        if (trader.IsTraderRunning())
        {
            trader.StopTrader();
        }
        else
        {
            trader.StartTrader();
        }
    }
}

*/
