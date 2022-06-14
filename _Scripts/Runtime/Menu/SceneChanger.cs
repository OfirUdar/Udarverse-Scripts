
using System;
using System.Collections;
using Udar.DesignPatterns.Singleton;
using Udar.DesignPatterns.UdarPool;
using UnityEngine.SceneManagement;

namespace Udarverse
{
    public class SceneChanger : SingletonPersistent<SceneChanger>
    {
        public static Action OnStartLoadingScene;
        public static Action<float> OnLoadingSceneProgress;
        public static Action<bool> OnBeforeEndLoadingScene;
        public static Action OnEndLoadingScene;

        public void LoadScene(string sceneName,bool isTeleport=false)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName,isTeleport));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, bool isTeleport)
        {
            OnStartLoadingScene?.Invoke();

            var async = SceneManager.LoadSceneAsync(sceneName);

            while (!async.isDone)
            {
                OnLoadingSceneProgress?.Invoke(async.progress + 0.1f);
                yield return null;
            }

            OnBeforeEndLoadingScene?.Invoke(isTeleport);

            yield return UdarPool.Instance.GetWaitForSeconds(0.5f);

            OnEndLoadingScene?.Invoke();
        }

    }
}

