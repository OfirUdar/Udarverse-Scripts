using System.Collections.Generic;
using System.IO;
using Udarverse.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Save
{
    public abstract class BaseSaveUtils
    {
        private const string EXTENION = ".dat";




        #region Abstarct Methods
        protected abstract void SetOnAcomplishedEvent(UnityEvent eve, UnityAction<bool> action, bool value);
        protected abstract Platform CreateNewPlatform(PlatformSC platformSC, GameObjectData entryData);
        protected abstract PlatformEntryPoint CreateNewEntryPoint(Platform platform, GameObjectData platformData);
        protected abstract PlatformSC GetPlatformSC(string guid);
        protected abstract ResourceSC GetResourceSC(string guid);
        #endregion

        #region Base Save & Load

        protected bool IsFileExists(string path, string fileName = "")
        {
            return File.Exists(path + fileName + EXTENION);
        }
        protected void Save(object ob, string path, string fileName = "")
        {
            var jsonData = JsonUtility.ToJson(ob);
            var fullFilePath = path + fileName + EXTENION;

            File.WriteAllText(fullFilePath, jsonData);
        }
        public T Load<T>(string path, string fileName = "")
        {
            var fullFilePath = path + fileName + EXTENION;

            if (!File.Exists(fullFilePath))
                return default;

            var json = File.ReadAllText(fullFilePath);
            return JsonUtility.FromJson<T>(json);
        }

        #endregion


        protected void LoadMapToScene(MapData map)
        {
            Dictionary<PlatformData, Platform> platfromDicionary = CreatePlatforms(map);
            Dictionary<EntryPointData, PlatformEntryPoint> entryPointDicionary = new Dictionary<EntryPointData, PlatformEntryPoint>();

            foreach (var platformData in platfromDicionary.Keys)
            {
                foreach (var entryPointData in platformData.entryPointList)
                {
                    entryPointDicionary.Add(entryPointData, CreateEntryPoint(entryPointData, platfromDicionary[platformData]));
                }
            }

            SetOnAccomplishedEvents(platfromDicionary, entryPointDicionary);
        }

        protected MapData SerializeMap(Transform mapContainer)
        {
            MapData map = new MapData();

            var platformList = mapContainer.GetComponentsInChildren<Platform>(true);

            foreach (var platform in platformList)
            {
                map.platformList.Add(GetPlatformData(platform));
            }
            return map;
        }

        private PlatformData GetPlatformData(Platform platform)
        {
            var platformData = new PlatformData()
            {
                platformGUID = platform.platformSC.GUID,
                saveObjectGUID = platform.GUID,
                name = platform.name,
                position = platform.transform.position,
                rotation = platform.transform.rotation,
                localScale = platform.transform.localScale,
                isActive = platform.gameObject.activeSelf
            };
            var entryPointList = platform.GetComponentsInChildren<PlatformEntryPoint>(true);
            foreach (var entryPoint in entryPointList)
            {
                EntryPointData entryPointData = GetEntryPointData(entryPoint);

                platformData.entryPointList.Add(entryPointData);
            }

            return platformData;
        }
        private EntryPointData GetEntryPointData(PlatformEntryPoint entryPoint)
        {
            var entryPointData = new EntryPointData()
            {
                saveObjectGUID = entryPoint.GUID,
                name = entryPoint.name,
                position = entryPoint.transform.position,
                rotation = entryPoint.transform.rotation,
                localScale = entryPoint.transform.localScale,
                isActive = entryPoint.gameObject.activeSelf
            };
            foreach (var price in entryPoint.Price)
            {
                var priceData = new PriceNextPlatformData()
                {
                    resourceGUID = price.resourceSC.GUID,
                    price = price.price,
                    currentPaid = price.CurrentPaid,
                };
                entryPointData.priceList.Add(priceData);
            }

            for (int i = 0; i < entryPoint.OnAcomplished.GetPersistentEventCount(); i++)
            {
                var saveableOB = (entryPoint.OnAcomplished.GetPersistentTarget(i) as GameObject).GetComponent<SaveableGameObject>();
                entryPointData.eventsIDList.Add(saveableOB.GUID);
            }

            return entryPointData;
        }


        private void SetOnAccomplishedEvents(Dictionary<PlatformData, Platform> platfromDicionary, Dictionary<EntryPointData, PlatformEntryPoint> entryPointDicionary)
        {
            foreach (var cell in entryPointDicionary)
            {
                foreach (var eventID in cell.Key.eventsIDList)
                {
                    var gameObject = GetGameObjectByGUID(eventID, platfromDicionary, entryPointDicionary);
                    SetOnAcomplishedEvent(cell.Value.OnAcomplished, gameObject.SetActive, true);
                    if (gameObject.TryGetComponent(out Platform platform))
                    {
                        cell.Value.SetPointTransistPosition(platform.transform.position);
                    }
                }
            }
        }
        private GameObject GetGameObjectByGUID(string guid, Dictionary<PlatformData, Platform> platfromDicionary, Dictionary<EntryPointData, PlatformEntryPoint> entryPointDicionary)
        {
            foreach (var cell in platfromDicionary)
            {
                if (cell.Key.saveObjectGUID == guid)
                    return cell.Value.gameObject;
            }
            foreach (var cell in entryPointDicionary)
            {
                if (cell.Key.saveObjectGUID == guid)
                    return cell.Value.gameObject;
            }
            return null;
        }

        private PlatformEntryPoint CreateEntryPoint(EntryPointData entryPointData, Platform platform)
        {
            var newEntryPoint = CreateNewEntryPoint
                (platform, entryPointData);

            foreach (var price in entryPointData.priceList)
            {
                PriceResource newPrice = new PriceResource()
                {
                    resourceSC = GetResourceSC(price.resourceGUID),
                    price = price.price,
                    CurrentPaid = price.currentPaid,
                };

                newEntryPoint.Price.Add(newPrice);
            }

            return newEntryPoint;
        }
        private Dictionary<PlatformData, Platform> CreatePlatforms(MapData map)
        {
            Dictionary<PlatformData, Platform> platfromDicionary = new Dictionary<PlatformData, Platform>();
            foreach (var platformData in map.platformList)
            {
                var platformSC = GetPlatformSC(platformData.platformGUID);
                var platform = CreateNewPlatform(platformSC, platformData);
                platfromDicionary.Add(platformData, platform);
            }

            return platfromDicionary;
        }


    }
}