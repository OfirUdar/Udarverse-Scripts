using UnityEngine;
using UnityEngine.EventSystems;

namespace Udarverse.Input
{
    public class UIEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }


        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }

    }
}