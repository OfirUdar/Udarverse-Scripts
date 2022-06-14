using System;
using UnityEngine;

namespace Udarverse.Resources
{
    [Serializable]
    public class ResourcePayAction : PayAction
    {
        public ResourceSC resourceSC;

        public override Sprite GetRewardSprite()
        {
            return resourceSC.sprite;
        }
    }
}
