
using UnityEngine;

namespace Udarverse.Inventory
{
    public abstract class ItemDamager : ItemBase
    {
        [SerializeField] protected int _damage;
        [SerializeField] protected float _fireRate;
        [SerializeField] protected LayerMask _targetsLayer;


    }
}