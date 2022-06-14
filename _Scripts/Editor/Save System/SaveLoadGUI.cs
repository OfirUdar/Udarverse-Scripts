using UnityEditor;
using UnityEngine;

namespace Udarverse.Editor
{
    public class SaveLoadGUI 
    {
        private static SaveLoadGUI _instance;
        public static SaveLoadGUI Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SaveLoadGUI();
                return _instance;
            }
        }


        private string _fileNameText;


        public void OnGUI()
        {
            SaveAndLoad();
        }

        private void SaveAndLoad()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Save And Load:", EditorStyles.boldLabel);
            _fileNameText = EditorGUILayout.TextField("File Name:", _fileNameText);

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_fileNameText));
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                MapSaveEditor.Instance.Save(PlatformCreation.MapContainer, _fileNameText);
            }
            if (GUILayout.Button("Load"))
            {
                MapSaveEditor.Instance.Load(_fileNameText);
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();


            GUILayout.EndVertical();
        }
    }
}