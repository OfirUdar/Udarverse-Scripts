using System;
using Udarverse.Inventory;
using UnityEngine;

namespace Udarverse.Resources
{
    [Serializable]
    public class ItemPayAction : PayAction
    {
        public ItemSC itemSC;

        public override Sprite GetRewardSprite()
        {
            return itemSC.sprite;
        }
    }
}
