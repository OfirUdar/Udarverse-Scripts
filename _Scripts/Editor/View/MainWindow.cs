namespace Udarverse.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class MainWindow : EditorWindow
    {
        public static MainWindow Instance { get; set; }


        private int _tab;
        private readonly string[] _TABS_NAME = new string[] { "Save&Load", "Map Editor", "Platform Editor" };

        
        [MenuItem("Udarverse/Udarverse Editor")]
        public static void OpenWindow()
        {
            GetWindow<MainWindow>("Udarverse Editor", true);
        }

       
        private void OnEnable()
        {
            Instance = this;

            PlatformCreation.Setup();
        }
        private void OnDisable()
        {
            MapEditorGUI.Instance.UnSelect();
        }
        private void OnGUI()
        {
            _tab = GUILayout.Toolbar(_tab, _TABS_NAME, GUILayout.Height(40f));
            switch (_tab)
            {
                case 0:
                    {
                        SaveLoadGUI.Instance.OnGUI();
                        break;
                    }
                case 1:
                    {
                        MapEditorGUI.Instance.OnGUI();
                        break;
                    }
                case 2:
                    {
                        PlatformEditorGUI.Instance.OnGUI();
                        break;
                    }
            }


        }

        
        public void SetTab(int tab)
        {
            _tab = tab;
        }

    



        //private void OpenSearchWindow(EntryPointPosition entryPointPosition)
        //{
        //    var windowPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition) - _offsetMousePosition;
        //    SearchWindow.Open(new SearchWindowContext(windowPosition),
        //        new PlatformSearchProvider(PlatformCreation.PlatformEditorDataSC.platformsList.GetPlatformList(), entryPointPosition,
        //        OnSelectNewPlatform));
        //}


    }



}
