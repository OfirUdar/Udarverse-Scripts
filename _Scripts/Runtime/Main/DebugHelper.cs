using Udarverse.Player.Stats;
using Udarverse.Resources;
using Udarverse.Save;
using UnityEditor;
using UnityEngine;

namespace Udarverse.DebugHelper
{
    public class DebugHelper : MonoBehaviour
    {
        [Range(0, 50)]
        public float TimeScale = 1f;
        public int startResources = 100;
        public ResourceListSC resources;
        private void Start()
        {
            foreach (var resource in resources.GetResources())
            {
                PlayerStats.Instance.AddResource(resource, startResources, false);
            }
        }
        private void OnValidate()
        {
            Time.timeScale = TimeScale;
        }

#if UNITY_EDITOR
        [ContextMenu("Delete Full Game Data")]
        public void DeleteFullGameData()
        {
            FileUtil.DeleteFileOrDirectory(GameSaveManager.GameDataPath);
        }
#endif
    }
}

