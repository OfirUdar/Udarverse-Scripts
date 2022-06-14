
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse
{
    [CreateAssetMenu(fileName = "Platform", menuName = "ScriptableObjects/Platform/Platform Category List", order = 0)]

    public class PlatformCategorySC : ScriptableObject
    {
        [SerializeField] private List<PlatformList> _platformCatgoryList;

        public List<PlatformList> GetPlatformList()
        {
            return _platformCatgoryList;
        }

        public PlatformSC GetPlatform(string guid)
        {
            foreach (var platformList in _platformCatgoryList)
            {
                var platform = platformList.list.Find((platformOb) => platformOb.GUID == guid);
                if (platform != null) return platform;
            }
            return null;
        }
    }

    [Serializable]
    public class PlatformList
    {
        public string name;
        public List<PlatformSC> list;
    }

}

