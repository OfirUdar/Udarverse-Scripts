using System;
using System.Collections.Generic;
using Udarverse.Save;
using UnityEngine;

namespace Udarverse.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item", order = 0)]
    public class ItemSC : SaveableScriptableObject
    {
        public Sprite sprite;
        public List<ItemBase> itemPfbList;

        //private int _level = 1;


//        public event Action<int> OnLevelUp;


        //[ContextMenu("Reset Level")]
        //public void ResetLevel()
        //{
        //    _level = 1;
        //}

        public ItemBase GetItem(int level) => itemPfbList[level - 1];

       // public int GetCurrentLevel() => _level;
        public int GetMaxLevel() => itemPfbList.Count;

        //public void SetLevel(int level)
        //{
        //    _level = level;
        //}
        //public void LevelUp()
        //{
        //    _level++;
        //    OnLevelUp?.Invoke(_level);
        //    GameSaveManager.Instance.SaveItemsInventory();
        //}

    }
}

