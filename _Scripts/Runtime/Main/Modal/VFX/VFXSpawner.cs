using System.Collections;
using System.Collections.Generic;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;

namespace Udarverse.VFX
{
    public class VFXSpawner : EventsExecuter
    {
       
        [SerializeField] private GameObject _pfbToSpawn;
        [SerializeField] private Vector3 _positionOffset;
       

        public override void Execute()
        {
            UdarPool.Instance.Get(_pfbToSpawn, transform.position + _positionOffset, Quaternion.identity);
        }
    }

}
