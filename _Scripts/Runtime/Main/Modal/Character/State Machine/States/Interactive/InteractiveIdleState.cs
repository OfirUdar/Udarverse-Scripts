using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class InteractiveIdleState : CharacterStateBase<CharacterInteractiveMachine>
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
        }

        public override void OnStateExit()
        {
        }

        public override void OnStateUpdate()
        {

        }
        public override void CheckChangeStates()
        {
            if (CanAttack())
            {
                if (TryTransit(_ctx.States.AttackState))
                    return;

            }
        }

        private bool CanAttack()
        {
            var isInputAttack = _ctx.InputCharacter.IsAttacking();
            if (isInputAttack)
                return _ctx.MovementMachine.IsCurrentStateAttackable;
            return false;
        }

    }
}


