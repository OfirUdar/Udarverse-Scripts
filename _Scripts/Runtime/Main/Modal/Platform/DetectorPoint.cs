
using System;
using UnityEngine;

namespace Udarverse
{
    public class DetectorPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayer;

        public event Action<Collider> OnEnterDetect;
        public event Action<Collider> OnExitDetect;
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _collisionLayer) != 0)
            {
                OnEnterDetect?.Invoke(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _collisionLayer) != 0)
            {
                OnExitDetect?.Invoke(other);
            }
        }

    }
}

