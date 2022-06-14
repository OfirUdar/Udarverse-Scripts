using System;
using UnityEngine;

namespace Udarverse.Editor
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    public class PlatformEditorGUI
    {
        private static PlatformEditorGUI _instance;
        public static PlatformEditorGUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlatformEditorGUI();
                }
                return _instance;
            }
        }

        private readonly Color _speratorColor = new Color(1, 1, 1, 0.3f);
        private readonly GUIStyle _centerBoldLabel;

        private Platform _currentPlatform;
        private Dictionary<UnityEngine.Object, List<FieldInfo>>
           _editPropetyDicionary = new Dictionary<UnityEngine.Object, List<FieldInfo>>();

        private Editor _editor;


        public PlatformEditorGUI()
        {
            Selection.selectionChanged += Event_OnSelectionChanged;
            _centerBoldLabel = new GUIStyle(EditorStyles.boldLabel)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter
            };
        }


        public void OnGUI()
        {
            if (_currentPlatform == null)
                return;

            EditObjectsDetails();
        }

        private void EditObjectsDetails()
        {
            foreach (var editObject in _editPropetyDicionary.Keys)
            {
                if (editObject == null)
                    continue;
                Editor.CreateCachedEditor(editObject, null, ref _editor);

                DrawGUILine(_speratorColor, 0.5f);
                GUILayout.Label(editObject.name, _centerBoldLabel);

                var fieldsInfoList = _editPropetyDicionary[editObject];
                if (fieldsInfoList == null || fieldsInfoList.Count == 0)
                    continue;

                GUILayout.BeginVertical("box");
                var isChanged = false;
                foreach (var field in fieldsInfoList)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(_editor.serializedObject.FindProperty(field.Name));
                    isChanged = isChanged || EditorGUI.EndChangeCheck();
                    GUILayout.Space(5);
                }
                if (isChanged)
                    _editor.serializedObject.ApplyModifiedProperties();

                GUILayout.Space(5);
                GUILayout.EndVertical();
            }

            GUILayout.Space(5f);

            EditorGUI.BeginChangeCheck();
            _currentPlatform.transform.GetChild(0).localScale = EditorGUILayout.Vector3Field("GFX Scale", _currentPlatform.transform.GetChild(0).localScale);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(_currentPlatform.GetComponentsInChildren<Transform>(), $" {_currentPlatform.name}");

                PlatformCreation.FixObjectY(_currentPlatform);
            }
        }
        private void StartEditing(params UnityEngine.Object[] editObjects)
        {
            _editPropetyDicionary.Clear();
            foreach (var editObject in editObjects)
            {
                var fields = editObject.GetType().GetFields(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var fieldInfoList = new List<FieldInfo>();
                Editor.CreateCachedEditor(editObject, null, ref _editor);

                foreach (var field in fields)
                {
                    if (Attribute.IsDefined(field, typeof(EditableAttribute)))
                    {
                        fieldInfoList.Add(field);
                    }
                }
                if (fieldInfoList.Count == 0)
                    continue;

                _editPropetyDicionary.Add(editObject, fieldInfoList);
            }

        }

        private void DrawGUILine(Color color, float thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        private Platform GetParentPlatform(Transform child)
        {
            if (child.TryGetComponent(out Platform platform))
                return platform;
            if (child.parent == null)
                return null;
            return GetParentPlatform(child.parent);
        }

       
        private void Event_OnSelectionChanged()
        {
            _currentPlatform = GetParentPlatform(Selection.activeGameObject.transform);
            if (_currentPlatform != null)
            {
                var objects = _currentPlatform.GetComponentsInChildren<MonoBehaviour>();
                StartEditing(objects);
            }
        }
    }
}