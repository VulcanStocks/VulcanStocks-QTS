using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class MacD : ITechnicalIndicator
    {
        private readonly int _shortPeriod;

        private readonly int _longPeriod;

        private readonly int _signalPeriod;

        private readonly Ema _shortEma;

        private readonly Ema _longEma;

        private readonly Ema _signalEma;

        public MacD(int shortPeriod, int longPeriod, int signalPeriod)
        {
            _shortPeriod = shortPeriod;
            _longPeriod = longPeriod;
            _signalPeriod = signalPeriod;
            _shortEma = new Ema(shortPeriod);
            _longEma = new Ema(longPeriod);
            _signalEma = new Ema(signalPeriod);
        }

        public (float, bool) TryGetValue(float price, float volume)
        {
            // Calculate the short EMA
            var (shortEmaValue, isShortEmaValid) =
                _shortEma.TryGetValue(price, volume);
            if (!isShortEmaValid)
            {
                return (0f, false);
            }

            // Calculate the long EMA
            var (longEmaValue, isLongEmaValid) =
                _longEma.TryGetValue(price, volume);
            if (!isLongEmaValid)
            {
                return (0f, false);
            }

            // Calculate the MACD line
            float macd = shortEmaValue - longEmaValue;

            // Calculate the signal line
            var (signalValue, isSignalValid) =
                _signalEma.TryGetValue(macd, volume);
            if (!isSignalValid)
            {
                return (0f, false);
            }

            // Calculate the histogram
            float histogram = macd - signalValue;

            return (histogram, true);
        }
    }
}
