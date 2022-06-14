using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class SwimWalkState : MovementStateBase
    {
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(SwimState));
        }
        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() == Vector3.zero || _ctx.InteractiveMachine.IsAttacking)
            {
                var swimIdleState = _ctx.States.SwimIdleState;
                if (TryTransit(swimIdleState))
                    return;
            }
        }

        public override void OnFixedUpdate()
        {
        }


    }
}

