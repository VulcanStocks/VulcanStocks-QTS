using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Indicators;
using Domain;

namespace Cli.Strategies
{
    public class MacdRsiStrategy
    {
        private MacD _macd;
        private Rsi _rsi;

        public MacdRsiStrategy(int macdShortPeriod, int macdLongPeriod, int macdSignalPeriod, int rsiPeriod)
        {
            _macd = new MacD(macdShortPeriod, macdLongPeriod, macdSignalPeriod);
            _rsi = new Rsi(rsiPeriod);
        }


        public StrategyResult Evaluate(float price, float volume)
        {
            var rsi = _rsi.TryGetValue(price, volume);
            var macd = _macd.TryGetValue(price, volume);
            float macdSignalLine = _macd.GetLatestSignal();
            float macdLine = _macd.GetLatestMacd();

            if (!macd.Item2 || !rsi.Item2)
            {
                return StrategyResult.Hold;
            }

            if (rsi.Item1 < 30 && macdSignalLine > macdLine)
            {
                return StrategyResult.Buy;
            }

            if (rsi.Item1 > 30 || macdSignalLine < macdLine)
            {
                return StrategyResult.Sell;
            }

            return StrategyResult.Hold;
        }

    }
}