using System;
using Udarverse.Character.Player;
using Udarverse.UI;
using UnityEngine;

namespace Udarverse.Input
{
    public class MobileInput : MonoBehaviour, IUserInput
    {
        [SerializeField] private GameObject _mobileUI;
        [Space]
        [SerializeField] private Joystick _joystick;
        [Space]
        [SerializeField] private UIEventTrigger _fireTrigger;
        [SerializeField] private UIEventTrigger _jumpTrigger;

        private bool _isCrouching;
        public float Vertical => _joystick.Vertical;

        public float Horizontal => _joystick.Horizontal;

        public bool IsRunPressed => (Mathf.Abs(Vertical) >= 0.75 || Mathf.Abs(Horizontal) >= 0.75) && !_isCrouching;

        public Vector2 Movement2D => new Vector2(Horizontal, Vertical);

        public Vector3 Movement3D => new Vector3(Horizontal, 0, Vertical);

        public bool IsFirePressed => _fireTrigger.IsPressed;

        public bool IsJumpPressed => _jumpTrigger.IsPressed;



        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action OnCrouchPressed;

        public void Setup()
        {
            Application.targetFrameRate = 60;
            _mobileUI.SetActive(true);
        }
        private void OnEnable()
        {
            UI_Item.OnItemSelected += Event_OnItemSelected;
            PlayerInput.OnCrouchStateChanged += Event_OnCrouchStateChanged;
        }


        private void OnDisable()
        {
            UI_Item.OnItemSelected -= Event_OnItemSelected;
            PlayerInput.OnCrouchStateChanged -= Event_OnCrouchStateChanged;
        }

        public void Crouch_Button()
        {
            OnCrouchPressed?.Invoke();
        }

        private void Event_OnItemSelected(int index)
        {
            OnItemSelected?.Invoke(index);
        }
        private void Event_OnCrouchStateChanged(bool isCrouching)
        {
            _isCrouching = isCrouching;
        }

    }
}