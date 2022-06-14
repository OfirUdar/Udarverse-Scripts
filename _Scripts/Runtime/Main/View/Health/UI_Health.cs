using System.Collections;
using UnityEngine;

namespace Udarverse.UI
{
    using Udar.DesignPatterns.UdarPool;
    using UnityEngine.UI;

    public class UI_Health : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _healthBarOb;
        [SerializeField] private Image _healthBar;

        private Coroutine _coroutine;
        private void Awake()
        {
            _health.OnHealthChangedNormalized += Event_OnHealthChangedNormalized;
        }

        private void OnDestroy()
        {
            _health.OnHealthChangedNormalized -= Event_OnHealthChangedNormalized;
        }



        private IEnumerator DisplayHealthBar(float normalizedHealth)
        {
            _healthBarOb.SetActive(true);
            _healthBar.fillAmount = normalizedHealth;
            yield return UdarPool.Instance.GetWaitForSeconds(2f);
            _healthBarOb.SetActive(false);

        }


        private void Event_OnHealthChangedNormalized(float normalizedHealth)
        {
            if(isActiveAndEnabled)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(DisplayHealthBar(normalizedHealth));
            }
            else
                _healthBar.fillAmount = normalizedHealth;

        }
    }

}
