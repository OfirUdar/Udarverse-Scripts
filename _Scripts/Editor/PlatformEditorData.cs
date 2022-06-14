using Udarverse.Resources;
using UnityEngine;

namespace Udarverse.Editor
{
    [CreateAssetMenu(fileName = "Udarverse Editor Data", menuName = "ScriptableObjects/Editor/Udarverse Editor Data", order = 0)]

    public class PlatformEditorData : ScriptableObject
    {
        public PlatformEntryPoint entryPointPfb;
        public PlatformCategorySC platformsList;
        public ResourceListSC resourcesList;


    }
}

