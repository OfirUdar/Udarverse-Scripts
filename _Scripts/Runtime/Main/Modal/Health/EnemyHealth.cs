
using System.Collections;
using Udar.DesignPatterns.UdarPool;
using Udar.Utils;
using Udarverse.Character;
using Udarverse.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse
{
    public class EnemyHealth : Health
    {
        [Header("Enemy Health")]
        [SerializeField] protected ResourceSpawnData _resourceSpawnData;
        [Space]
        [SerializeField] protected Vector2 _respawnTimeRandom = new Vector2(2.5f, 5f);
        [SerializeField] protected EnemyInput _enemyInput;
        [Space]
        public UnityEvent OnGetHit;


        protected override void Awake()
        {
            base.Awake();
            _resourceSpawnData.startTransform = transform;
            _resourceSpawnData.target = GameManager.Instance.MainPlayer;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            OnDied.AddListener(Event_OnDied);
            OnHealthChanged += Event_OnHealthChanged;
        }



        private void OnDisable()
        {
            OnDied.RemoveListener(Event_OnDied);
            OnHealthChanged -= Event_OnHealthChanged;
        }

        private IEnumerator Respawn()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(Random.Range(_respawnTimeRandom.x, _respawnTimeRandom.y));
            transform.position = _enemyInput.StartPosition;
            gameObject.SetActive(true);
        }
        private void Event_OnDied()
        {
            ResourceSpawner.Instance.Spawn(_resourceSpawnData);
            UdarCoroutines.Instance.Begin(Respawn());
            gameObject.SetActive(false);
        }

        private void Event_OnHealthChanged(int currentHealth, Transform hitOb)
        {
            if (currentHealth < _maxHealth)
                OnGetHit?.Invoke();
        }
    }

}
