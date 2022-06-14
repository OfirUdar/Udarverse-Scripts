using Udarverse.Player.Stats;
using Udarverse.Resources;
using System.Collections;
using TMPro;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ResourceAmount : MonoBehaviour
    {
        [SerializeField] private Image _resourceImage;
        [SerializeField] private TextMeshProUGUI _amountText;

        private ResourceSC _resourceSC;


        private int _currentAmount;
        private int _targetAmount;
        private Coroutine _coroutine;


        private void OnDisable()
        {
            PlayerStats.OnResourceAmountChanged -= Event_OnResourceCollected;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }


        public void Setup(ResourceSC resourceSC,int initAmount)
        {
            _resourceImage.sprite = resourceSC.sprite;
            _resourceSC = resourceSC;
            _currentAmount = initAmount;
            _amountText.text = FuncUI.GetAmountClearableText(initAmount);

            PlayerStats.OnResourceAmountChanged += Event_OnResourceCollected;
        }
        private void Event_OnResourceCollected(ResourceSC resourceSC, int amount)
        {
            if (_resourceSC == resourceSC)
            {
                _targetAmount = amount;
                if (_coroutine == null)
                    _coroutine = StartCoroutine(UpdateTextAnim(0.1f));
            }

        }

        private IEnumerator UpdateTextAnim(float speed)
        {
            while (_currentAmount != _targetAmount)
            {
                if (_targetAmount > _currentAmount)
                    _currentAmount++;
                else
                    _currentAmount--;
                _amountText.text = FuncUI.GetAmountClearableText(_currentAmount);
                yield return UdarPool.Instance.GetWaitForSeconds(speed);
                speed -= Time.deltaTime;
                speed = Mathf.Max(0, speed);
            }

            _coroutine = null;
        }
    }

}
