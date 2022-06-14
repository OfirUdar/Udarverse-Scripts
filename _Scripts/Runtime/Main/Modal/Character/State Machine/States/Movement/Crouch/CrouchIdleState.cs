using System;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class CrouchIdleState : IdleState
    {

        protected override void Setup()
        {
            RequirementParentList.Add(typeof(CrouchState));
        }
        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() != Vector3.zero)
            {
                var crouchWalkState = _ctx.States.CrouchWalkState;
                if (TryTransit(crouchWalkState))
                    return;
            }
        }
    }
}

