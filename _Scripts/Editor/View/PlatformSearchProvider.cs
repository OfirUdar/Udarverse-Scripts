using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Udarverse.Editor
{
    public class PlatformSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private readonly List<PlatformList> _platformCatgoryList;
        private readonly EntryPointPosition _entryPointPosition;


        private readonly Action<PlatformSC, EntryPointPosition> _onSelectFromList;
        public PlatformSearchProvider(List<PlatformList> platforms, EntryPointPosition entryPointPosition, Action<PlatformSC, EntryPointPosition> onSelect)
        {
            _platformCatgoryList = platforms;
            _entryPointPosition = entryPointPosition;
            _onSelectFromList = onSelect;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTreeList = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Platforms"), 0)
            };

            for (int i = 0; i < _platformCatgoryList.Count; i++)
            {
                searchTreeList.Add(new SearchTreeGroupEntry(new GUIContent(_platformCatgoryList[i].name), 1));

                for (int k = 0; k < _platformCatgoryList[i].list.Count; k++)
                {
                    var entry = new SearchTreeEntry(new GUIContent(_platformCatgoryList[i].list[k].nameDisplay))
                    {
                        level = 2,
                        userData = _platformCatgoryList[i].list[k]
                    };
                    searchTreeList.Add(entry);
                }
            }

            return searchTreeList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            _onSelectFromList(SearchTreeEntry.userData as PlatformSC, _entryPointPosition);

            return true;
        }



    }
}

