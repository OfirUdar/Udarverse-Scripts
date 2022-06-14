using System;
using UnityEngine;

namespace Udarverse.Character
{
    public interface IInputCharacter 
    {
        public Vector3 GetMovement();
        public Vector3 GetRotation();
        public bool IsJumping();
        public bool IsRunning();
        public void SetIsCrouching(bool isCrouching);
        public bool IsAttacking();

        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action<bool> OnCrouchChanged;


    }
}