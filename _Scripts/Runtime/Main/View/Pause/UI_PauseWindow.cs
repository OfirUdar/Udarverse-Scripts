using UnityEngine;

namespace Udarverse.UI
{
    public class UI_PauseWindow : MonoBehaviour
    {
        [SerializeField] private string _menuSceneName = "Menu_Scene";
        #region UI

        private void OnEnable()
        {
            Time.timeScale = 0f; 
        }
        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
        public void ReturnToMainMenu()
        {
            SceneChanger.Instance.LoadScene(_menuSceneName);
        }

        #endregion
    }
}