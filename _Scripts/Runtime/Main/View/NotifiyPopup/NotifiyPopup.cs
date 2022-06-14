using System.Collections;
using System.Collections.Generic;
using TMPro;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class NotifiyPopup : MonoBehaviour
    {
        [SerializeField] private Animation _animation;
        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        // private static List<Vector3> _currentDisplayPositions= new List<Vector3>();
        private Coroutine _coroutine;
        public void Display(Sprite sprite, string text)
        {
            _image.sprite = sprite;
            _text.text = text;
            Display();
        }

        public void Display()
        {
            //if (_currentDisplayPositions.Contains(transform.position))
            //{
            //    FinishDisplay();
            //    return;
            //}

            //_currentDisplayPositions.Add(transform.position);
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _animation.Play(PlayMode.StopAll);
            _coroutine = StartCoroutine(EndDisplay(_animation.clip.length));
        }

        public NotifiyPopup SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }
        public NotifiyPopup SetColor(Color color)
        {
            _image.color = color;
            return this;
        }

        private IEnumerator EndDisplay(float delay)
        {
            yield return UdarPool.Instance.GetWaitForSeconds(delay);
            FinishDisplay();
            _coroutine = null;
        }
        private void FinishDisplay()
        {
            //_currentDisplayPositions.Remove(transform.position);
            gameObject.SetActive(false);
        }

    }
}

