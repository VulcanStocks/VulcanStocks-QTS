// See https://aka.ms/new-console-template for more information
using Application.Services;
using static Application.Services.TradeEngineService;


var trader = new TradeEngineService(strategy, "BINANCE:BTCUSDT","cg867dpr01qsgaf0mme0cg867dpr01qsgaf0mmeg", 1f, true);


int i = 10;

int count = 0;

StrategyResult strategy(float price, float volume){
    return StrategyResult.Sell;
}

trader.StartTrader();
