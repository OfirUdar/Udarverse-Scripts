using System.Collections.Generic;
using Udarverse.Character;
using UnityEngine;

namespace Udarverse.Inventory
{
    public abstract class CharacterItemsInventory : MonoBehaviour
    {
        [SerializeField] protected ItemListSC _itemListData;
        [SerializeField] protected Transform _handHolder;
        [SerializeField] private CharacterAnimation _characterAnim;
        private IInputCharacter _inputCharacter;

        protected List<ItemInventory> _itemInventoryList = new List<ItemInventory>();
        protected int _currentItemIndex = 0;
        protected ItemBase _currentItem;

        private const int _DEFAULT_ITEM_LEVEL = 1;


        protected abstract void Init();

        private void Awake()
        {
            _inputCharacter = this.GetComponent<IInputCharacter>();
            Init();
        }

        private void OnEnable()
        {
            if (_currentItem != null)
                _characterAnim.SetLayer(_currentItem.GetAnimationLayer(), true);

            _inputCharacter.OnItemNextPrev += NextPrevItem;
            _inputCharacter.OnItemSelected += ChangeItem;
        }
        private void OnDisable()
        {
            _inputCharacter.OnItemNextPrev -= NextPrevItem;
            _inputCharacter.OnItemSelected -= ChangeItem;
        }



        public ItemBase GetCurrentItem()
        {
            return _currentItem;
        }

        protected void InitDefaultIventoryList()
        {
            foreach (var item in _itemListData.itemList)
            {
                var itemInstance = Instantiate(item.GetItem(_DEFAULT_ITEM_LEVEL), _handHolder, false);
                itemInstance.SetOwner(this.transform);
                itemInstance.gameObject.SetActive(false);
                var itemInvetory = new ItemInventory(item, _DEFAULT_ITEM_LEVEL).SetInstance(itemInstance);
                _itemInventoryList.Add(itemInvetory);
            }
        }
        protected ItemInventory GetItem(ItemSC item)
        {
            return _itemInventoryList.Find((itemInv) => item == itemInv.itemSC);
        }
        public int GetLevel(ItemSC item)
        {
            return GetItem(item).level;
        }
        private void NextPrevItem(bool isNext)
        {
            if (isNext)
            {
                _currentItemIndex--;
            }
            else
            {
                _currentItemIndex++;
            }
            _currentItemIndex = Mathf.Clamp(_currentItemIndex, 0, _itemInventoryList.Count - 1);

            ChangeItem();
        }
        protected void ChangeItem(int index)
        {
            _currentItemIndex = Mathf.Clamp(index, 0, _itemInventoryList.Count - 1);
            ChangeItem();
        }


        protected void ChangeItem()
        {
            if (_currentItem != null)
            {
                _currentItem.gameObject.SetActive(false);
                _characterAnim.SetLayer(_currentItem.GetAnimationLayer(), false);
            }

            _currentItem = _itemInventoryList[_currentItemIndex].itemInstance;

            _currentItem.gameObject.SetActive(true);
            _characterAnim.SetLayer(_currentItem.GetAnimationLayer(), true);

            OnItemChanged();
        }

        protected virtual void OnItemChanged()
        {

        }






    }

}
