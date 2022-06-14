
using System.IO;
using Udarverse.Save;

namespace Udarverse.Menu
{
    using System.Collections.Generic;
    using Udar.SceneField;
    using Udarverse.Resources;
    using UnityEngine;
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]private ResourceListSC _resources;
        
        private void Awake()
        {
            if (Application.isMobilePlatform)
                Application.targetFrameRate = 30;
        }
        public void SetResourcesHack(int startResources=100)
        {
            List<ResourceInventory> resourceInventoryList= new List<ResourceInventory>();
            foreach (var resource in _resources.GetResources())
            {
                var resourceInventory = new ResourceInventory(resource);
                resourceInventory.amount = startResources;
                resourceInventoryList.Add(resourceInventory);
            }
            var resourceInventoryData = new ResourceInventoryData()
            {
                resourcesInventoryList = resourceInventoryList
            };
            Save(resourceInventoryData, Application.persistentDataPath + "/Game/Game Data/", "ResourcesInventory");
            
        }

        private void Save(object ob, string path, string fileName)
        {
            var jsonData = JsonUtility.ToJson(ob);
            var fullFilePath = path + fileName + ".dat";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(fullFilePath, jsonData);
        }

        #region UI
        public void LoadScene(SceneFieldRef sceneFieldRef)
        {
            SceneChanger.Instance.LoadScene(sceneFieldRef.SceneField.SceneName);
        }
        public void ResetGameData()
        {
            Directory.Delete(GameSaveManager.GameDataPath, true);
        }
        #endregion
    }

}
