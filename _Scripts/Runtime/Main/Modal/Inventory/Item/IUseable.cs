using System;

namespace Udarverse.Inventory
{
    public interface IUseable
    {
        public void Use(Action onUsed);
    }

}
