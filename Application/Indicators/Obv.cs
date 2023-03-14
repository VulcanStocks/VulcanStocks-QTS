using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Obv : ITechnicalIndicator
    {
        private float _previousPrice;
        private float _obv;
        private bool _isValid;
        private List<Tick> _ticks;

        public Obv()
        {
            _previousPrice = 0;
            _obv = 0;
            _isValid = false;
            _ticks = new List<Tick>();
        }

        public (float, bool) TryGetValue(float price, float volume)
        {
            _ticks.Add(new Tick(price, volume));

            float currentObv;

            if (_previousPrice == 0) // first time
            {
                currentObv = 0; // no value yet
                _isValid = false; // not ready yet
            }
            else
            {
                if (price > _previousPrice) // price increased
                {
                    _obv += volume; // add volume to previous OBV
                }
                else if (price < _previousPrice) // price decreased
                {
                    _obv -= volume; // subtract volume from previous OBV
                }

                currentObv = _obv; // return current OBV value
                _isValid = true; // ready to use
            }

            _previousPrice = price; // update previous price

            return (currentObv, _isValid);
        }

        public float GetResistanceLevel()
        {
            // Create a list of all the OBV values
            var obvValues = new List<float>();
            float prevObv = 0;
            foreach (var tick in _ticks)
            {
                var (obv, isValid) = TryGetValue(tick.Price, tick.Volume);
                if (isValid)
                {
                    prevObv = obv;
                    obvValues.Add(obv);
                }
                else
                {
                    obvValues.Add(prevObv);
                }
            }

            // Calculate the average and standard deviation of the OBV values
            var obvAvg = obvValues.Average();
            var obvStdDev = Math.Sqrt(obvValues.Select(x => Math.Pow(x - obvAvg, 2)).Average());

            // Find the highest peak in the OBV values that is more than 1 standard deviation above the average
            var resistanceLevel = obvAvg + obvStdDev;
            var resistancePoints = new List<int>();
            for (int i = 1; i < obvValues.Count - 1; i++)
            {
                if (obvValues[i] > resistanceLevel && obvValues[i] > obvValues[i - 1] && obvValues[i] > obvValues[i + 1])
                {
                    resistancePoints.Add(i);
                }
            }
            if (resistancePoints.Count > 0)
            {
                var resistancePeakIndex = resistancePoints.Aggregate((x, y) => obvValues[x] > obvValues[y] ? x : y);
                return obvValues[resistancePeakIndex];
            }
            else
            {
                return float.NaN;
            }
        }

        private class Tick
        {
            public float Price { get; }
            public float Volume { get; }

            public Tick(float price, float volume)
            {
                Price = price;
                Volume = volume;
            }
        }
    }
}
