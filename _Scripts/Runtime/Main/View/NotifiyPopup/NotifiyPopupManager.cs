using Udar.DesignPatterns.UdarPool;
using Udarverse.Resources;
using UnityEngine;

namespace Udarverse.UI
{
    public class NotifiyPopupManager : MonoBehaviour
    {
        [SerializeField] private NotifiyPopup _notifiyPopupPfb;
        private void OnEnable()
        {
            UnitHealth.OnTakingDamageFailed += Event_OnUnitTakingDamageFailed;
            Unit.OnSpawnResources += Event_OnGetHit;
        }


        private void OnDisable()
        {
            UnitHealth.OnTakingDamageFailed -= Event_OnUnitTakingDamageFailed;
            Unit.OnSpawnResources -= Event_OnGetHit;

        }

        private void Event_OnUnitTakingDamageFailed(Vector3 position, Sprite sprite)
        {
            var notifiyPopup = UdarPool.Instance.Get(_notifiyPopupPfb).SetPosition(position + Vector3.up * 0.5f);
            notifiyPopup.Display(sprite, "lvl+");
        }

        private void Event_OnGetHit(Vector3 position, Sprite sprite, int amount)
        {
            var offset = new Vector3(Random.Range(0, 1f), 1, 0) * 0.5f;
            var notifiyPopup = UdarPool.Instance.Get(_notifiyPopupPfb).SetPosition(position + offset);
            notifiyPopup.Display(sprite, "+" + amount.ToString());
        }
    }
}

