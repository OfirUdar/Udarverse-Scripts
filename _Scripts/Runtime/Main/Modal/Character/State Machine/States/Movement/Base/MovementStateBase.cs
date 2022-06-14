
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Character
{
    public abstract class MovementStateBase : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedRotate;
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        public override void OnStateEnter()
        {
            _ctx.SpeedMove = _speedMove;
            _ctx.SpeedRotate = _speedRotate;
            if (_ctx.CharacterController.isGrounded)
                OnEnter?.Invoke();
        }
        public override void OnStateExit()
        {
            OnExit?.Invoke();
        }

        protected void SetSpeedMove(float speed)
        {
            _speedMove = speed;
        }
        protected void SetSpeedRotate(float speed)
        {
            _speedRotate = speed;
        }

        public override void OnStateUpdate()
        {
            //var movement = _ctx.InputCharacter.GetMovement();
            var rotation = _ctx.InputCharacter.GetRotation();
            _ctx.Movement = _ctx.transform.forward;//movement;
            //_ctx.Movement = movement;//movement;
            _ctx.Rotation = rotation;
           // _ctx.CharacterAnimation.SetVelocities(movement.x, movement.z);
            _ctx.CharacterAnimation.SetVelocities(_ctx.Movement.x, _ctx.Movement.z);
        }
    }
}

