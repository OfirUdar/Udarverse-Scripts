using Udarverse.Resources;
using System;
using System.Collections.Generic;
using Udar.DesignPatterns.Singleton;
using UnityEngine;
using System.Linq;
using Udarverse.Save;

namespace Udarverse.Player.Stats
{
    public class PlayerStats : Singleton<PlayerStats>
    {

        private List<ResourceInventory> _resourceInventoryList
            = new List<ResourceInventory>();

        public static Action<ResourceSC, int> OnResourceAmountChanged;

        public List<ResourceInventory> GetResourcesInventory() => _resourceInventoryList;

     
        private void OnEnable()
        {
            ResourceSpawner.OnResourceSpawned += Event_OnResourceCollected;

        }
        private void OnDisable()
        {
            ResourceSpawner.OnResourceSpawned -= Event_OnResourceCollected;
        }
        private void Start()
        {
            LoadFromSave();
        }


        private void LoadFromSave()
        {
            var resourceInventory = GameSaveManager.Instance.LoadResourceInventory();
            if (resourceInventory != null)
            {
                _resourceInventoryList = resourceInventory.resourcesInventoryList;
                foreach (var resource in _resourceInventoryList)
                {
                    OnResourceAmountChanged?.Invoke(resource.resourceSC, resource.amount);
                }
            }
        }

        public bool TryPayWithResource(ResourceSC resourceSC, int amountToPay = 1)
        {
            if (Contains(resourceSC, out ResourceInventory resourceInventory))
            {
                if (resourceInventory.amount - amountToPay < 0)
                    return false;
                resourceInventory.amount -= amountToPay;

                OnResourceAmountChanged?.Invoke(resourceSC, resourceInventory.amount);
                //GameSaveManager.Instance.SaveResourcesInventory();
                return true;
            }
            return false;
        }
        private void Pay(List<PriceResource> price)
        {
            foreach (var priceResource in price)
            {
                TryPayWithResource(priceResource.resourceSC,
                    priceResource.price);
            }
        }
        public bool TryPay(List<PriceResource> price)
        {
            if (!CanPay(price))
                return false;
            foreach (var priceResource in price)
            {
                TryPayWithResource(priceResource.resourceSC,
                    priceResource.price);
            }
            return true;
        }
        public bool CanPay(List<PriceResource> price)
        {
            foreach (var priceResource in price)
            {
                if (!CanPayForResource(priceResource))
                    return false;
            }
            return true;
        }


        public void AddResource(ResourceSC resourceSC, int amount, bool save = true)
        {
            if (!Contains(resourceSC))
                _resourceInventoryList.Add(new ResourceInventory(resourceSC));

            Contains(resourceSC, out ResourceInventory resourceInventory);
            resourceInventory.amount += amount;
            OnResourceAmountChanged?.Invoke(resourceSC, resourceInventory.amount);
            //if (save)
            //    GameSaveManager.Instance.SaveResourcesInventory();
        }


        public bool CanPayForResource(PriceResource priceResource)
        {
            if (Contains(priceResource.resourceSC, out ResourceInventory resourceInventory))
            {
                if (resourceInventory.amount - priceResource.price < 0)
                    return false;
                return true;
            }
            return false;
        }


        private bool Contains(ResourceSC resourceSC)
        {
            return _resourceInventoryList.Any((ob) => ob.resourceSC == resourceSC);
        }
        private bool Contains(ResourceSC resourceSC, out ResourceInventory resourceInventory)
        {
            var resource = _resourceInventoryList.Find((ob) => ob.resourceSC == resourceSC);
            var isContains = resource != null;
            resourceInventory = resource;
            return isContains;
        }
        private void Event_OnResourceCollected(ResourceSC resourceSC, int amount, GameObject target)
        {
            if (target != GameManager.Instance.MainPlayer)
                return;

            AddResource(resourceSC, amount);
        }

    }
}