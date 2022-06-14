using System;
using Udarverse.Player.Stats;
using UnityEngine;

namespace Udarverse.Resources
{
    public class ResourceProducerMachine : UpgradeMachineBase
    {
        [Editable]
        [SerializeField] private ResourcePayAction _resourceToProduce;

        public static Action<bool, Vector3, PayAction> OnPlayerDetected;

        protected override PayAction GetPayAction()
        {
            return _resourceToProduce;
        }

        protected override void OnOneProduceFinish()
        {
            PlayerStats.Instance.AddResource(_resourceToProduce.resourceSC, 1);
        }

        protected override void OnPlayerInPayRegion(bool isDetected)
        {
            if (isDetected)
            {
                OnPlayerDetected?.Invoke(true, transform.position, GetPayAction());
            }
            else
            {
                OnPlayerDetected?.Invoke(false, transform.position, null);
            }
        }
    }
}
