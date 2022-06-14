using System.Collections.Generic;
using Udarverse.Inventory;
using Udarverse.Save;
using UnityEngine;

namespace Udarverse.UI
{
    public class UI_ItemsInventory : MonoBehaviour
    {
        [SerializeField] private UI_Item _itemPfb;
        [SerializeField] private Transform _itemsContainer;

        private readonly List<UI_Item> _itemList = new List<UI_Item>();

        private int _currentItemIndex;

        private void OnEnable()
        {
            PlayerItemsInventory.OnInitialized += Event_OnInialized;
            PlayerItemsInventory.OnPlayerItemChanged += Event_OnItemChanged;
        }

        private void OnDisable()
        {
            PlayerItemsInventory.OnInitialized -= Event_OnInialized;
            PlayerItemsInventory.OnPlayerItemChanged -= Event_OnItemChanged;
        }


        private void Event_OnItemChanged(int currentItemIndex)
        {
            if (_itemList.Count == 0)
                return;

            _itemList[_currentItemIndex].SetCanvasAlpha(0.3f);
            _currentItemIndex = currentItemIndex;
            _itemList[_currentItemIndex].SetCanvasAlpha(1f);
        }
        private void Event_OnInialized(List<ItemInventory> itemInventoryList)
        {
            for (int i = 0; i < itemInventoryList.Count; i++)
            {
                var iteminventory = itemInventoryList[i];
                var itemSC = iteminventory.itemSC;
                var itemInstance = Instantiate(_itemPfb, _itemsContainer, false);
                itemInstance.SetItemIndex(i)
                    .SetSprite(itemSC.sprite)
                    .SetCanvasAlpha(0.3f)
                    .SetLevel(iteminventory.level);

                iteminventory.OnLevelUp += (level) => itemInstance.SetLevel(level);
                _itemList.Add(itemInstance);
            }
        }
    }

}
