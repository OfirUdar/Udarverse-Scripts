using System;
using Udarverse.Inventory;
using UnityEngine;

namespace Udarverse
{
    public class UnitHealth : Health
    {
        [Space]
        [SerializeField] private Collider _collider;
        [SerializeField] private ItemSC _item;
        private int _minDamage;

        public static event Action<Vector3,Sprite> OnTakingDamageFailed;

        public override void HealFull()
        {
            base.HealFull();
            _minDamage = _maxHealth * 8 / 100;
        }
        public override void SetMaxHealth(int maxHealth)
        {
            base.SetMaxHealth(maxHealth);
            _minDamage = _maxHealth * 8 / 100;
        }

        public void SetMinimumDamage(int minDamage)
        {
            _minDamage = minDamage;
        }
        public override void TakeDamage(int damage, Transform hitOb = null)
        {
            if (_minDamage > damage)
            {
                OnTakingDamageFailed?.Invoke(_collider.bounds.max, _item.sprite);
                return;
            }
            base.TakeDamage(damage, hitOb);
        }


    }

}
