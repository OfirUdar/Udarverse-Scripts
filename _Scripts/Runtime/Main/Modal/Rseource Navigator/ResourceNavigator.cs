using System;
using Udar.DesignPatterns.Singleton;
using Udar.Utils.Timer;
using UnityEngine;

namespace Udarverse.Resources.Navigator
{
    public class ResourceNavigator : Singleton<ResourceNavigator>
    {
        [SerializeField] private Transform _navigatorView;
        [SerializeField] private Transform _resourceContainer;

        private Vector3 _targetPosition;
        private ResourceSC _currentResourceTarget;

        public event Action<bool> OnNavigatorStatusChanged;

        private UdarTimer _timer;

        protected override void Awake()
        {
            base.Awake();
            _timer = new UdarTimer(this, 7f, () => SearchForTarget(_currentResourceTarget));
        }
        private void OnEnable()
        {
            _navigatorView.gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            _navigatorView.gameObject.SetActive(false);
        }

        private void Update()
        {
            var direction = _targetPosition - transform.position;
            direction.y = 0;
            var sqrDistance = direction.sqrMagnitude;

            if (sqrDistance < 15f)
                FinishNavigator();

            _navigatorView.forward = Vector3.Lerp(_navigatorView.forward, direction, Time.deltaTime);
        }

        private void StartNavigator()
        {            
            _timer.Start();
            this.enabled = true;
            OnNavigatorStatusChanged?.Invoke(true);
        }
        public void FinishNavigator()
        {
            _timer.Stop();
            OnNavigatorStatusChanged?.Invoke(false);
            this.enabled = false;
        }

        public bool SearchForTarget(ResourceSC resource)
        {
            var mapContainer = GameObject.Find("Map Container");

            var units = mapContainer.GetComponentsInChildren<Unit>();
            var minDistance = Mathf.Infinity;
            Unit closetUnit = null;
            foreach (var unit in units)
            {
                if (unit.GetResourceSpawner() != resource)
                    continue;

                var sqrDistance = (transform.position - unit.transform.position).sqrMagnitude;

                if (sqrDistance < minDistance)
                {
                    minDistance = sqrDistance;
                    closetUnit = unit;
                }

            }
            if (closetUnit != null)
            {
                if(enabled&&_currentResourceTarget!=resource)
                    OnNavigatorStatusChanged?.Invoke(false);

                _targetPosition = closetUnit.transform.position;
                _currentResourceTarget = resource;
                if (_resourceContainer.childCount > 0)
                    Destroy(_resourceContainer.GetChild(0).gameObject);
                Instantiate(resource.resourcePfb.transform.GetChild(0), _resourceContainer);
                StartNavigator();
                return true;
            }
            return false;

        }
    }
}
