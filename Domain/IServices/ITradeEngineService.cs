using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface ITradeEngineService
    {
        void StartTrader();
        void StopTrader();
        bool IsTraderRunning();
    }
}