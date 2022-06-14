
using System;
using UnityEngine;

namespace Udarverse.Character
{
    public class EnemyInput : MonoBehaviour, IInputCharacter
    {

        [SerializeField] private Vector2 _chaseDistance = new Vector2(5f, 1f);
        [SerializeField] private float _chaseHeight = 1f; //the height distance between the enemy and player that allow the enemy to chase
        [SerializeField] private LayerMask _targetLayer;

        private bool _isAttacking = false;
        private Vector3 _directionToTarget;
        private Vector3 _movementToTarget;


        private Vector3 _startPosition;

        public event Action<bool> OnItemNextPrev;
        public event Action<int> OnItemSelected;
        public event Action<bool> OnCrouchChanged;

        public Vector3 StartPosition => _startPosition;

        private void OnEnable()
        {
            Invoke(nameof(SetupStartPosition),0.5f);
        }

        private void Update()
        {
            SearchForTarget();

        }

        private void SetupStartPosition()
        {
            _startPosition = transform.position;
        }

        private void SearchForTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, _chaseDistance.x, _targetLayer);

            if (colliders != null && colliders.Length > 0)
            {
                GoForTarget(colliders);
            }
            else
            {
                _isAttacking = false;
                GoForStartPosition();
            }
        }

        private void GoForStartPosition()
        {
            if ((_startPosition - transform.position).sqrMagnitude > 1f)
            {
                _directionToTarget = _startPosition - transform.position;
                _directionToTarget.y = 0;
                _movementToTarget = _directionToTarget.normalized;
            }
            else
                _movementToTarget = Vector3.zero;
        }

        private void GoForTarget(Collider[] colliders)
        {
            //var diffY = Mathf.Abs(colliders[0].transform.position.y - transform.position.y);
            //if (diffY >= _chaseHeight)
            //{
            //    _isAttacking = false;
            //    _movementToTarget = Vector3.zero;
            //    return;
            //}

            _directionToTarget = colliders[0].transform.position - transform.position;
            _directionToTarget.y = 0;

            if (_directionToTarget.sqrMagnitude > _chaseDistance.y * _chaseDistance.y)
            {
                _isAttacking = false;
                _movementToTarget = _directionToTarget;
            }
            else
            {
                _isAttacking = true;
                _movementToTarget = Vector3.zero;
            }
        }

        public Vector3 GetMovement()
        {
            return _movementToTarget;
        }

        public Vector3 GetRotation()
        {
            return _directionToTarget;
        }

        public bool IsAttacking()
        {
            return _isAttacking;
        }

        public bool IsCrouching()
        {
            return false;

        }

        public bool IsJumping()
        {
            return false;

        }

        public bool IsRunning()
        {
            return false;

        }

        public void SetIsCrouching(bool isCrouching)
        {

        }

        public int IsSwitchingItem()
        {
            return 0;
        }
    }

}
