using System;


namespace Udarverse.Inventory
{
    [Serializable]
    public class ItemInventory
    {
        public ItemSC itemSC;
        public ItemBase itemInstance;
        public int level = 1;
        public string GUID;

        public event Action<int> OnLevelUp;
        public ItemInventory(ItemSC otherItemSC,int level)
        {
            itemSC = otherItemSC;
            GUID = itemSC.GUID;
            this.level = level;
        }
        public ItemInventory()
        {
        }
        public ItemInventory SetInstance(ItemBase itemInstance)
        {
            this.itemInstance = itemInstance;
            return this;
        }
        public void LevelUp()
        {
            level++;
            OnLevelUp?.Invoke(level);
        }
    }

}
