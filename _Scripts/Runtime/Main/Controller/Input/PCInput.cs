
namespace Udarverse.Input
{
    using System;
    using UnityEngine;

    public class PCInput : MonoBehaviour, IUserInput
    {
        public float Vertical => Input.GetAxisRaw("Vertical");

        public float Horizontal => Input.GetAxisRaw("Horizontal");

        public bool IsRunPressed => Input.GetKey(KeyCode.LeftShift);

        public Vector2 Movement2D => new Vector2(Horizontal, Vertical);

        public Vector3 Movement3D => new Vector3(Horizontal, 0, Vertical);

        public bool IsFirePressed => Input.GetMouseButton(0);

        public bool IsJumpPressed => Input.GetKeyDown(KeyCode.Space);



        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action OnCrouchPressed;

        public void Setup()
        {
           
        }
        private void Update()
        {
            if(Input.mouseScrollDelta.y!=0)
            {
                var isNextItem = Input.mouseScrollDelta.y > 0;
                OnItemNextPrev?.Invoke(isNextItem);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                OnCrouchPressed?.Invoke();
            }
        }

    }
}