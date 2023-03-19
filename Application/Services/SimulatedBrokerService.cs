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

        public static float PreviousBalance { get; set; }

        private static float _amountOfAssetsToBuy;

        public static int TotalWins { get; set; }

        public static int TotalLosses { get; set; }

        public static void InitSimulatedBroker(
            float balance,
            float amountOfAssetsToBuy
        )
        {
            _amountOfAssetsToBuy = amountOfAssetsToBuy;
            Balance = balance;
            PreviousBalance = 0;
            TotalWins = 0;
            TotalLosses = 0;
        }

        public static void Buy(float price)
        {
            if (!HasAsset && Balance >= price * _amountOfAssetsToBuy)
            {

                PreviousBalance = Balance;
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

                if (Balance > PreviousBalance)
                {
                    TotalWins++;
                }
                else
                {
                    TotalLosses++;
                }
            }
        }
    }
}
