using System;
using System.Collections.Generic;
using Udarverse.Save;

namespace Udarverse.Inventory
{
    public class PlayerItemsInventory : CharacterItemsInventory
    {
        public static PlayerItemsInventory Instance { get; private set; }

        public static Action<int> OnPlayerItemChanged;
        public static Action<List<ItemInventory>> OnInitialized;

        protected override void Init()
        {
            Instance = this;
            LoadFromSave();
        }

        private void LoadFromSave()
        {
            var itemsInventoryData = GameSaveManager.Instance.LoadItemsInventory();

            //From Save:
            if (itemsInventoryData != null)
            {
                _itemInventoryList = itemsInventoryData.itemsInventoryList;
                foreach (var item in _itemInventoryList)
                {
                    var itemInstance = Instantiate(item.itemSC.GetItem(item.level), _handHolder, false);
                    itemInstance.SetOwner(this.transform);
                    itemInstance.gameObject.SetActive(false);
                    item.SetInstance(itemInstance);
                }
            }
            else
            {
                //Default:
                InitDefaultIventoryList();
            }

            Invoke(nameof(Initialize), 0.2f);

        }

        private void Initialize()
        {
            OnInitialized?.Invoke(_itemInventoryList);
            ChangeItem();
        }
        protected override void OnItemChanged()
        {
            base.OnItemChanged();
            OnPlayerItemChanged?.Invoke(_currentItemIndex);
        }

        public void LevelUp(ItemSC item)
        {
            var itemInventory = GetItem(item);
            var itemInstance = itemInventory.itemInstance;
            if (itemInstance == null)
                return;

            var siblingIndex = itemInstance.transform.GetSiblingIndex();
            var isActive = itemInstance.gameObject.activeInHierarchy;
            Destroy(itemInstance.gameObject);
            itemInventory.LevelUp();
            var newItemInstance = Instantiate(item.GetItem(itemInventory.level), _handHolder, false);
            newItemInstance.SetOwner(this.transform);
            newItemInstance.gameObject.SetActive(isActive);
            newItemInstance.transform.SetSiblingIndex(siblingIndex);
            if (isActive)
                _currentItem = newItemInstance;

            itemInventory.itemInstance = newItemInstance;

            GameSaveManager.Instance.SaveItemsInventory(_itemInventoryList);
        }
    }
}