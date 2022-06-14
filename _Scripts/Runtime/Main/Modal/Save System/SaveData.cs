using System;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Save
{
    [Serializable]
    public class MapData
    {
        public List<PlatformData> platformList = new List<PlatformData>();
    }
    [Serializable]
    public class PlatformData : GameObjectData
    {
        public string platformGUID;
        public List<EntryPointData> entryPointList = new List<EntryPointData>();
    }
    [Serializable]
    public class EntryPointData : GameObjectData
    {
        public List<PriceNextPlatformData> priceList = new List<PriceNextPlatformData>();
        public List<string> eventsIDList = new List<string>();
    }
    [Serializable]
    public class PriceNextPlatformData
    {
        public string resourceGUID;
        public int price;
        public int currentPaid;
    }
    [Serializable]
    public class GameObjectData
    {
        public string saveObjectGUID;
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
        public bool isActive;
    }

}