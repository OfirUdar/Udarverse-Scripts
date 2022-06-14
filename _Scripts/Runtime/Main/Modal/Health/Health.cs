using System;
using Udar.Utils.Timer;
using Udar.Utils.Value;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Editable]
        [SerializeField] protected int _maxHealth;
        protected UdarValue<int> _currentHealth;

        private Transform _hitOB;

        public event Action<float> OnHealthChangedNormalized;
        public event Action<int, Transform> OnHealthChanged;
        public event Action<int, int> OnHealthChangedWithCompare;//returns with old and new
        public UnityEvent OnDied;
        public UnityEvent OnFullHealed;


        private bool _isCalledOnDied;
        private UdarTimer _timer;

        protected virtual void Awake()
        {
            _currentHealth = new UdarValue<int>(OnHealthValueChanged);
            _timer = new UdarTimer(this, 3f).Start();
        }
        protected virtual void OnEnable()
        {
            HealFull();
        }
        public virtual void SetMaxHealth(int maxHealth) => _maxHealth = maxHealth;
        public int GetMaxHealth() => _maxHealth;
        public float GetNormalizedHealth() => (float)_currentHealth.Value / (float)_maxHealth;

        public virtual void HealFull()
        {
            Heal(_maxHealth);
            if (_timer.IsFinished)
                OnFullHealed?.Invoke();
        }
        public void Heal(int healthAmount)
        {
            _currentHealth.Value = Mathf.Max(0, _currentHealth.Value + healthAmount);
            _isCalledOnDied = false;
        }
        public virtual void TakeDamage(int damage, Transform hitOb = null)
        {
            _hitOB = hitOb;
            _currentHealth.Value = Mathf.Max(0, _currentHealth.Value - damage);
        }


        protected virtual void OnHealthValueChanged(int oldHealth, int newHealth)
        {
            if (newHealth <= 0 && !_isCalledOnDied)
            {
                OnDied?.Invoke();
                _isCalledOnDied = true;
            }

            if (newHealth < oldHealth && _hitOB != null)
            {
                OnHealthChanged?.Invoke(newHealth, _hitOB);
                OnHealthChangedWithCompare?.Invoke(oldHealth, newHealth);
            }

            OnHealthChangedNormalized?.Invoke((float)newHealth / (float)_maxHealth);
        }
    }
}

