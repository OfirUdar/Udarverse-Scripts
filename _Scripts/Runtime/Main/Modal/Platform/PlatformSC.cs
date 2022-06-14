
using Udarverse.Save;
using UnityEngine;

namespace Udarverse
{
    [CreateAssetMenu(fileName = "Platform", menuName = "ScriptableObjects/Platform/Platform Data", order = 1)]

    public class PlatformSC : SaveableScriptableObject
    {
        public string nameDisplay;
        public Platform platformPfb;


    }

}
