using System.Collections.Generic;
using System.Linq;
using Udar.DesignPatterns.UdarPool;
using Udarverse.Player.Stats;
using Udarverse.Resources;
using UnityEngine;

namespace Udarverse.UI
{
    public class UI_ResourcesInventoryBar : MonoBehaviour
    {
        [SerializeField] private UI_ResourceAmount _resourceAmountPfb;

        private const int _VISIBLE_NUM_RESOURCES = 4;

        private Dictionary<ResourceSC, UI_ResourceAmount> _resourceToDisplayDicionary
            = new Dictionary<ResourceSC, UI_ResourceAmount>(); // Display the most interactive resources for moment

        private void OnEnable()
        {
            PlayerStats.OnResourceAmountChanged += Event_OnResourceAmountChanged;
        }
        private void OnDisable()
        {
            PlayerStats.OnResourceAmountChanged -= Event_OnResourceAmountChanged;
        }

        private void AddResourceToDisplay(ResourceSC resourceSC, int amount)
        {
            var resourceAmountInstance = UdarPool.Instance.Get(_resourceAmountPfb, transform);
            resourceAmountInstance.Setup(resourceSC, amount);
            _resourceToDisplayDicionary.Add(resourceSC, resourceAmountInstance);
        }
        private void Event_OnResourceAmountChanged(ResourceSC resourceSC, int amount)
        {
            //NEED TO BE DONE:

            if (_resourceToDisplayDicionary.Count < _VISIBLE_NUM_RESOURCES)
            {
                if (!_resourceToDisplayDicionary.ContainsKey(resourceSC))
                {
                    AddResourceToDisplay(resourceSC, amount);
                }
            }
            else
            {
                if (!_resourceToDisplayDicionary.ContainsKey(resourceSC))
                {
                    //Remove the first:
                    var firstKey = _resourceToDisplayDicionary.Keys.First();
                    var gameOb = _resourceToDisplayDicionary[firstKey].gameObject;
                    _resourceToDisplayDicionary.Remove(firstKey);
                    UdarPool.Instance.Return(gameOb);
                    _resourceToDisplayDicionary = 
                        new Dictionary<ResourceSC, UI_ResourceAmount>(_resourceToDisplayDicionary);
                    
                    AddResourceToDisplay(resourceSC, amount);
                }

            }

        }
    }

}
