
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ProduceProgress : MonoBehaviour
    {
        [SerializeField] private UpgradeMachineBase _upgradeMachine;
        [SerializeField] private GameObject _progressBarObject;
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _currentAmountText;

        private void OnEnable()
        {
            _upgradeMachine.OnProgressingProduce += Event_OnProducingProgress;
            _upgradeMachine.OnProducingStateChanged += Event_OnProducingStateChanged;
        }
        private void OnDisable()
        {
            _upgradeMachine.OnProgressingProduce -= Event_OnProducingProgress;
            _upgradeMachine.OnProducingStateChanged -= Event_OnProducingStateChanged;
        }


        private void Start()
        {
            _rewardImage.sprite = _upgradeMachine.GetRewardSprite();
        }

        private void Event_OnProducingStateChanged(bool isActive, Sprite sprite)
        {
            if (isActive)
                _rewardImage.sprite = sprite;
            _progressBarImage.fillAmount = 0;
            _currentAmountText.gameObject.SetActive(isActive);
        }
        private void Event_OnProducingProgress(float normalizedAmount, int currentAmount)
        {
            _progressBarImage.fillAmount = normalizedAmount;
            _currentAmountText.text = currentAmount > 1 ? currentAmount.ToString() : "";
        }

    }
}

