
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_Item : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _itemLevelText;

        private int _itemIndex;

        public static event Action<int> OnItemSelected;

        public UI_Item SetItemIndex(int itemIndex)
        {
            _itemIndex = itemIndex;
            return this;
        }
        public UI_Item SetCanvasAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
            return this;
        }
        public UI_Item SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
            return this;
        }
        public UI_Item SetLevel(int level)
        {
            _itemLevelText.text = "lvl " + level;
            return this;
        }

        public void Select_Button()
        {
            OnItemSelected?.Invoke(_itemIndex);
        }

    }
}

