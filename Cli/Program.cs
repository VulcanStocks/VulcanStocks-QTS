// See https://aka.ms/new-console-template for more information
using Application.Indicators;
using Application.Services;
using Cli.Strategies;
using static Application.Services.TradeEngineService;

SimulatedBrokerService.InitSimulatedBroker(1000);
var hiddenBullishDivergenceStrategy = new HiddenBullishDivergenceStrategy(14);

float orderPrice = 0;
var trader = new TradeEngineService(strategy, "AAPL", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg", 1f, true);


StrategyResult strategy(float price, float volume)
{
    if (hiddenBullishDivergenceStrategy.CheckHiddenBullishDivergence(price))
    {
        Console.WriteLine($"Hidden Bullish Divergence upptäckt vid pris: {price}");
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
