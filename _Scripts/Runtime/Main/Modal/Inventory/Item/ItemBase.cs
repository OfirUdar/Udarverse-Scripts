using System;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Inventory
{
    public abstract class ItemBase : MonoBehaviour, IUseable
    {
        [SerializeField] protected ItemEquipPoint _equipPoint;
        [Space]
        [SerializeField] protected int _animationLayer;

        public UnityEvent OnUsed;

        protected Transform _owner;
        protected Action _onUsed;

        protected abstract void Use();

        private void OnDisable()
        {
            _onUsed?.Invoke();
        }

        public void SetOwner(Transform owner)
        {
            _owner = owner;
            transform.localPosition = _equipPoint.position;
            transform.localRotation = Quaternion.Euler(_equipPoint.rotation);
        }
        public int GetAnimationLayer()
        {
            return _animationLayer;
        }

        public virtual void Equip(Transform parent, Transform owner)
        {
            _owner = owner;
            transform.SetParent(parent);
            transform.localPosition = _equipPoint.position;
            transform.localEulerAngles = _equipPoint.rotation;
        }

        public void Use(Action onUsed)
        {
            _onUsed = onUsed;
            Use();
            OnUsed?.Invoke();
        }

    }
}

