
using System;
using System.Collections;
using Udar.DesignPatterns.Singleton;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;
using UnityEngine.Events;

namespace Udarverse.Resources
{
    public class ResourceSpawner : Singleton<ResourceSpawner>
    {
        private const int _MAX_SPAWN = 7;
        public static event Action<ResourceSC, int, GameObject> OnResourceSpawned;
        public UnityEvent OnSpawned;
        public void Spawn(ResourceSpawnData spawnData, int amount)
        {
            OnResourceSpawned?.Invoke(spawnData.resourceSC, amount, spawnData.target);

            var amountToSpawn = Mathf.Min(amount * 3, _MAX_SPAWN);
            StartCoroutine(SpawnCoroutine(spawnData, amountToSpawn));
        }
        public void Spawn(ResourceSpawnData spawnData, ResourceSC resourceSC, int amount)
        {
            OnResourceSpawned?.Invoke(resourceSC, amount, spawnData.target);

            var amountToSpawn = Mathf.Min(amount, _MAX_SPAWN);
            spawnData.resourceSC = resourceSC;
            StartCoroutine(SpawnCoroutine(spawnData, amountToSpawn));
        }
        public void Spawn(ResourceSpawnData spawnData)
        {
            int amount = Mathf.RoundToInt(UnityEngine.Random.Range(spawnData.spawnAmount.x, spawnData.spawnAmount.y));

            OnResourceSpawned?.Invoke(spawnData.resourceSC, amount, spawnData.target);

            var amountToSpawn = Mathf.Min(amount, _MAX_SPAWN);
            StartCoroutine(SpawnCoroutine(spawnData, amountToSpawn));
        }
        private IEnumerator SpawnCoroutine(ResourceSpawnData spawnData, int amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var resource = UdarPool.Instance.Get(spawnData.resourceSC.resourcePfb, spawnData.startTransform.position, Quaternion.identity);
                resource.Setup(spawnData.target.transform);
                OnSpawned?.Invoke();
                yield return UdarPool.Instance.GetWaitForSeconds(0.1f);
            }
        }


    }
    [Serializable]
    public struct ResourceSpawnData
    {
        public ResourceSC resourceSC;
        public Vector2 spawnAmount;
        [HideInInspector] public Transform startTransform;
        [HideInInspector] public GameObject target;
    }

}
