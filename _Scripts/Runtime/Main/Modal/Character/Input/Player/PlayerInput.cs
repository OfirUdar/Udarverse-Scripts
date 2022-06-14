using System;
using System.Collections;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;

namespace Udarverse.Character.Player
{
    public class PlayerInput : MonoBehaviour, IInputCharacter
    {
        private bool _isCrouching;

        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action<bool> OnCrouchChanged;


        public static event Action<bool> OnCrouchStateChanged;

        public Vector3 GetMovement()
        {
            return InputManager.Ctx.Movement3D;
        }
        public Vector3 GetRotation()
        {
            return InputManager.Ctx.Movement3D;
        }
        public bool IsJumping()
        {
            return InputManager.Ctx.IsJumpPressed;
        }
        public bool IsRunning()
        {
            return InputManager.Ctx.IsRunPressed;
        }
        public void SetIsCrouching(bool isCrouching)
        {
            _isCrouching = isCrouching;
            OnCrouchStateChanged?.Invoke(_isCrouching);
        }
        private void ChangeCrouchState()
        {
            OnCrouchChanged?.Invoke(!_isCrouching);
        }

        public bool IsAttacking()
        {
            return InputManager.Ctx.IsFirePressed;
        }

        private void OnEnable()
        {
            StartCoroutine(SubscribeDelay());
        }
        private void OnDisable()
        {
            InputManager.Ctx.OnItemNextPrev -= OnItemNextPrev;
            InputManager.Ctx.OnItemSelected -= OnItemSelected;
            InputManager.Ctx.OnCrouchPressed -= ChangeCrouchState;
        }

        private IEnumerator SubscribeDelay()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(1f);
            InputManager.Ctx.OnItemNextPrev += OnItemNextPrev;
            InputManager.Ctx.OnItemSelected += OnItemSelected;
            InputManager.Ctx.OnCrouchPressed += ChangeCrouchState;
        }

    }
}