using System;
using System.Collections;
using Udarverse.Resources;
using UnityEngine;

namespace Udarverse
{
    public abstract class UpgradeMachineBase : MonoBehaviour
    {
        [SerializeField] private DetectorPoint _detectorArea;
        [SerializeField] private float _timeProducer = 3f;

        public Action<float, int> OnProgressingProduce;
        public Action<bool, Sprite> OnProducingStateChanged;

        private Coroutine _coroutineProducing;
        private int _amountToProduce;
        private ResourceSpawnData _resourceSpawnData;


        protected bool IsProducing => _coroutineProducing != null;

        protected abstract void OnOneProduceFinish();
        protected abstract PayAction GetPayAction();
        protected abstract void OnPlayerInPayRegion(bool isDetected);


        protected virtual void Awake()
        {
            _resourceSpawnData.startTransform = GameManager.Instance.MainPlayer.transform;
            _resourceSpawnData.target = gameObject;
        }
        private void OnEnable()
        {
            _detectorArea.OnEnterDetect += Event_OnEnterDetect;
            _detectorArea.OnExitDetect += Event_OnExitDetect;
        }
        private void OnDisable()
        {
            _detectorArea.OnEnterDetect -= Event_OnEnterDetect;
            _detectorArea.OnExitDetect -= Event_OnExitDetect;
        }

        public Sprite GetRewardSprite() => GetPayAction().GetRewardSprite();

        protected void OnPaid()
        {
            var payAction = GetPayAction();

            foreach (var price in payAction.priceList)
            {
                ResourceSpawner.Instance.Spawn(_resourceSpawnData, price.resourceSC, price.price);
            }
            if (_coroutineProducing == null)
            {
                _amountToProduce = GetPayAction().amountToProduce;
                _coroutineProducing = StartCoroutine(Produce());
            }
            else
            {
                _amountToProduce += GetPayAction().amountToProduce;
            }

        }

        private IEnumerator Produce()
        {
            OnProducingStateChanged?.Invoke(true, GetPayAction().GetRewardSprite());
            while (_amountToProduce > 0)
            {
                float _timer = 0;
                while (_timer < _timeProducer)
                {
                    _timer += Time.deltaTime;
                    OnProgressingProduce?.Invoke((_timer / _timeProducer), _amountToProduce);
                    yield return null;
                }
                OnOneProduceFinish();
                _amountToProduce--;
            }
            OnProducingStateChanged?.Invoke(false, null);
            _coroutineProducing = null;
        }

        private void Event_OnEnterDetect(Collider collider)
        {
            var payAction = GetPayAction();
            payAction.OnPaid = OnPaid;
            OnPlayerInPayRegion(true);
        }
        private void Event_OnExitDetect(Collider collider)
        {
            OnPlayerInPayRegion(false);
        }

    }

}
