using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Udar.DesignPatterns.UdarPool;
using Udar.Utils;
using Udarverse.Player.Stats;
using Udarverse.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Udarverse.UI
{
    public class UI_ItemProducerPrice : MonoBehaviour
    {
        [Header("Overlay")]
        [SerializeField] private GameObject _overlay;
        [Header("Price Container")]
        [SerializeField] private Transform _priceContainer;
        [Header("Price Item Prefab")]
        [SerializeField] private UI_ItemPrice _uiItemPricePfb;
        [Header("Reward")]
        [SerializeField] private Image _rewardImage;
        [Header("Buy Button")]
        [SerializeField] private Button _buyButton;


        private List<GameObject> _uiItemPriceList = new List<GameObject>();

        private void OnEnable()
        {
            ItemProducerMachine.OnPlayerDetected += Event_OnPlayerDetected;
        }
        private void OnDisable()
        {
            ItemProducerMachine.OnPlayerDetected -= Event_OnPlayerDetected;
        }

        private void Setup(PayAction payAction)
        {
            var priceList = payAction.priceList;


            foreach (var price in priceList)
            {
                var uiItem = UdarPool.Instance.Get(_uiItemPricePfb, _priceContainer);

                bool canPay = PlayerStats.Instance.CanPayForResource(price);
                uiItem.Setup(price.resourceSC.sprite, price.price, canPay ? Color.green : Color.red);
                
                _uiItemPriceList.Add(uiItem.gameObject);
            }

            _rewardImage.sprite = payAction.GetRewardSprite();


            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener
                (delegate
                {
                    if (payAction.TryPay())
                    {
                        _overlay.SetActive(false);
                        ClearPriceListInstances();
                    }
                });

        }

        private void ClearPriceListInstances()
        {
            foreach (var uiItem in _uiItemPriceList)
            {
                uiItem.SetActive(false);
            }
            _uiItemPriceList.Clear();
        }

        private void Event_OnPlayerDetected(bool isDetected, Vector3 position, PayAction payAction)
        {
            if (isDetected)
            {
                Setup(payAction);
                var newPosition = position;
                newPosition.y += 3.5f;
                _overlay.transform.position = newPosition;
                _overlay.SetActive(true);
            }
            else
            {
                _overlay.SetActive(false);
                ClearPriceListInstances();
            }
        }

      

    }
}