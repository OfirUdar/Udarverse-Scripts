using Udarverse.Save;
using UnityEngine;

namespace Udarverse
{
    public class CheckpointPlatform : Platform
    {
        [SerializeField] private DetectorPoint _detectorPoint;

        private void OnEnable()
        {
            _detectorPoint.OnEnterDetect += Event_OnEnterDetect;
        }
        private void OnDisable()
        {
            _detectorPoint.OnEnterDetect -= Event_OnEnterDetect;
        }


        private void Event_OnEnterDetect(Collider collider)
        {
            GameSaveManager.Instance.SavePlayerPosition();
        }
    }
}

