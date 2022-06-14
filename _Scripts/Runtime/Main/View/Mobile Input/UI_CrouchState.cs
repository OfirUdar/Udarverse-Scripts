using Udarverse.Character.Player;
using UnityEngine;

namespace Udarverse.UI
{
    public class UI_CrouchState : MonoBehaviour
    {
        [SerializeField] private GameObject _iconOn;
        private void OnEnable()
        {
            PlayerInput.OnCrouchStateChanged += Event_OnCrouchStateChanged;
        }

        private void OnDisable()
        {
            PlayerInput.OnCrouchStateChanged -= Event_OnCrouchStateChanged;
        }

        private void Event_OnCrouchStateChanged(bool isCrouch)
        {
            _iconOn.SetActive(isCrouch);
        }

    }
}