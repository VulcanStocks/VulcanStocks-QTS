using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IRealTimeDataService
    {
        void Start();
        void Stop();
        float GetPrice();
        float GetVolume();
    }
}