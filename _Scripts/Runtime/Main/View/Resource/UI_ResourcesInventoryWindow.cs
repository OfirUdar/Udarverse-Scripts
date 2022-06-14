
using System.Collections;
using System.Collections.Generic;
using Udar.DesignPatterns.UdarPool;
using Udarverse.Player.Stats;
using UnityEngine;

namespace Udarverse.UI
{
    public class UI_ResourcesInventoryWindow : MonoBehaviour
    {
        [SerializeField] private UI_ResourceInventoryCard _resourceCardPfb;
        [SerializeField] private RectTransform _container;


        private readonly List<GameObject> _resourceCardList = new List<GameObject>();

        private void OnEnable()
        {
            var inventory = PlayerStats.Instance.GetResourcesInventory();
            foreach (var resource in inventory)
            {
                var resourceCardInstance = UdarPool.Instance.Get(_resourceCardPfb, _container);
                resourceCardInstance.Setup(resource.resourceSC, resource.amount);
                _resourceCardList.Add(resourceCardInstance.gameObject);
            }
            Time.timeScale = 0;
        }
        private void OnDisable()
        {
            foreach (var resource in _resourceCardList)
            {
                UdarPool.Instance.Return(resource);
            }
            _resourceCardList.Clear();

            Time.timeScale = 1;
        }

    }

}
