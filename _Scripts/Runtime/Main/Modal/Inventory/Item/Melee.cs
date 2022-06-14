
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Inventory.Damager.Melee
{
    public class Melee : ItemDamager
    {
        [SerializeField] protected float _radius = .5f;

        private readonly List<Collider> _collidersTookHit = new List<Collider>();


        protected override void Use()
        {
            _collidersTookHit.Clear();
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            float timer = _fireRate;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                HandleCollision();
                yield return null;
            }
            _onUsed.Invoke();
        }
        private void HandleCollision()
        {
            //Set some offset to the sphere collider:
            var position = _owner.position;
            position.z += _radius * .8f * _owner.forward.z;
            position.x += _radius * .8f * _owner.forward.x;

            //Find colliders:
            var colliders = Physics.OverlapSphere(position, _radius, _targetsLayer);
            foreach (var coll in colliders)
            {
                if (coll.transform == _owner)//Player cannot damage to himself
                    continue;
                if (_collidersTookHit.Contains(coll))
                    continue;
                else
                {
                    if (coll.TryGetComponent(out IDamageable damageable))
                    {
                        _collidersTookHit.Add(coll);
                        damageable.TakeDamage(_damage, _owner);
                    }
                }
            }
        }
    }
}