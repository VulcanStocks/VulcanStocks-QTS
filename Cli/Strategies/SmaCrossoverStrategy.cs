using System;
using Application.Indicators;
using Domain;

namespace Cli.Strategies
{
    public class SmaCrossoverStrategy
    {
        private readonly Sma _shortSma;
        private readonly Sma _longSma;

        public SmaCrossoverStrategy(int shortPeriod, int longPeriod)
        {
            _shortSma = new Sma(shortPeriod);
            _longSma = new Sma(longPeriod);
        }

        public StrategyResult CheckSmaCrossover(float price, float volume)
        {
            var shortSmaValue = _shortSma.TryGetValue(price, volume);
            var longSmaValue = _longSma.TryGetValue(price, volume);

            if (!shortSmaValue.Item2 || !longSmaValue.Item2)
            {
                return StrategyResult.Hold;
            }

            if (shortSmaValue.Item1 > longSmaValue.Item1 && price != 0)
            {
                return StrategyResult.Buy;
            }
            else if (shortSmaValue.Item1 < longSmaValue.Item1)
            {
                return StrategyResult.Sell;
            }
            else
            {
                return StrategyResult.Hold;
            }
        }
    }
}
