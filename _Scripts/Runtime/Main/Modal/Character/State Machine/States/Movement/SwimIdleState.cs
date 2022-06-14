using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class SwimIdleState : IdleState
    {
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(SwimState));
        }
        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() != Vector3.zero && !_ctx.InteractiveMachine.IsAttacking)
            {
                var swimWalkState = _ctx.States.SwimWalkState;
                if (TryTransit(swimWalkState))
                    return;
            }
        }

    }
}

