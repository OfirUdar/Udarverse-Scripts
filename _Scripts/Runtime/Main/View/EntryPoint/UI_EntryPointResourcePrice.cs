
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_EntryPointResourcePrice : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _resourceImage;


        public void SetPrice(int currentAmount, int price)
        {
            _priceText.text = $"<size=150%>{currentAmount}<size=80%>/{price}";
        }

        public void SetResourceSprite(Sprite sprite)
        {
            _resourceImage.sprite = sprite;
        }
    }

}
