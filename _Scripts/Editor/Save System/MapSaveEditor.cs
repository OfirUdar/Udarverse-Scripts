using System;
using System.IO;
using Udarverse.Resources;
using UnityEditor;
using UnityEditor.Events;

namespace Udarverse.Editor
{
    using Udarverse.Save;
    using UnityEngine;
    using UnityEngine.Events;

    public class MapSaveEditor : BaseSaveUtils
    {
        private static MapSaveEditor _instance;
        public static MapSaveEditor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapSaveEditor();
                return _instance;
            }
        }


        private static readonly string _savePath = Application.persistentDataPath + "/Editor/Maps";

        private static PlatformEditorData _editorData;

        public MapSaveEditor()
        {
            _editorData = Resources.Load<PlatformEditorData>("_Data/Platforms/Platform_Editor_Data");
        }

        public void Save(Transform mapContainer, string fileName)
        {

            var map = SerializeMap(mapContainer);
            if (!IsFileExists(_savePath))
                Directory.CreateDirectory(_savePath);
            var path = _savePath + "/" + fileName;
            if (IsFileExists(path))
            {
                if (!EditorUtility.DisplayDialog("File already exists", "Are you want to override it?", "Ok", "Cancel"))
                    return;
            }
            base.Save(map, path);


        }
        public void Load(string fileName)
        {
            try
            {
                var path = _savePath + "/" + fileName;
                if (!IsFileExists(path))
                    throw new Exception("File doesnt exists!");
                var map = Load<MapData>(path);
                base.LoadMapToScene(map);
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "Ok");
                Debug.Log("Errors:\n" + e.Message);
            }

        }

        protected override void SetOnAcomplishedEvent(UnityEvent eve, UnityAction<bool> action, bool value)
        {
            UnityEventTools.AddBoolPersistentListener(eve, action, value);
        }

        protected override Platform CreateNewPlatform(PlatformSC platformSC, GameObjectData platformData)
        {
            return PlatformCreation.CreateNewPlatform(platformSC, platformData);
        }

        protected override PlatformEntryPoint CreateNewEntryPoint(Platform platform, GameObjectData entryData)
        {
            return PlatformCreation.CreateNewEntryPoint
                (platform, entryData);
        }

        protected override PlatformSC GetPlatformSC(string guid)
        {
            return _editorData.platformsList.GetPlatform(guid);
        }

        protected override ResourceSC GetResourceSC(string guid)
        {
            return _editorData.resourcesList.GetResource(guid);
        }
    }
}