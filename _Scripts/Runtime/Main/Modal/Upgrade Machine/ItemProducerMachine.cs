using System;
using Udarverse.Inventory;
using UnityEngine;

namespace Udarverse.Resources
{
    public class ItemProducerMachine : UpgradeMachineBase
    {
        [Editable]
        [SerializeField] private ItemPayAction _itemPayAction;

        public static Action<bool, Vector3, PayAction> OnPlayerDetected;


        private void Start()
        {
            var item = _itemPayAction.itemSC;

            CheckIsLevelIsExsist(item);
        }

        private void CheckIsLevelIsExsist(ItemSC item)
        {
            if (PlayerItemsInventory.Instance.GetLevel(item) >= item.GetMaxLevel())
            {
                this.gameObject.SetActive(false);
            }
        }

        protected override PayAction GetPayAction()
        {
            return _itemPayAction;
        }

        protected override void OnOneProduceFinish()
        {
            var item = _itemPayAction.itemSC;
            PlayerItemsInventory.Instance.LevelUp(item);

            CheckIsLevelIsExsist(item);
        }

        protected override void OnPlayerInPayRegion(bool isDetected)
        {
            if (isDetected)
            {
                if (!IsProducing)
                    OnPlayerDetected?.Invoke(true, transform.position, GetPayAction());
            }
            else
            {
                OnPlayerDetected?.Invoke(false, transform.position, null);
            }
        }
    }
}