// See https://aka.ms/new-console-template for more information
using Application.Services;


var client = new RealTimeDataService();


var trader = new TraderService(strategy);

string strategy(float price){
    IndicatorService.TryCalculateRsi(price);
    return "hold";
}

      

while (true){
    
    if(Console.ReadKey().KeyChar == 's'){
        trader.StartTrader();
    }else{
        trader.StopTrader();
    }
}
