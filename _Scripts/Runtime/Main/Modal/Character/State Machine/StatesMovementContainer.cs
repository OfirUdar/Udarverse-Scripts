using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Character
{
    [System.Serializable]
    public class StatesMovementContainer
    {
        public GroundState GroundState;
        public JumpState JumpState;
        public FallState FallState;
        [Space]
        public IdleState IdleState;
        public WalkState WalkState;
        public RunState RunState;

        [Space]
        public CrouchState CrouchState;
        public CrouchIdleState CrouchIdleState;
        public CrouchWalkState CrouchWalkState;

        [Space]
        public SwimState SwimState;
        public SwimIdleState SwimIdleState;
        public SwimWalkState SwimWalkState;
        [Space]
        public HitState HitState;
        public void Init(CharacterMovementMachine context)
        {

            GroundState.Init(context);
            JumpState.Init(context);
            FallState.Init(context);

            IdleState.Init(context);
            WalkState.Init(context);
            RunState.Init(context);


            CrouchState.Init(context);
            CrouchIdleState.Init(context);
            CrouchWalkState.Init(context);

            SwimState.Init(context);
            SwimIdleState.Init(context);
            SwimWalkState.Init(context);


            HitState.Init(context);
        }
    }

}
