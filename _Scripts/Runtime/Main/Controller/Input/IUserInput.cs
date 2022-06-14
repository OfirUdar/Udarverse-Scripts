using System;
using System.Collections;
using UnityEngine;

namespace Udarverse.Input
{
    public interface IUserInput
    {
        public float Vertical { get; }
        public float Horizontal { get; }
        public bool IsRunPressed { get; }

        public Vector2 Movement2D { get; }
        public Vector3 Movement3D { get; }

        public bool IsFirePressed { get; }
        public bool IsJumpPressed { get; }

        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action OnCrouchPressed;
        public void Setup();



    }
}