using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Stochastic : ITechnicalIndicator
{
    private readonly int _period;
    private readonly Queue<float> _highs;
    private readonly Queue<float> _lows;

    public Stochastic(int period)
    {
        _period = period;
        _highs = new Queue<float>(period);
        _lows = new Queue<float>(period);
    }

    public (float, bool) TryGetValue(float price, float volume)
    {
        _highs.Enqueue(price);
        _lows.Enqueue(price);

        if (_highs.Count > _period)
        {
            _highs.Dequeue();
        }

        if (_lows.Count > _period)
        {
            _lows.Dequeue();
        }

        if (_highs.Count < _period || _lows.Count < _period)
        {
            return (0f, false);
        }

        float highestHigh = _highs.Max();
        float lowestLow = _lows.Min();
        float current = price;

        float stochastic = (current - lowestLow) / (highestHigh - lowestLow) * 100;

        return (stochastic, true);
    }
}

}