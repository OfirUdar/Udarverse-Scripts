using System.Collections;
using Udar.DesignPatterns.UdarPool;
using Udar.SceneField;
using UnityEngine;

namespace Udarverse
{
    [SelectionBase]
    public class TeleportPlatform : Platform
    {
        [SerializeField] private DetectorPoint _detectorPoint;

        [Editable]
        [Header("Teleport To Scene:")]
        [SerializeField] protected SceneField _teleportToScene;

        private void OnEnable()
        {
            StartCoroutine(SubscribeDelay());
        }

        private void OnDisable()
        {
            _detectorPoint.OnEnterDetect -= Event_OnEnterDetect;
        }

        private IEnumerator SubscribeDelay()
        {
            yield return UdarPool.Instance.GetWaitForSeconds(2f);
            _detectorPoint.OnEnterDetect += Event_OnEnterDetect;
        }

        private void Event_OnEnterDetect(Collider collider)
        {
            if (_teleportToScene.HasScene)
                SceneChanger.Instance.LoadScene(_teleportToScene.SceneName, transform);

        }
    }
}
