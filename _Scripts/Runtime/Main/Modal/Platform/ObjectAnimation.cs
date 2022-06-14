
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Object
{
    public class ObjectAnimation : MonoBehaviour
    {
        private enum PlayTime
        {
            None,
            Awake,
            Start,
            OnEnable,
        }

        [SerializeField] private PlayTime _playTime = PlayTime.OnEnable;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _speed = 1f;

        public UnityEvent OnAnimationEnd;

        private void Awake()
        {
            if (_playTime == PlayTime.Awake)
            {
                PlayAnimation();
            }
        }
        private void Start()
        {
            if (_playTime == PlayTime.Start)
            {
                PlayAnimation();
            }
        }
        private void OnEnable()
        {
            if (_playTime == PlayTime.OnEnable)
            {
                PlayAnimation();
            }

        }

        //It can called from Unity Event
        public void PlayAnimation()
        {
            StartCoroutine(PlayAnimationCoroutine());
        }
        public void PlayRewindAnimation()
        {
            StartCoroutine(PlayRewindAnimationCoroutine());
        }
        private IEnumerator PlayAnimationCoroutine()
        {
            var timer = 0f;
            var startKey = _animationCurve.keys[0];
            var lastKey = _animationCurve.keys[_animationCurve.keys.Length - 1];

            transform.localScale = Vector3.one * startKey.value;

            while (timer < lastKey.time)
            {
                timer += Time.deltaTime * _speed;
                transform.localScale = Vector3.one * _animationCurve.Evaluate(timer);
                yield return null;
            }
            OnAnimationEnd?.Invoke();
        }
        private IEnumerator PlayRewindAnimationCoroutine()
        {
            var startKey = _animationCurve.keys[0];
            var lastKey = _animationCurve.keys[_animationCurve.keys.Length - 1];
            var timer = lastKey.time;

            transform.localScale = Vector3.one * lastKey.value;

            while (timer >= startKey.time)
            {
                timer -= Time.deltaTime * _speed;
                transform.localScale = Vector3.one * _animationCurve.Evaluate(timer);
                yield return null;
            }
            OnAnimationEnd?.Invoke();
        }

    }

}
