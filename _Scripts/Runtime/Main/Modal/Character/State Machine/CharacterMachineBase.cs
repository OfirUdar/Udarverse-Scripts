using System;
using Udar.DesignPatterns.StateMachine;
using UnityEngine;

namespace Udarverse.Character
{
    public abstract class CharacterMachineBase : UdarStateMachine
    {
        public CharacterController CharacterController;
        public CharacterAnimation CharacterAnimation;
        public IInputCharacter InputCharacter;

        protected override void Awake()
        {
            InputCharacter = GetComponent<IInputCharacter>();
            base.Awake();
        }
        public override void ChangeState(IState nextState, IState nextSubState = null)
        {
            if (nextSubState != null)
            {
                if (IsCanTransitionTo(nextSubState, nextState))
                {
                    base.ChangeState(nextState, nextSubState);
                    return;
                }
            }

            base.ChangeState(nextState, nextState.SubStateDefualt);
        }

        public bool IsCanTransitionTo(IState nextState, IChangeableState parentState)
        {
            return nextState.RequirementParentList.Contains(parentState.GetType());
        }

    }
}

