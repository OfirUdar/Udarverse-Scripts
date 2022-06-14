using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class CrouchWalkState : MovementStateBase
    {
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(CrouchState));
        }

        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.GetMovement() == Vector3.zero)
            {
                var crouchIdleState = _ctx.States.CrouchIdleState;
                if (TryTransit(crouchIdleState))
                    return;
            }
        }

        public override void OnFixedUpdate()
        {
        }

       
    }
}

