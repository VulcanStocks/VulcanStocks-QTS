using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Indicators;
using Domain;

namespace Cli.Strategies
{
    public class AdvancedVwapStrategy
    {
        private NoramlizedVwap _vwap;
        private MacD _macd;
        private Rsi _rsi;

        public AdvancedVwapStrategy(int vwapPeriod, int macdShortPeriod, int macdLongPeriod, int macdSignalPeriod, int rsiPeriod)
        {
            _vwap = new NoramlizedVwap(vwapPeriod);
            _macd = new MacD(macdShortPeriod, macdLongPeriod, macdSignalPeriod);
            _rsi = new Rsi(rsiPeriod);
        }

        public StrategyResult Evaluate(float price, float volume)
        {
            (float normalizedVwap, bool isVwapValid) = _vwap.TryGetValue(price, volume);
            (float macd, bool isMacdValid) = _macd.TryGetValue(price, volume);
            (float rsi, bool isRsiValid) = _rsi.TryGetValue(price, volume);

            if (!isVwapValid || !isMacdValid || !isRsiValid)
            {
                return StrategyResult.Hold;
            }

            float macdSignalLine = _macd.GetLatestSignal();

            bool isMacdCrossAbove = macd > macdSignalLine;
            bool isMacdCrossBelow = macd < macdSignalLine;

            if (price > normalizedVwap && rsi > 70 && isMacdCrossBelow)
            {
                return StrategyResult.Sell;
            }
            else if (price < normalizedVwap && rsi < 30 && isMacdCrossAbove)
            {
                return StrategyResult.Buy;
            }
            else
            {
                return StrategyResult.Hold;
            }
        }
    }
}
