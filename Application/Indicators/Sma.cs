using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Sma : ITechnicalIndicator
{
    private readonly int _period;
    private readonly Queue<float> _values;

    public Sma(int period)
    {
        _period = period;
        _values = new Queue<float>(period);
    }

    public (float, bool) TryGetValue(float price, float volume)
    {
        _values.Enqueue(price);

        if (_values.Count > _period)
        {
            _values.Dequeue();
        }

        if (_values.Count < _period)
        {
            return (0f, false);
        }

        float sum = _values.Sum();
        float sma = sum / _period;

        return (sma, true);
    }
}

}