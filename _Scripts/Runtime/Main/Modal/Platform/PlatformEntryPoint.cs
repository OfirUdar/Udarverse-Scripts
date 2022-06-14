using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Udarverse.Player.Stats;
using Udar.DesignPatterns.UdarPool;
using Udarverse.Resources;
using Udarverse.Save;

namespace Udarverse
{
    [Serializable]
    [SelectionBase]
    public class PlatformEntryPoint : SaveableGameObject
    {
        [SerializeField] private ResourceListSC _resources;
        [SerializeField] private DetectorPoint _entryPointDetector;
        [SerializeField] private Transform _pointToTransist;
        [SerializeField] protected ResourceSpawnData _resourceSpawnData;

        [Editable]
        public List<PriceResource> Price;

        [Editable]
        public UnityEvent OnAcomplished;


        private Coroutine _coroutineTransist;

        public event Action<List<PriceResource>> OnGetPrices;
        public event Action<PriceResource> OnPriceChanged;

        private void Awake()
        {
            _resourceSpawnData.startTransform = GameManager.Instance.MainPlayer.transform;
            _resourceSpawnData.target = _pointToTransist.gameObject;

            if (Price.Count == 0)
            {
                Acomplish(false);
            }
        }



        private void OnEnable()
        {
            _entryPointDetector.OnEnterDetect += Event_OnEnterDetect;
            _entryPointDetector.OnExitDetect += Event_OnExitDetect;

            StartCoroutine(SetGetPriceDelay(0.2f));


        }
        private void OnDisable()
        {
            _entryPointDetector.OnEnterDetect -= Event_OnEnterDetect;
            _entryPointDetector.OnExitDetect -= Event_OnExitDetect;
        }

        public void UpdatePrice(PriceResource priceResource)
        {
            OnPriceChanged?.Invoke(priceResource);
        }
        public void SetPointTransistPosition(Vector3 position)
        {
            _pointToTransist.transform.position = position;
        }
        private IEnumerator SetGetPriceDelay(float delay)
        {
            yield return UdarPool.Instance.GetWaitForSeconds(delay);
            OnGetPrices?.Invoke(Price);
        }
        private void StartTransist()
        {
            transform.localScale = Vector3.one * 1.1f;
            _coroutineTransist = StartCoroutine(TransistResources());
        }
        private void StopTransist()
        {
            transform.localScale = Vector3.one;
            if (_coroutineTransist != null)
                StopCoroutine(_coroutineTransist);
        }

        private IEnumerator TransistResources()
        {
            float timeBtwCreation = .05f;
            var unpaidPriceList = new List<PriceResource>();
            foreach (var price in Price)
            {
                if (!price.IsPaid)
                    unpaidPriceList.Add(price);
            }
            while (unpaidPriceList.Count > 0)
            {
                for (int i = 0; i < unpaidPriceList.Count; i++)
                {
                    var resourceSC = unpaidPriceList[i].resourceSC;
                    bool playerHasPaied = PlayerStats.Instance.TryPayWithResource(resourceSC);
                    if (!playerHasPaied)
                        continue;

                    ResourceSpawner.Instance.Spawn(_resourceSpawnData, resourceSC, 1);

                    unpaidPriceList[i].CurrentPaid++;
                    OnPriceChanged?.Invoke(unpaidPriceList[i]);
                    //GameSaveManager.Instance.SaveMap();

                    if (unpaidPriceList[i].IsPaid)
                    {
                        unpaidPriceList.RemoveAt(i);
                        continue;
                    }
                }
                yield return UdarPool.Instance.GetWaitForSeconds(timeBtwCreation);
            }
            Acomplish();
        }
        private void Acomplish(bool isSaveMap = true)
        {
            OnAcomplished?.Invoke();
            this.gameObject.SetActive(false);

            //if (isSaveMap)
            //    GameSaveManager.Instance.SaveMap();
        }

        #region Events

        private void Event_OnEnterDetect(Collider collider)
        {
            StartTransist();
        }
        private void Event_OnExitDetect(Collider collider)
        {
            StopTransist();
        }

        #endregion
    }

    [Serializable]
    public class PriceResource
    {
        public ResourceSC resourceSC;
        public int price = 500;
        [SerializeField, HideInInspector] private int _currentPaid = 0;
        public int CurrentPaid
        {
            get
            {
                return _currentPaid;
            }
            set
            {
                _currentPaid = value;
                _isPaid = _currentPaid >= price;
            }
        }

        [SerializeField, HideInInspector] private bool _isPaid = false;
        public bool IsPaid => _isPaid;
    }
}

