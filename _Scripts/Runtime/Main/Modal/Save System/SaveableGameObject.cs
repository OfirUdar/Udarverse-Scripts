using Udar.Utils;
using UnityEngine;

namespace Udarverse.Save
{
    public class SaveableGameObject : MonoBehaviour
    {
        [ReadOnly]
        public string GUID;

        public void SetGUID(string guid)
        {
            GUID = guid;
        }

#if UNITY_EDITOR
        [ContextMenu("Create GUID")]
        public void CreateGUID()
        {
            SetGUID(UnityEditor.GUID.Generate().ToString());
        }
#endif
    }
    public class SaveableScriptableObject : ScriptableObject
    {
        [ReadOnly]
        public string GUID;


        public void SetGUID(string guid)
        {
            GUID = guid;
        }

#if UNITY_EDITOR
        [ContextMenu("Create GUID")]
        public void CreateGUID()
        {
            SetGUID(UnityEditor.GUID.Generate().ToString());
        }
        //private void OnValidate()
        //{
        //    if (string.IsNullOrEmpty(GUID))
        //        CreateGUID();
        //}
#endif
    }
}

