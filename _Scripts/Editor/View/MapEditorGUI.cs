

namespace Udarverse.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public class MapEditorGUI
    {
        private static MapEditorGUI _instance;
        public static MapEditorGUI Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapEditorGUI();
                return _instance;
            }
        }


        private readonly PlatformCategorySC _platformCategories = Resources.Load<PlatformCategorySC>("_Data/Platforms/Platform_Category_List");
        private PlatformList _categorySelected;
        private PlatformSC _platformSelected;

        //Editing:
        private Platform _currentNewPlatform;
        private Dictionary<UnityEngine.Object, List<FieldInfo>>
            _editPropetyDicionary = new Dictionary<UnityEngine.Object, List<FieldInfo>>();
        private Editor _editor;
        private bool IsEditDetails => _editPropetyDicionary.Count > 0 ;

        //Ghosts:
        private GameObject _ghostPlatform;
        private GameObject _ghostEntryPoint;

        //Constants:
        private readonly GUIStyle _centerBoldLabel;
        private readonly GUIStyle _itemStyle;
        private readonly Material _ghostPlatformMat = Resources.Load<Material>("Art/Materials/Ghosts/Ghost_Mat");
        private readonly Material _ghostEntrypointMat = Resources.Load<Material>("Art/Materials/Ghosts/GhostSprite_Mat");
        private readonly Color _speratorColor = new Color(1, 1, 1, 0.3f);

        //GUI variables:
        private Vector2 _scrollPosition;
        private bool _isShowBranches = true;
        private bool _isBuildFlow = false;

        private MapEditorGUI()
        {
            _categorySelected = _platformCategories.GetPlatformList()[0];

            _centerBoldLabel = new GUIStyle(EditorStyles.boldLabel)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter
            };
            _itemStyle = new GUIStyle(GUI.skin.button);

            UnSelect();
            if (_isShowBranches)
                ShowHideBranches();
        }


        public void OnGUI()
        {
            if (IsEditDetails)
            {
                EditObjectsDetails();
                return;
            }
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("box", GUILayout.MaxWidth(MainWindow.Instance.position.width / 4f));
            LeftPanel();
            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");
            RightPanel();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box", GUILayout.MinHeight(200f));
            DownPanel();
            GUILayout.EndVertical();
        }

        private void LeftPanel()
        {
            GUILayout.Label("Catergories:", _centerBoldLabel);
            foreach (var category in _platformCategories.GetPlatformList())
            {
                if (_categorySelected == category)
                {
                    GUI.backgroundColor = new Color(0, .7f, 1f, 1f);
                    GUILayout.Button(category.name, GUILayout.Height(30f));
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    if (GUILayout.Button(category.name, GUILayout.Height(40f)))
                    {
                        UnSelect();
                        _categorySelected = category;
                    }
                }
                GUILayout.Space(1);
            }

        }
        private void RightPanel()
        {
            GUILayout.Label("Platforms:", _centerBoldLabel);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < _categorySelected.list.Count;)
            {
                GUILayout.BeginHorizontal();

                int limit = Mathf.Min(2, _categorySelected.list.Count - i) + i;
                for (; i < limit; i++)
                {
                    var platformSC = _categorySelected.list[i];
                    CreateItem(platformSC);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(3);
            }

            EditorGUILayout.EndScrollView();
        }
        private void DownPanel()
        {
            GUILayout.Label("Settings:", _centerBoldLabel);
            var labelWidth = EditorGUIUtility.labelWidth;
             EditorGUIUtility.labelWidth = 200f;
            EditorGUI.BeginChangeCheck();
            _isShowBranches = EditorGUILayout.Toggle("Show Branches:", _isShowBranches);
            if (EditorGUI.EndChangeCheck())
            {
                ShowHideBranches();
            }
            _isBuildFlow = EditorGUILayout.Toggle("Build Flow (Without Editing):", _isBuildFlow);

            EditorGUIUtility.labelWidth = labelWidth;
        }


        private void CreateItem(PlatformSC platformSC)
        {
            bool isButtonSelected = _platformSelected != null && _platformSelected == platformSC;

            if (isButtonSelected)
                GUI.backgroundColor = new Color(0, .7f, 1f, 1f);
            var itemSize = MainWindow.Instance.position.width / 4f;
            GUILayout.BeginVertical("Box", _itemStyle, GUILayout.Width(itemSize), GUILayout.Height(itemSize));
            GUI.backgroundColor = Color.white;
            var platformTexture = AssetPreview.GetAssetPreview(platformSC.platformPfb.gameObject);

            if (GUILayout.Button(platformTexture, GUILayout.Width(itemSize), GUILayout.Height(itemSize)))
            {
                UnSelect();
                if (!isButtonSelected)
                    Select(platformSC);
            }

            GUILayout.Label(platformSC.nameDisplay, _centerBoldLabel);
            GUILayout.EndVertical();
        }


        //Editing:
        private void EditObjectsDetails()
        {
            var isEditing = false;
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

                isEditing = true;
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

            if (!isEditing)
            {
                FinishEditing();
                return;
            }
            GUILayout.Space(5f);
            EditorGUI.BeginChangeCheck();
            _currentNewPlatform.transform.GetChild(0).localScale = EditorGUILayout.Vector3Field("GFX Scale", _currentNewPlatform.transform.GetChild(0).localScale);
            if (EditorGUI.EndChangeCheck())
                PlatformCreation.FixObjectY(_currentNewPlatform);
            GUILayout.Space(5f);
            if (GUILayout.Button("Continue", GUILayout.Height(40f)))
            {
                FinishEditing();
            }
        }
        private void StartEditing(params UnityEngine.Object[] editObjects)
        {
            if (_isBuildFlow)
                return;
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
            if (IsEditDetails)
                DestroyGhosts();

        }
        private void FinishEditing()
        {
            if (_isBuildFlow)
                return;
            InstantiateGhosts();
            _editPropetyDicionary.Clear();
        }


        //Utils:
        private void DrawGUILine(Color color, float thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
        private void ShowHideBranches()
        {
            var mapContainer = GameObject.Find("Map Container");
            var platforms = mapContainer.GetComponentsInChildren<Platform>(true);
            Undo.RecordObjects(platforms, "Activate Branches");
            foreach (var platform in platforms)
            {
                if (!platform.IsBase)
                    platform.gameObject.SetActive(_isShowBranches);
            }
        }


        private void Select(PlatformSC platformToSelect)
        {
            _platformSelected = platformToSelect;
            InstantiateGhosts();
            SceneVisibilityManager.instance.DisablePicking(_ghostPlatform, true);
            SceneView.beforeSceneGui += OnSceneGUI;
        }

        public void UnSelect()
        {
            _platformSelected = null;
            DestroyGhosts();
            SceneView.beforeSceneGui -= OnSceneGUI;
        }

        private void InstantiateGhosts()
        {
            var ghostPlatform = GameObject.Instantiate(_platformSelected.platformPfb);
            ghostPlatform.HexagonColl.gameObject.layer = 0;
            _ghostPlatform = ghostPlatform.gameObject;
            foreach (var render in _ghostPlatform.GetComponentsInChildren<MeshRenderer>())
                render.materials = new Material[1] { _ghostPlatformMat };

            _ghostEntryPoint = GameObject.Instantiate(PlatformCreation.PlatformEditorDataSC.entryPointPfb).gameObject;
            foreach (var render in _ghostEntryPoint.GetComponentsInChildren<SpriteRenderer>())
                render.material = _ghostEntrypointMat;
        }
        private void DestroyGhosts()
        {
            if (_ghostPlatform != null)
                GameObject.DestroyImmediate(_ghostPlatform);
            if (_ghostEntryPoint != null)
                GameObject.DestroyImmediate(_ghostEntryPoint);
        }


        private void OnSceneGUI(SceneView sceneView)
        {
            if (IsEditDetails)
                return;
            try
            {
                if (ShootRaycast(sceneView, out Vector3 hitPoint))
                {
                    Vector3 platformPosition = hitPoint;
                    var platform = FindClosetPlatform(hitPoint);


                    if (platform != null)
                    {
                        var closetEntryPoint = FindClosetEntryPoint(hitPoint, platform);
                        platformPosition = PlatformCreation.GetNewPlatformPosition(closetEntryPoint, platform.gameObject);
                        _ghostEntryPoint.transform.SetPositionAndRotation(PlatformCreation.GetEntryPosition(closetEntryPoint, platform.gameObject) + Vector3.up * 0.03f, PlatformCreation.GetEnrtyRotation(closetEntryPoint));

                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            Event.current.Use();
                            CreateNewPlatform(_platformSelected, closetEntryPoint, platform);
                        }
                    }
                    else
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            Event.current.Use();
                            _currentNewPlatform = PlatformCreation.CreateNewPlatform(_platformSelected, platformPosition, true);
                            var objects = _currentNewPlatform.GetComponentsInChildren<MonoBehaviour>();
                            StartEditing(objects);
                        }
                    }

                    if (Event.current.keyCode == KeyCode.R && Event.current.type == EventType.KeyDown)
                    {
                        //Rotate
                        Event.current.Use();
                        _ghostPlatform.transform.Rotate(Vector3.up, 60f);
                    }

                    _ghostPlatform.transform.position = platformPosition;
                }
            }
            catch
            {
            }

            sceneView.Repaint();
        }


        private bool ShootRaycast(SceneView sceneView, out Vector3 hitPoint)
        {
            Vector3 screenPosition = Event.current.mousePosition;
            screenPosition.y = Camera.current.pixelHeight - screenPosition.y;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            var ray = sceneView.camera.ScreenPointToRay(screenPosition);

            bool isHit = plane.Raycast(ray, out float entry);
            hitPoint = ray.GetPoint(entry);
            return isHit;
        }
        private EntryPointPosition FindClosetEntryPoint(Vector3 point, Platform platform)
        {
            PlatformCreation.SetPointsAvaliableList(platform.gameObject);
            var minDistance = Mathf.Infinity;
            EntryPointPosition closetEntryPoint = default;
            foreach (var entryPoint in PlatformCreation.PointsAvailableList)
            {
                var position = PlatformCreation.GetNewPlatformPosition(entryPoint, platform.gameObject);
                var distance = Vector3.Distance(point, position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closetEntryPoint = entryPoint;
                }
            }
            return closetEntryPoint;
        }
        private Platform FindClosetPlatform(Vector3 position)
        {
            var allPlatforms = GameObject.FindObjectsOfType<Platform>();
            var minDistance = Mathf.Infinity;
            Platform closetPlatform = null;
            foreach (var platform in allPlatforms)
            {
                if (platform.transform.parent == null)
                    continue;
                var distance = Vector3.Distance(platform.transform.position, position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closetPlatform = platform;
                }

            }
            if (minDistance > 8)
                return null;
            return closetPlatform;
        }
        private void CreateNewPlatform(PlatformSC platformSC, EntryPointPosition entryPointPosition, Platform connectedPlatform)
        {
            _currentNewPlatform = PlatformCreation.CreateNewPlatform(platformSC, entryPointPosition, connectedPlatform, _ghostPlatform.transform.rotation);
            var newEntryPoint = PlatformCreation.CreateNewEntryPoint(_currentNewPlatform, entryPointPosition, connectedPlatform);

            var objects = _currentNewPlatform.GetComponentsInChildren<MonoBehaviour>();
            var list = new List<UnityEngine.Object>(objects);
            list.Add(newEntryPoint);
            StartEditing(list.ToArray());


            MainWindow.Instance.Repaint();
        }





    }
}

