using System;


namespace Udarverse.Resources
{
    [Serializable]
    public class ResourceInventory
    {
        public ResourceSC resourceSC;
        public int amount;
        public string GUID;
        public ResourceInventory(ResourceSC resourceSC)
        {
            this.resourceSC = resourceSC;
            GUID = resourceSC.GUID;
        }

    }
}