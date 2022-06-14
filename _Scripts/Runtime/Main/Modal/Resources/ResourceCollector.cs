using System;
using System.Collections;
using UnityEngine;
using Udar.DesignPatterns.UdarPool;

namespace Udarverse.Resources
{
    public class ResourceCollector : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ResourceSC _resourceSC;
        [SerializeField] private AnimationCurve _collectMoveAnimCurve;
        [SerializeField] private float _speed = 1.5f;

        [Header("Spawn Datas:")]
        [SerializeField] private Vector3 _minPositionRadius = new Vector3(-0.3f, .95f, -0.3f);
        [SerializeField] private Vector3 _maxPositionRadius = new Vector3(0.3f, 1.1f, 0.3f);
        [SerializeField] private Vector3 _minRotationRadius = new Vector3(-90, -90, -90);
        [SerializeField] private Vector3 _maxRotationRadius = new Vector3(90, 90, 90);
        [SerializeField] private float _force = 400f;
        [SerializeField] private float _forceTorque = 2500f;


        private Transform _target;

        private void OnEnable()
        {
            Impulse();
        }
        public void Setup(Transform target)
        {
            _target = target;
        }

        private void Impulse()
        {
            Vector3 position;
            position.x = UnityEngine.Random.Range(_minPositionRadius.x, _maxPositionRadius.x);
            position.y = UnityEngine.Random.Range(_minPositionRadius.y, _maxPositionRadius.y);
            position.z = UnityEngine.Random.Range(_minPositionRadius.z, _maxPositionRadius.z);

            Vector3 eularAngles;
            eularAngles.x = UnityEngine.Random.Range(_minRotationRadius.x, _maxRotationRadius.x);
            eularAngles.y = UnityEngine.Random.Range(_minRotationRadius.y, _maxRotationRadius.y);
            eularAngles.z = UnityEngine.Random.Range(_minRotationRadius.z, _maxRotationRadius.z);

            _rigidbody.velocity = position * _force;
            _rigidbody.AddTorque(eularAngles * _forceTorque, ForceMode.Acceleration);

            StartCoroutine(MoveToTarget());
        }


        private IEnumerator MoveToTarget()
        {
            while (_rigidbody.velocity.y > 7f || _target == null)
                yield return null;

            var startPos = transform.position;
            var minDistance = 0.05f;
            var timer = 0f;

            while ((transform.position - _target.position).sqrMagnitude > minDistance)
            {
                timer += Time.deltaTime;
                var lerpAmount = _collectMoveAnimCurve.Evaluate(timer) * _speed;
                transform.position = Vector3.Lerp(startPos, _target.position, lerpAmount);
                yield return null;
            }

            UdarPool.Instance.Return(this);
        }
    }
}

