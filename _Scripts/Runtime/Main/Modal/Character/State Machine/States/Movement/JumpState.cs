using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class JumpState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private float _gravity = -9.81f;
        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
            SubStateDefualt=_ctx.States.IdleState;
        }


        public override void OnFixedUpdate()
        {

        }

        public override void OnStateEnter()
        {
            _ctx.VelocityY = _jumpHeight;
            _ctx.CharacterAnimation.TriggerJump();
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            _ctx.VelocityY += _gravity * Time.deltaTime;
        }
        public override void CheckChangeStates()
        {
            if (_ctx.VelocityY < 0)
            {
                var fallState = _ctx.States.FallState;
                if (TryTransit(fallState))
                    return;
            }
        }


    }
}

