using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class RunState : MovementStateBase
    {
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(GroundState));
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _ctx.CharacterAnimation.SetIsRunning(true);
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
            _ctx.CharacterAnimation.SetIsRunning(false);
        }

        public override void OnFixedUpdate()
        {
        }


        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() == Vector3.zero)
            {
                var idleState = _ctx.States.IdleState;
                if (TryTransit(idleState))
                    return;
            }
            if (!_ctx.InputCharacter.IsRunning() || _ctx.InteractiveMachine.IsAttacking)
            {
                var walkState = _ctx.States.WalkState;
                if (TryTransit(walkState))
                    return;
            }

        }


    }
}

