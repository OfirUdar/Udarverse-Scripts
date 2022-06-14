using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class WalkState : MovementStateBase
    {

        protected override void Setup()
        {
            RequirementParentList.Add(typeof(GroundState));
            RequirementParentList.Add(typeof(JumpState));
            RequirementParentList.Add(typeof(FallState));
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _ctx.InputCharacter.OnCrouchChanged += Event_OnCrouchChanged;
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
            _ctx.InputCharacter.OnCrouchChanged -= Event_OnCrouchChanged;
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
            if (_ctx.InputCharacter.IsRunning() && !_ctx.InteractiveMachine.IsAttacking)
            {
                var runState = _ctx.States.RunState;

                if (TryTransit(runState))
                    return;
            }
        }


        private void Event_OnCrouchChanged(bool isCrouched)
        {
            if (!isCrouched)
                return;

            var crouchState = _ctx.States.CrouchState;
            if (TryTransit(crouchState))
                return;

        }
    }

}

