using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IBrokerService
    {
        void Buy(float price);
        void Sell(float price);
        float GetBalance();
    }
}