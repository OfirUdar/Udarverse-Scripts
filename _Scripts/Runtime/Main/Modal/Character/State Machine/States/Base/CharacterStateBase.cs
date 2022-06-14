using System;
using System.Collections.Generic;
using Udar.DesignPatterns.StateMachine;

namespace Udarverse.Character
{
    [Serializable]
    public abstract class CharacterStateBase<T> : StateBase
    {
        protected T _ctx;
        protected CharacterStateBase<T> _prevState;
        protected abstract void Setup();
        public virtual void Init(T context)
        {
            _ctx = context;
            Setup();
        }

        public override void ChangeState(IState nextState, IState nextSubState = null)
        {
            if (nextSubState != null)
            {
                if (IsCanTransitionTo(nextSubState, nextState))
                {
            //        UnityEngine.Debug.Log($"next state= {nextState.GetType().Name}\n next sub state= {nextSubState.GetType().Name}");
                    base.ChangeState(nextState, nextSubState);
                    return;
                }
            }
          //  UnityEngine.Debug.Log($"next state= {nextState.GetType().Name}\n next sub state= { nextState.SubStateDefualt?.GetType().Name}");

            base.ChangeState(nextState, nextState.SubStateDefualt);
        }

        public bool IsCanTransitionTo(IState nextState, IChangeableState parentState)
        {
            return nextState.RequirementParentList.Contains(parentState.GetType());
        }

        public bool TryTransit(CharacterStateBase<T> nextState, IChangeableState parentState = null)
        {
            if (parentState == null)
                parentState = _parentState;
            bool canTranstit = IsCanTransitionTo(nextState, parentState);
            if (canTranstit)
            {
                nextState._prevState = this;
                _parentState.ChangeState(nextState, _subState);
            }


            return canTranstit;
        }

    }
}

