using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public static class SimulatedBrokerService
    {
        public static float Balance { get; set; }
        public static bool HasAsset { get; set; }
        private static float _amountOfAssetsToBuy;

        public static void InitSimulatedBroker(float balance, float amountOfAssetsToBuy)
        {
            _amountOfAssetsToBuy = amountOfAssetsToBuy;
            Balance = balance;
        }
        public static void Buy(float price)
        {
            if (!HasAsset)
            {
                Balance -= price * _amountOfAssetsToBuy;
                HasAsset = true;
            }
        }

        public static void Sell(float price)
        {
            if (HasAsset)
            {
                Balance += price * _amountOfAssetsToBuy;
                HasAsset = false;
            }
        }

    }
}