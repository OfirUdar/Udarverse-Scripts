using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class FallState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _gravity = -9.81f;

        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
            SubStateDefualt=_ctx.States.IdleState;
        }
        public override void OnStateEnter()
        {
            _ctx.CharacterAnimation.SetIsFalling(true);
        }

        public override void OnStateExit()
        {
            _ctx.CharacterAnimation.SetIsFalling(false);
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStateUpdate()
        {
            _ctx.VelocityY += _gravity * Time.deltaTime;
        }
        public override void CheckChangeStates()
        {
            if (_ctx.CharacterController.isGrounded)
            {
                var groundState = _ctx.States.GroundState;
                if (TryTransit(groundState))
                    return;
            }
        }


    }
}
