
using TMPro;
using Udarverse.Player.Stats;
using Udarverse.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ResourceProducerPrice : MonoBehaviour
    {
        [Header("Overlay")]
        [SerializeField] private GameObject _overlay;
        [Header("Price")]
        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [Header("Reward")]
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardAmountText;
        [Header("Buy Button")]
        [SerializeField] private Button _buyButton;


        private void OnEnable()
        {
            ResourceProducerMachine.OnPlayerDetected += Event_OnPlayerDetected;
        }
        private void OnDisable()
        {
            ResourceProducerMachine.OnPlayerDetected -= Event_OnPlayerDetected;
        }

        private void Setup(PayAction payAction)
        {
            var priceList = payAction.priceList;

            _priceImage.sprite = priceList[0].resourceSC.sprite;
            _priceText.text = priceList[0].price.ToString();

            bool canPay = PlayerStats.Instance.CanPay(priceList);
            _priceText.color = canPay ? Color.green : Color.red;

            _rewardImage.sprite = payAction.GetRewardSprite();
            _rewardAmountText.text = payAction.amountToProduce.ToString();


            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener
                (delegate
                {
                    payAction.TryPay();
                    bool canPay = PlayerStats.Instance.CanPay(priceList);
                    _priceText.color = canPay ? Color.green : Color.red;
                });

        }


        private void Event_OnPlayerDetected(bool isDetected, Vector3 position, PayAction payAction)
        {
            if (isDetected)
            {
                Setup(payAction);
                var newPosition = position;
                newPosition.y += 3.5f;
                _overlay.transform.position = newPosition;
                _overlay.SetActive(true);
            }
            else
            {
                _overlay.SetActive(false);
            }
        }

    }

}
