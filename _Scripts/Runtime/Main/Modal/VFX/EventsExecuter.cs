using UnityEngine;

namespace Udarverse
{
    public abstract class EventsExecuter : MonoBehaviour
    {
        private enum PlayTime
        {
            None,
            Awake,
            Start,
            OnEnable,
            OnDisable,
            OnDestroy,
        }
        [SerializeField] private PlayTime _playTime;


        public abstract void Execute();

        private void Awake()
        {
            if (_playTime == PlayTime.Awake)
                Execute();

        }
        private void OnEnable()
        {
            if (_playTime == PlayTime.OnEnable)
                Execute();
        }
        private void OnDisable()
        {
            if (_playTime == PlayTime.OnDisable)
                Execute();
        }
        private void Start()
        {
            if (_playTime == PlayTime.Start)
                Execute();
        }
        private void OnDestroy()
        {
            if (_playTime == PlayTime.OnDestroy)
                Execute();
        }
    }
}