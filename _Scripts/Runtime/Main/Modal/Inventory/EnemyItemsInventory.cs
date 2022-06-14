namespace Udarverse.Inventory
{
    public class EnemyItemsInventory : CharacterItemsInventory
    {
        protected override void Init()
        {
            InitDefaultIventoryList();
            ChangeItem();
        }
    }
}