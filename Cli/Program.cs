// See https://aka.ms/new-console-template for more information

using Application.Services;
using Cli.Strategies;
using Domain;

//BINANCE:BTCUSDT"


/*

StrategyResult strategy(float price, float volume){
    var Strategy = new SmaCrossoverStrategy(14,28);
    return Strategy.CheckSmaCrossover(price, volume);
}



// För varje pris i din prishistorik, använd strategin för att avgöra om du ska köpa, sälja eller hålla

*/
var macdRsiStrategy = new MacdRsiStrategy(12, 26, 9, 14);
var trader = new TradingOrchestrator(strategy, 1f, new RealTimeDataService("BINANCE:BTCUSDT", "cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg"), new BrokerService(true));

StrategyResult strategy(float price, float volume)
{
    return macdRsiStrategy.Evaluate(price, volume);
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


var result = await trader.RunBacktest(10000, 2, "aapl", "5min", "9HU1R3FPJHV0PEKT");

Console.WriteLine("Statistik för handelsstrategi:");
Console.WriteLine("--------------------------------");
Console.WriteLine($"Vinst/Förlust-kvot: {result.WinLossRatio}");
Console.WriteLine($"Antal genomförda affärer: {result.TradesTaken}");
Console.WriteLine($"Antal tester: {result.AmountOfTests}");
Console.WriteLine($"Kontosaldo: {result.Balance}");
Console.WriteLine($"Total vinst: {result.Profit}");