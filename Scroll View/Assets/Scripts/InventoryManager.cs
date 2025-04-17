using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private RectTransform contentHolder;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private InventoryRow inventoryRow;
    
    [SerializeField] private int numRows = 10;
    public GameObject[] inventoryRows;

    private void Start()
    {
        inventoryRows = new GameObject[numRows];
        PopulateInventoryDisplay();
    }

    private void PopulateInventoryDisplay()
    {
        for (var i = 0; i < numRows; i++)
        {
            var row = Instantiate(inventoryRow, contentHolder);
            row.Init(i);
            inventoryRows[i] = row.gameObject;
        }
        SetContentHeight();
    }

    private void SetContentHeight()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        var contentHeight = verticalLayoutGroup.preferredHeight;
        var contentWidth = verticalLayoutGroup.preferredWidth;
        verticalLayoutGroup.enabled = false;
        contentHolder.sizeDelta = new Vector2(contentWidth, contentHeight);
    }
}