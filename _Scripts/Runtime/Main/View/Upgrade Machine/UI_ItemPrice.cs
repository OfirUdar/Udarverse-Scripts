using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ItemPrice : MonoBehaviour
    {
        [SerializeField] private Image _resourcePriceImage;
        [SerializeField] private TextMeshProUGUI _priceText;



        public void Setup(Sprite resourceSprite, int price, Color colorText)
        {
            _resourcePriceImage.sprite = resourceSprite;
            _priceText.text = price.ToString();
            _priceText.color = colorText;
        }
    }
}

