using System;
using System.Collections;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;

namespace Udarverse.Character
{
    [Serializable]
    public class HitState : CharacterStateBase<CharacterMovementMachine>
    {
        [SerializeField] private float _force = 5f;
        [SerializeField] private float _duration = 1f;
        private Vector3 _direction;

        protected override void Setup()
        {
            RequirementParentList.Add(_ctx.GetType());
        }
        
        public void StartGetHit(Transform hitOb)
        {
            if (!_ctx.isActiveAndEnabled)
                return;
            _direction = _ctx.transform.position - hitOb.position;
            _ctx.SpeedMove = _force;

            _ctx.CharacterAnimation.TriggerGetHit();
            _ctx.StartCoroutine(HitEnd());
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnStateEnter()
        {
            
        }

        public override void OnStateExit()
        {
        }

        public override void OnStateUpdate()
        {
            _ctx.Movement = _direction;
        }
        public override void CheckChangeStates()
        {

        }

        private IEnumerator HitEnd()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(_duration);
            if (TryTransit(_prevState))
                yield break;
        }
    }

}
