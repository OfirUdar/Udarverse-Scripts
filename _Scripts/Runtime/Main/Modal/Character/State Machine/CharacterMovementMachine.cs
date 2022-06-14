using System;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Character
{
    public class CharacterMovementMachine : CharacterMachineBase, IMoveable, IRotateable
    {
        public CharacterInteractiveMachine InteractiveMachine;
        public Health Health;
        public StatesMovementContainer States;

        public event Action<ControllerColliderHit> OnControllerColldided;
        public bool IsCurrentStateAttackable => true;//!CurrentState.ProhibitionStatesInParelarllList.Contains(typeof(AttackState));


        [HideInInspector] public float SpeedMove = 3.5f;
        [HideInInspector] public float SpeedRotate = 1000f;
        [HideInInspector] public Vector3 Movement;
        [HideInInspector] public Vector3 Rotation;
        [HideInInspector] public float VelocityY;



        protected override void Init()
        {
            Invoke(nameof(Setup), 0.3f);
        }

        private void Setup()
        {
            States.Init(this);
            ChangeState(States.GroundState, States.IdleState);
        }

        protected override void Update()
        {
            base.Update();

            if (Rotation != Vector3.zero)
                Rotate(Rotation);
            Move(Movement);
        }


        public void Move(Vector3 direction)
        {
            //direction.Normalize();
            direction.y = VelocityY;

            if (CharacterController.velocity == Vector3.zero && direction == Vector3.zero)
                return;
            CharacterController.Move(SpeedMove * Time.deltaTime * direction);
        }
        public void Rotate(Vector3 direction)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), SpeedRotate * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            OnControllerColldided?.Invoke(hit);
        }


    }
}

