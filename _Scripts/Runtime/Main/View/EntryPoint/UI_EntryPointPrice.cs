
using System;
using System.Collections.Generic;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;

namespace Udarverse.UI
{
    public class UI_EntryPointPrice : MonoBehaviour
    {
        [SerializeField] private PlatformEntryPoint _platformEntryPoint;
        [SerializeField] private Transform _priceDisplayContainter;
        [SerializeField] private UI_EntryPointResourcePrice _resourcePricePfb;

        private readonly Dictionary<PriceResource, UI_EntryPointResourcePrice> _resourcesPriceDicionary = new Dictionary<PriceResource, UI_EntryPointResourcePrice>();

        private void OnEnable()
        {
            _platformEntryPoint.OnGetPrices += Event_OnGetPrices;
            _platformEntryPoint.OnPriceChanged += Event_OnPriceChanged;
        }

        private void OnDisable()
        {
            _platformEntryPoint.OnGetPrices -= Event_OnGetPrices;
            _platformEntryPoint.OnPriceChanged -= Event_OnPriceChanged;
        }
        private void CreateUIPointPirce(PriceResource price)
        {
            var resourcePriceUI = UdarPool.Instance.Get(_resourcePricePfb, _priceDisplayContainter);
            resourcePriceUI.SetPrice(price.CurrentPaid, price.price);
            resourcePriceUI.SetResourceSprite(price.resourceSC.sprite);
            _resourcesPriceDicionary.Add(price, resourcePriceUI);
        }

        private void Event_OnPriceChanged(PriceResource currentPrice)
        {
            if (!_resourcesPriceDicionary.ContainsKey(currentPrice))
                CreateUIPointPirce(currentPrice);

            _resourcesPriceDicionary[currentPrice].SetPrice
                (currentPrice.CurrentPaid, currentPrice.price);
        }

        private void Event_OnGetPrices(List<PriceResource> prices)
        {
            foreach (var price in prices)
            {
                if (!_resourcesPriceDicionary.ContainsKey(price))
                    CreateUIPointPirce(price);
            }
        }

    }
}

