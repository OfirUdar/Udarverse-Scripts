
using Udarverse.Save;
using UnityEngine;

namespace Udarverse.Resources
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resources/Resource", order = 0)]
    public class ResourceSC : SaveableScriptableObject
    {
        public Sprite sprite;
        public ResourceCollector resourcePfb;
    }

}
