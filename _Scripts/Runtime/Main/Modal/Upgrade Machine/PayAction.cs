using System;
using System.Collections.Generic;
using Udarverse.Player.Stats;
using UnityEngine;

namespace Udarverse.Resources
{
    public abstract class PayAction
    {
        public int amountToProduce = 1;
        public List<PriceResource> priceList = new List<PriceResource>();
        public Action OnPaid;
        public abstract Sprite GetRewardSprite();

        public bool TryPay()
        {
            if (PlayerStats.Instance.TryPay(priceList))
            {
                OnPaid?.Invoke();
                return true;
            }
            return false;
        }

    }
}
