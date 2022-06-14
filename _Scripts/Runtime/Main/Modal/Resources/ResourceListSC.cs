using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Resources
{
    [CreateAssetMenu(fileName = "Resources List", menuName = "ScriptableObjects/Resources/ResourcesList", order =0)]
    public class ResourceListSC : ScriptableObject
    {
        [SerializeField] private List<ResourceSC> _resources = new List<ResourceSC>();


        public ResourceCollector GetResourcePfb(ResourceSC resourceSC)
        {
            var resource = _resources.Find((resourceWithOb) => resourceWithOb == resourceSC);
            return resource.resourcePfb;
        }
        public ResourceSC GetResource(string guid)
        {
            var resource = _resources.Find((resourceWithOb) => resourceWithOb.GUID == guid);
            return resource;
        }

        public List<ResourceSC> GetResources()
        {
            return _resources;
        }
    }
}

