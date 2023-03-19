// See https://aka.ms/new-console-template for more information

using Application.Services;
using Cli.Strategies;
using Domain;

var hiddenBullishDivergenceStrategy = new HiddenBullishDivergenceStrategy(14, 50, 3);
//BINANCE:BTCUSDT"

float orderPrice = 0;

var trader = new TradingOrchestrator(strategy, 1f, new RealTimeDataService("BINANCE:BTCUSDT", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg"), new BrokerService(true));
/*

StrategyResult strategy(float price, float volume){
    var Strategy = new SmaCrossoverStrategy(14,28);
    return Strategy.CheckSmaCrossover(price, volume);
}



// För varje pris i din prishistorik, använd strategin för att avgöra om du ska köpa, sälja eller hålla

*/
StrategyResult strategy(float price, float volume)
{
    var div = hiddenBullishDivergenceStrategy.CheckHiddenBullishDivergence(price);
    if (div)
    {
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
    else if (price > orderPrice * (1 + (0.005 / 100))&& orderPrice != 0)
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
    else if (price < orderPrice * (1 - (0.0025 / 100))){


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


var result = await trader.RunBacktest(100000, 2, "ADBE", "1min", "9HU1R3FPJHV0PEKT");

Console.WriteLine("Statistik för handelsstrategi:");
    Console.WriteLine("--------------------------------");
    Console.WriteLine($"Vinst/Förlust-kvot: {result.WinLossRatio}");
    Console.WriteLine($"Antal genomförda affärer: {result.TradesTaken}");
    Console.WriteLine($"Antal tester: {result.AmountOfTests}");
    Console.WriteLine($"Kontosaldo: {result.Balance}");
    Console.WriteLine($"Total vinst: {result.Profit}");