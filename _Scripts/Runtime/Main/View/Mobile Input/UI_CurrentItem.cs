using Udarverse.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_CurrentItem : MonoBehaviour
    {
        [SerializeField] private ItemListSC _itemListData;
        [SerializeField] private Image _image;


        private void Awake()
        {
            _image.sprite = _itemListData.itemList[0].sprite;
        }
        private void OnEnable()
        {
            PlayerItemsInventory.OnPlayerItemChanged += Event_OnItemChanged;
        }

        private void OnDisable()
        {
            PlayerItemsInventory.OnPlayerItemChanged -= Event_OnItemChanged;
        }

        private void Event_OnItemChanged(int currentIndex)
        {
            _image.sprite = _itemListData.itemList[currentIndex].sprite;
        }
    }
}

