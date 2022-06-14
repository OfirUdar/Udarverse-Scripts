using System;
using System.Collections.Generic;
using Udarverse.Inventory;
using Udarverse.Resources;
using UnityEngine;

namespace Udarverse.Save
{
    [Serializable]
    public class ItemInventoryData
    {
        public List<ItemInventory> itemsInventoryList;
    }
    [Serializable]
    public class ResourceInventoryData
    {
        public List<ResourceInventory> resourcesInventoryList;

    }
    [Serializable]
    public class PlayerPositionData
    {
        public Vector3 position;
    }

}
