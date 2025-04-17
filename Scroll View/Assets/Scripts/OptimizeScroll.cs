using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptimizeScroll : MonoBehaviour
{
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    
    [SerializeField] private InventoryManager inventoryManager;

    private RectTransform[] rowTrs; 
    private Dictionary<int, InventoryRow> rowItems = new();
    
    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(HandleScroll);
        
        StartCoroutine(DelayCullRows());
    }

    IEnumerator DelayCullRows()
    {
        yield return new WaitForEndOfFrame();
        HandleScroll(Vector2.zero);
    }
    
    private void HandleScroll(Vector2 _)
    {
        var currentPos = verticalLayoutGroup.transform.localPosition;
        var height = viewPort.rect.height;
        var rowHeight = verticalLayoutGroup.spacing + inventoryManager.RowSize.y; 
        var startIndex = Mathf.FloorToInt(currentPos.y / rowHeight);
        var endIndex = Mathf.CeilToInt((currentPos.y + height) / rowHeight);

        var toRemoveIndices = new List<int>();
        foreach (var index in rowItems.Keys)
        {
            if (index >= startIndex && index <= endIndex) continue;
            var row = rowItems[index];
            toRemoveIndices.Add(index);
            inventoryManager.ReturnToPool(row.gameObject);
        }

        foreach (var index in toRemoveIndices) rowItems.Remove(index);

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (rowItems.ContainsKey(i)) continue;
            var row = inventoryManager.InitFromPool(i);
            var posX = verticalLayoutGroup.padding.left;
            var posY = verticalLayoutGroup.padding.top + //Top Padding 
                       rowHeight * i; // Previous Row heights and spacings
            row.transform.localPosition = new Vector2(posX, -posY);
            rowItems[i] = row;
        }

    }
}
