using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Inventory
{
    [CreateAssetMenu(fileName = "Item List", menuName = "ScriptableObjects/Items/Item List", order = 0)]
    public class ItemListSC : ScriptableObject
    {
        public List<ItemSC> itemList;

    }
}

