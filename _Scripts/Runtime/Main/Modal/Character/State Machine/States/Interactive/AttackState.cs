using Udarverse.Inventory;
using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class AttackState : CharacterStateBase<CharacterInteractiveMachine>
    {

        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStateEnter()
        {
            _ctx.IsAttacking = true;
            
            _ctx.CharacterAnimation.TriggerAttack();

            var item = _ctx.InventoryManager.GetCurrentItem();
            item.Use(EndAttack);
        }

        public override void OnStateExit()
        {
            _ctx.IsAttacking = false;
        }

        public override void CheckChangeStates()
        {

        }

        public override void OnStateUpdate()
        {

        }
        private void EndAttack()
        {
            if (TryTransit(_ctx.States.IdleState))
                return;
        }

    }
}

