using System;
using System.Collections;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class CrouchState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _heightOnCrouch = 1.5f;
        private float _originalHeightCollider;
        public override void Init(CharacterMovementMachine context)
        {
            base.Init(context);
            _originalHeightCollider = _ctx.CharacterController.height;
        }


        #region State Methods
        protected override void Setup()
        {
            RequirementParentList.Add(typeof(GroundState));
            SubStateDefualt=_ctx.States.CrouchIdleState;
        }
        public override void OnStateEnter()
        {
            _ctx.CharacterAnimation.SetIsCrouching(true);
            SetLowHeight();
            _ctx.InputCharacter.SetIsCrouching(true);
            _ctx.InputCharacter.OnCrouchChanged += Event_OnCrouchChanged;
        }
        public override void OnStateExit()
        {
            _ctx.CharacterAnimation.SetIsCrouching(false);
            SetOriginalHeight();
            _ctx.InputCharacter.OnCrouchChanged -= Event_OnCrouchChanged;
            _ctx.InputCharacter.SetIsCrouching(false);
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStateUpdate()
        {


        }
        public override void CheckChangeStates()
        {
            if (_ctx.InputCharacter.IsRunning())
            {
                var runState = _ctx.States.RunState;
                if (TryTransit(runState))
                    return;
            }
        }

        #endregion


        private void Event_OnCrouchChanged(bool isCrouched)
        {
            if (!isCrouched)
            {
                if (_ctx.InputCharacter.GetMovement() == Vector3.zero)
                {
                    var idleState = _ctx.States.IdleState;
                    if (TryTransit(idleState))
                        return;
                }
                else
                {
                    var walkState = _ctx.States.WalkState;
                    if (TryTransit(walkState))
                        return;
                }

            }
        }

        private void SetLowHeight()
        {
            _ctx.CharacterController.height = _heightOnCrouch;
            Vector3 center = _ctx.CharacterController.center;
            center.y = _heightOnCrouch / 2f;
            _ctx.CharacterController.center = center;
        }
        private void SetOriginalHeight()
        {
            _ctx.CharacterController.height = _originalHeightCollider;
            Vector3 center = _ctx.CharacterController.center;
            center.y = _originalHeightCollider / 2f;
            _ctx.CharacterController.center = center;
        }


    }
}