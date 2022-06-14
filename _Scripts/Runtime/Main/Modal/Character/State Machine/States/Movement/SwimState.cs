using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class SwimState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private LayerMask _groundLayer;

        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
            SubStateDefualt = _ctx.States.SwimIdleState;
        }
        public override void CheckChangeStates()
        {

        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnStateEnter()
        {
            _ctx.CharacterAnimation.SetIsGrounded(true);
            _ctx.CharacterAnimation.SetIsSwimming(true);
            _ctx.OnControllerColldided += Event_OnControllerColldided;
        }


        public override void OnStateExit()
        {
            _ctx.CharacterAnimation.SetIsSwimming(false);
            _ctx.OnControllerColldided -= Event_OnControllerColldided;
        }

        public override void OnStateUpdate()
        {

        }
        private void Event_OnControllerColldided(ControllerColliderHit hit)
        {
            if (((1 << hit.gameObject.layer) & _groundLayer) != 0)
            {
                var groundState = _ctx.States.GroundState;
                if (TryTransit(groundState))
                {
                    return;
                }
            }
        }


    }
}

