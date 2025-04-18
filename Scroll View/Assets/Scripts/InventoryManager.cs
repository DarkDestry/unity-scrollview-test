using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private RectTransform contentHolder;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private InventoryRow inventoryRow;

    [SerializeField] private int numRows = 10;
    [SerializeField] private int numPoolRows = 10;
    public Queue<GameObject> inventoryRowPool;
    
    public int NumRows => numRows;
    public Vector2 RowSize { get; private set; }

    private void Start()
    {
        inventoryRowPool = new();
        PopulateInventoryDisplay();
    }

    private void PopulateInventoryDisplay()
    {
        for (var i = 0; i < numPoolRows; i++)
        {
            var row = Instantiate(inventoryRow, contentHolder);
            if (i == 0)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(row.GetComponent<RectTransform>());
                var rowLayout = row.GetComponent<HorizontalLayoutGroup>();
                RowSize = new Vector2(rowLayout.preferredWidth, rowLayout.preferredHeight);
            }

            row.GetComponent<CanvasGroup>().alpha = 0;
            // row.gameObject.SetActive(false);
            inventoryRowPool.Enqueue(row.gameObject);
        }

        SetContentHeight();
    }

    private void SetContentHeight()
    {
        var contentHeight = RowSize.y * numRows +
                            verticalLayoutGroup.spacing * (numRows - 1) +
                            verticalLayoutGroup.padding.top +
                            verticalLayoutGroup.padding.bottom;
        var contentWidth = RowSize.x;

        verticalLayoutGroup.enabled = false;
        contentHolder.sizeDelta = new Vector2(contentWidth, contentHeight);
    }

    public void ReturnToPool(GameObject go)
    {
        inventoryRowPool.Enqueue(go);
        go.GetComponent<CanvasGroup>().alpha = 0;
        // go.SetActive(false);
    }

    public InventoryRow InitFromPool(int index)
    {
        if (inventoryRowPool.Count == 0)
        {
            var newPoolItem = Instantiate(inventoryRow, contentHolder).gameObject;
            inventoryRowPool.Enqueue(newPoolItem);
        }
        var poolItem = inventoryRowPool.Dequeue();

        var rowComponent = poolItem.GetComponent<InventoryRow>();
        rowComponent.Init(index);

        poolItem.GetComponent<CanvasGroup>().alpha = 1;
        // poolItem.gameObject.SetActive(true);
        return rowComponent;
    }
}