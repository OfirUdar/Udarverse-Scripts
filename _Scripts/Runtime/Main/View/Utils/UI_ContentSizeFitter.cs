using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_ContentSizeFitter : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [Header("Padding")]
    [SerializeField] private float _vertical = 10;
    private RectTransform _rect;
    private List<RectTransform> _children= new List<RectTransform>();
    private void Awake()
    {
        _rect = transform.GetComponent<RectTransform>();
        var children = _parent.GetComponentsInChildren<RectTransform>(false);
        foreach (var child in children)
        {
            if (child.parent == _parent)
                _children.Add(child);
        }
        
    }
    private void OnEnable()
    {
        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        var size = _rect.sizeDelta;
        size.y = 0;
        foreach (var child in _children)
        {
            if (size.y < child.rect.height)
                size.y = child.rect.height;
        }
        size.y += _vertical;
        _rect.sizeDelta = size;
    }

}
