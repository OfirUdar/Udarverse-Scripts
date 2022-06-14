using System;
using System.Collections;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Resources
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected Health _health;
        [SerializeField] protected Collider _collider;
        [Space]
        [SerializeField] protected GameObject[] _gfxArray = new GameObject[3];
        [SerializeField] protected ResourceSpawnData _resourceSpawnData;
        [SerializeField] protected Vector2 _respawnTimeRandom = new Vector2(2.5f, 5f);

        protected int _currentLevel;
        private float _amountToSpawn;

        private Sprite _resourceSprite;
        private Vector3 _topPosition;


        public UnityEvent<float> OnGetHit;
        public static event Action<Vector3, Sprite,int> OnSpawnResources;

        protected virtual void Awake()
        {
            _currentLevel = _gfxArray.Length - 1;
            _resourceSpawnData.startTransform = transform;
            _resourceSpawnData.target = GameManager.Instance.MainPlayer;

            var min = _resourceSpawnData.spawnAmount.x;
            var max = _resourceSpawnData.spawnAmount.y;

            var maxHealth = _health.GetMaxHealth();
            var minSpawnPerOneDamage = min / maxHealth;
            var maxSpawnPerOneDamage = max / maxHealth;
            _resourceSpawnData.spawnAmount.x = minSpawnPerOneDamage;
            _resourceSpawnData.spawnAmount.y = maxSpawnPerOneDamage;

            _resourceSprite = _resourceSpawnData.resourceSC.sprite;
            _topPosition = _collider.bounds.max;
        }

        protected virtual void OnEnable()
        {
            _health.OnHealthChangedWithCompare += Event_OnHealthChanged;
            _health.OnDied.AddListener(Event_OnDied);
        }

        protected virtual void OnDisable()
        {
            _health.OnHealthChangedWithCompare -= Event_OnHealthChanged;
            _health.OnDied.RemoveListener(Event_OnDied);
        }


        public ResourceSC GetResourceSpawner() => _resourceSpawnData.resourceSC;

        private IEnumerator RespawnUnit()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(UnityEngine.Random.Range(_respawnTimeRandom.x, _respawnTimeRandom.y));
            Restore();
        }

        private void Restore()
        {
            _currentLevel = _gfxArray.Length - 1;
            _health.HealFull();
            _gfxArray[0].SetActive(false);
            _gfxArray[_currentLevel].SetActive(true);
            _collider.enabled = true;
        }
        protected virtual void Event_OnHealthChanged(int oldHealth, int currentHealth)
        {
            var maxHealth = _health.GetMaxHealth();
            var healthPerLevel = (float)(maxHealth / (_gfxArray.Length - 1));

            if (currentHealth <= (_currentLevel - 1) * healthPerLevel)
            {
                _gfxArray[_currentLevel].SetActive(false);
                _currentLevel--;
                if (_currentLevel >= 0)
                    _gfxArray[_currentLevel].SetActive(true);
            }
            if (currentHealth != maxHealth)
            {
                var damageTaken = (oldHealth - currentHealth);
                _amountToSpawn += UnityEngine.Random.Range(_resourceSpawnData.spawnAmount.x, _resourceSpawnData.spawnAmount.y) * damageTaken;
                if (_amountToSpawn > 0.5f)
                {
                    int amountToSpawn = Mathf.RoundToInt(_amountToSpawn);
                    _amountToSpawn = 0;
                    ResourceSpawner.Instance.Spawn(_resourceSpawnData, amountToSpawn);
                    OnSpawnResources?.Invoke(_topPosition, _resourceSprite, amountToSpawn);
                }
                OnGetHit?.Invoke(_health.GetNormalizedHealth());

            }


        }
        protected virtual void Event_OnDied()
        {
            _collider.enabled = false;
            StartCoroutine(RespawnUnit());
        }


    }
}

