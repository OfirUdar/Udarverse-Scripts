using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.Menu.UI
{
    public class UI_LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private TextMeshProUGUI _percentageText;

        private void OnEnable()
        {
            SceneChanger.OnStartLoadingScene += Event_OnStartLoadingScene;
            SceneChanger.OnLoadingSceneProgress += Event_OnLoadingSceneProgress;
            SceneChanger.OnEndLoadingScene += Event_OnEndLoadingScene;
        }
        private void OnDisable()
        {
            SceneChanger.OnStartLoadingScene -= Event_OnStartLoadingScene;
            SceneChanger.OnLoadingSceneProgress -= Event_OnLoadingSceneProgress;
            SceneChanger.OnEndLoadingScene -= Event_OnEndLoadingScene;
        }

        private void Event_OnEndLoadingScene()
        {
            _loadingScreen.SetActive(false);
        }
        private void Event_OnStartLoadingScene()
        {
            _percentageText.text = 0 + "%";
            _progressBarImage.fillAmount = 0;
            _loadingScreen.SetActive(true);
        }

        private void Event_OnLoadingSceneProgress(float progress)
        {
            _progressBarImage.fillAmount = progress;
            _percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";
        }
    }

}
