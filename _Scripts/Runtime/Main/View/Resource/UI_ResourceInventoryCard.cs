
using TMPro;
using Udarverse.Resources;
using Udarverse.Resources.Navigator;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ResourceInventoryCard : MonoBehaviour
    {
        [SerializeField] private Image _resourceImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private GameObject _fillImage;

        private ResourceSC _resourceSC;
        public void Setup(ResourceSC resourceSC, int amount)
        {
            _resourceSC = resourceSC;
            _resourceImage.sprite = resourceSC.sprite;
            _amountText.text = amount.ToString();
        }

        public void Navigate_Button()
        {
            if(_fillImage.activeSelf)
            {
                ResourceNavigator.Instance.FinishNavigator();
                return;
            }
            var isFound = ResourceNavigator.Instance.SearchForTarget(_resourceSC);

            if (isFound)
            {
                _fillImage.SetActive(true);
                ResourceNavigator.Instance.OnNavigatorStatusChanged += Event_OnNavigatorStatusChanged;
            }

        }

        private void Event_OnNavigatorStatusChanged(bool isNavigatorActive)
        {
            if (!isNavigatorActive)
            {
                _fillImage.SetActive(false);
                ResourceNavigator.Instance.OnNavigatorStatusChanged -= Event_OnNavigatorStatusChanged;
            }
        }
    }

}
