
using System.Collections;
using Udar.DesignPatterns.UdarPool;
using Udar.Utils;
using Udarverse.Save;
using UnityEngine;

namespace Udarverse
{
    public class PlayerHealth : Health
    {
        [SerializeField] private int _healAmount = 1;

        private Coroutine _coroutine;

        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            OnDied.AddListener(Event_OnDied);
        }


        private void OnDisable()
        {
            OnDied.RemoveListener(Event_OnDied);
        }



        protected override void OnHealthValueChanged(int oldHealth, int newHealth)
        {
            base.OnHealthValueChanged(oldHealth, newHealth);

            if (newHealth < oldHealth && newHealth > 0)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(Heal());
            }
        }

        private IEnumerator Heal()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(2f);
            while (_currentHealth.Value < _maxHealth)
            {
                Heal(_healAmount);
                yield return UdarPool.Instance.GetWaitForSeconds(.07f);
            }
        }


        private IEnumerator Respawn()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(2f);
            GameSaveManager.Instance.LoadPlayerPosition();
            gameObject.SetActive(true);
        }

        private void Event_OnDied()
        {
            UdarCoroutines.Instance.Begin(Respawn());
            gameObject.SetActive(false);

        }

    }

}
