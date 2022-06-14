
namespace Udarverse.Character
{
    [System.Serializable]
    public class StatesInteractiveContainer
    {
        public InteractiveIdleState IdleState;
        public AttackState AttackState;



        public void Init(CharacterInteractiveMachine context)
        {
            IdleState.Init(context);
            AttackState.Init(context);
        }
    }

}
