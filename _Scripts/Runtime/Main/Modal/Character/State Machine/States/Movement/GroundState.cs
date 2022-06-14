using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class GroundState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private LayerMask _waterLayer;

        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
            SubStateDefualt = _ctx.States.IdleState;
        }
        public override void OnStateEnter()
        {
            _ctx.VelocityY = -0.5f;
            _ctx.CharacterAnimation.SetIsGrounded(true);
            _ctx.OnControllerColldided += Event_OnControllerColldided;
            _ctx.Health.OnHealthChanged += Event_OnHealthChanged;
        }



        public override void OnStateExit()
        {
            _ctx.CharacterAnimation.SetIsGrounded(false);
            _ctx.OnControllerColldided -= Event_OnControllerColldided;
            _ctx.Health.OnHealthChanged -= Event_OnHealthChanged;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStateUpdate()
        {


        }
        public override void CheckChangeStates()
        {
            if (!_ctx.CharacterController.isGrounded)
            {
                var fallState = _ctx.States.FallState;

                if (TryTransit(fallState))
                    return;
            }
            if (_ctx.InputCharacter.IsJumping())
            {
                var jumpState = _ctx.States.JumpState;

                if (TryTransit(jumpState))
                    return;
            }

        }

        private void Event_OnControllerColldided(ControllerColliderHit hit)
        {
            if (((1 << hit.gameObject.layer) & _waterLayer) != 0)
            {
                var swimState = _ctx.States.SwimState;

                if (TryTransit(swimState))
                    return;
            }
        }
        private void Event_OnHealthChanged(int health, Transform hitOb)
        {
            var hitState = _ctx.States.HitState;

            if (TryTransit(hitState))
            {
                hitState.StartGetHit(hitOb);
                return;
            }
        }
    }
}