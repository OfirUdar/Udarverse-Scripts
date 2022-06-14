
using Udar.Utils;
using Udarverse.Save;
using UnityEngine;

namespace Udarverse
{
    [SelectionBase]
    public class Platform : SaveableGameObject
    {
        // This relate if the plaftorm is branch or not: true -> it is not branch, false-> it is branch
        [ReadOnly] public bool IsBase = false;

        [Space]
        public PlatformSC platformSC;
        [field: SerializeField] public Collider HexagonColl { get; private set; }



    }
}