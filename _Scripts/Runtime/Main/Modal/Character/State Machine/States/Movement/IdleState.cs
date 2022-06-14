using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class IdleState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _speedRotate;
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(GroundState));
            RequirementParentList.Add(typeof(JumpState));
            RequirementParentList.Add(typeof(FallState));
        }

        public override void OnStateEnter()
        {
            _ctx.Movement = Vector3.zero;
            _ctx.SpeedRotate = _speedRotate;
            _ctx.InputCharacter.OnCrouchChanged += Event_OnCrouchChanged;
        }

        public override void OnStateExit()
        {
            _ctx.InputCharacter.OnCrouchChanged -= Event_OnCrouchChanged;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnStateUpdate()
        {
            _ctx.CharacterAnimation.SetVelocities(0, 0);
            _ctx.Rotation=_ctx.InputCharacter.GetRotation();
        }

        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() != Vector3.zero)
            {
                var walkState = _ctx.States.WalkState;
                if (TryTransit(walkState))
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
