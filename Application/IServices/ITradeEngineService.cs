using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface ITradeEngineService
    {
        void StartTrader();
        void StopTrader();
    }
}