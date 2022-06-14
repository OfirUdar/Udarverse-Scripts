using System;
using System.Collections.Generic;
using System.IO;
using Udar.DesignPatterns.Singleton;
using Udarverse.Inventory;
using Udarverse.Player.Stats;
using Udarverse.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Udarverse.Save
{
    public class GameSaveManager : Singleton<GameSaveManager>
    {
        [SerializeField] private ResourceListSC _resourceList;
        [SerializeField] private ItemListSC _itemList;
        public enum SaveFolder
        {
            PlayerPosition,
            ResourcesInventory,
            ItemsInventory,
            Map,
        }
        private static string _gameDataPath;
        public static string GameDataPath
        {
            get
            {
                _gameDataPath = Application.persistentDataPath + "/Game/Game Data/";
                return _gameDataPath;
            }
        }
        private string _gameDataPathWithScene => _gameDataPath + SceneManager.GetActiveScene().name + "/";



        protected override void Awake()
        {
            base.Awake();
            _gameDataPath = Application.persistentDataPath + "/Game/Game Data/";
        }

        private void OnEnable()
        {
            SceneChanger.OnStartLoadingScene += Event_OnStartLoadingScene;
            SceneChanger.OnBeforeEndLoadingScene += Event_OnBeforeEndLoadingScene;
        }
        private void OnDisable()
        {
            SceneChanger.OnStartLoadingScene -= Event_OnStartLoadingScene;
            SceneChanger.OnBeforeEndLoadingScene -= Event_OnBeforeEndLoadingScene;
        }

        private void Start()
        {
            try
            {
                LoadMap();
               // LoadResourceInventory();
                LoadItemsInventory();
            }
            catch
            {

            }
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveFullGameData(false);
            }
        }
        private void OnApplicationQuit()
        {
            SaveFullGameData(false);
        }
        public void Save(object ob, string path, string fileName)
        {
            var jsonData = JsonUtility.ToJson(ob);
            var fullFilePath = path + fileName + ".dat";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(fullFilePath, jsonData);
        }
        public T Load<T>(string path, string fileName)
        {
            var fullFilePath = path + fileName + ".dat";

            if (!File.Exists(fullFilePath))
                return default;

            var json = File.ReadAllText(fullFilePath);
            return JsonUtility.FromJson<T>(json);
        }


        #region Save Game Data
        public void SaveFullGameData(bool withRuntimeSave = true)
        {
            if (withRuntimeSave)
            {
                SavePlayerPosition();
                //SaveItemsInventory();
            }
            SaveResourcesInventory();
            SaveMap();
        }

        public void SavePlayerPosition()
        {
            try
            {
                var playerPosition = new PlayerPositionData()
                {
                    position = GameManager.Instance.MainPlayer.transform.position,
                };

                Save(playerPosition, _gameDataPathWithScene, SaveFolder.PlayerPosition.ToString());

            }
            catch
            {

            }
        }
        public void SaveResourcesInventory()
        {
            try
            {
                var resourceInventoryData = new ResourceInventoryData()
                {
                    resourcesInventoryList = PlayerStats.Instance.GetResourcesInventory()
                };

                Save(resourceInventoryData, GameDataPath, SaveFolder.ResourcesInventory.ToString());
            }
            catch
            {

            }
        }
        public void SaveItemsInventory(List<ItemInventory> itemInventoryList)
        {
            try
            {
                var itemInventoryData = new ItemInventoryData()
                {
                    itemsInventoryList = itemInventoryList,
                };

                Save(itemInventoryData, GameDataPath, SaveFolder.ItemsInventory.ToString());
            }
            catch
            {

            }
        }
        public void SaveMap()
        {
            try
            {
                var map = GameObject.Find("Map Container").transform;

                var mapData = SerializeMap(map);

                Save(mapData, _gameDataPathWithScene, SaveFolder.Map.ToString());
            }
            catch
            {

            }

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

            return entryPointData;
        }
        #endregion


        #region Load Game Data

        public void LoadFullGameData()
        {
            LoadPlayerPosition();
            LoadMap();
            LoadResourceInventory();
        }

        public void LoadPlayerPosition()
        {
            var playerPosition = Load<PlayerPositionData>(_gameDataPathWithScene, SaveFolder.PlayerPosition.ToString());
            if (playerPosition == null)
                return;

            var pos = playerPosition.position + Vector3.up * 1.2f;
            GameManager.Instance.SetPlayerPosition(pos);

        }
        public ResourceInventoryData LoadResourceInventory()
        {
            var resourceInventory = Load<ResourceInventoryData>(GameDataPath, SaveFolder.ResourcesInventory.ToString());
            if (resourceInventory == null)
                return null;

            foreach (var resource in resourceInventory.resourcesInventoryList)
            {
                resource.resourceSC = _resourceList.GetResources().Find(r => r.GUID.Equals(resource.GUID));
            }

            return resourceInventory;
        }
        public ItemInventoryData LoadItemsInventory()
        {
            var itemsInventory = Load<ItemInventoryData>(GameDataPath, SaveFolder.ItemsInventory.ToString());
            if (itemsInventory == null)
                return null;

            foreach (var item in itemsInventory.itemsInventoryList)
            {
                var itemSC = _itemList.itemList.Find((itemSC) => itemSC.GUID.Equals(item.GUID));
                item.itemSC = itemSC;
            }
            return itemsInventory;
        }
        private void LoadMap()
        {
            var mapData = Load<MapData>(_gameDataPathWithScene, SaveFolder.Map.ToString());

            if (mapData == null)
                return;
            var map = GameObject.Find("Map Container").transform;


            foreach (var platform in mapData.platformList)
            {
                var currentPlatform = FindChildByGUID(map, platform.saveObjectGUID);
                currentPlatform.gameObject.SetActive(platform.isActive);

                foreach (var entry in platform.entryPointList)
                {
                    var currentEntry = FindChildByGUID(currentPlatform, entry.saveObjectGUID).GetComponent<PlatformEntryPoint>();
                    currentEntry.gameObject.SetActive(entry.isActive);
                    foreach (var price in entry.priceList)
                    {
                        var priceFound = currentEntry.Price.Find((currentPrice) => currentPrice.resourceSC.GUID == price.resourceGUID);
                        priceFound.CurrentPaid = price.currentPaid;
                        currentEntry.UpdatePrice(priceFound);
                    }
                }
            }


        }

        private Transform FindChildByGUID(Transform parent, string guid)
        {
            var saveObList = parent.GetComponentsInChildren<SaveableGameObject>(true);
            foreach (SaveableGameObject child in saveObList)
            {
                if (child.TryGetComponent(out SaveableGameObject saveableGame))
                {
                    if (saveableGame.GUID == guid)
                        return child.transform;
                }

            }
            return null;
        }

        #endregion

        #region Events

        private void Event_OnStartLoadingScene()
        {
            SaveFullGameData(false);
        }
        private void Event_OnBeforeEndLoadingScene(bool isTeleport)
        {
            if (isTeleport)
            {
                //Searching the enter teleport and set the position
                var map = GameObject.Find("Map Container").transform;
                var enterTeleport = map.GetComponentInChildren<EnterTeleportPlatform>();
                if (enterTeleport != null)
                {

                    var pos = enterTeleport.transform.position + Vector3.up * 1.2f;
                    GameManager.Instance.SetPlayerPosition(pos);

                    return;
                }
            }
            LoadPlayerPosition();
        }

        #endregion

    }
}