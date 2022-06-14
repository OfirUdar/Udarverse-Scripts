using Udarverse.Inventory;

namespace Udarverse.Character
{
    public class CharacterInteractiveMachine : CharacterMachineBase
    {
        public CharacterMovementMachine MovementMachine;
        public StatesInteractiveContainer States;
        public CharacterItemsInventory InventoryManager;

        public bool IsAttacking { get; set; }

        protected override void Init()
        {
            States.Init(this);
            ChangeState(States.IdleState);
        }
    }
}
