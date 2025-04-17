using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptimizeScroll : MonoBehaviour
{
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private ScrollRect scrollRect;
    
    [SerializeField] private InventoryManager inventoryManager;

    private RectTransform[] rowTrs; 
    
    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(HandleScroll);
        
        StartCoroutine(DelayCullRows());
    }

    IEnumerator DelayCullRows()
    {
        yield return new WaitForEndOfFrame();
        UpdateVisibleItems();
    }
    
    private void HandleScroll(Vector2 value)
    {
        UpdateVisibleItems();
    }

    private const int BOTTOM_LEFT = 0;
    private const int TOP_LEFT = 1;
    private const int TOP_RIGHT = 2;
    private const int BOTTOM_RIGHT = 3;

    // Outside of scope to avoid GC
    Vector3[] rowCorners = new Vector3[4];
    Vector3[] viewPortCorners = new Vector3[4];
    private void UpdateVisibleItems()
    {
        // Implement your solution here
        // Access the array of inventory rows as needed: inventoryManager.inventoryRows
        viewPort.GetWorldCorners(viewPortCorners);
        rowTrs ??= inventoryManager.inventoryRows.Select(go => go.GetComponent<RectTransform>()).ToArray();
        
        foreach (var rowTr in rowTrs)
        {
            rowTr.GetWorldCorners(rowCorners);
            rowTr.gameObject.SetActive(!(rowCorners[BOTTOM_LEFT].y > viewPortCorners[TOP_LEFT].y ||
                           rowCorners[TOP_LEFT].y < viewPortCorners[BOTTOM_LEFT].y));
        }
    }
}
