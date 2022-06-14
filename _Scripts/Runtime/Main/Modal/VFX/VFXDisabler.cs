using UnityEngine;

namespace Udarverse.VFX
{
    public class VFXDisabler : MonoBehaviour
    {
        [SerializeField] private float _timeToDisable = .5f;

        private void OnEnable()
        {
            Invoke(nameof(Disable), _timeToDisable);
        }
        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }

}
