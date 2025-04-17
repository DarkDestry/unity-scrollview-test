using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private RectTransform contentHolder;
    [SerializeField] private RectTransform poolHolder;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private InventoryRow inventoryRow;

    [SerializeField] private int numRows = 10;
    [SerializeField] private int numPoolRows = 10;
    public GameObject[] inventoryRowPool;

    public Vector2 RowSize { get; private set; }

    private void Start()
    {
        inventoryRowPool = new GameObject[numPoolRows];
        PopulateInventoryDisplay();
    }

    private void PopulateInventoryDisplay()
    {
        for (var i = 0; i < numPoolRows; i++)
        {
            var row = Instantiate(inventoryRow, poolHolder);
            if (i == 0)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(row.GetComponent<RectTransform>());
                var rowLayout = row.GetComponent<HorizontalLayoutGroup>();
                RowSize = new Vector2(rowLayout.preferredWidth, rowLayout.preferredHeight);
            }

            row.gameObject.SetActive(false);
            inventoryRowPool[i] = row.gameObject;
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
        go.transform.SetParent(poolHolder);
        go.SetActive(false);
    }

    public InventoryRow InitFromPool(int index)
    {
        var poolItem = poolHolder.GetChild(0);
        poolItem.transform.SetParent(contentHolder);

        var rowComponent = poolItem.GetComponent<InventoryRow>();
        rowComponent.Init(index);

        poolItem.gameObject.SetActive(true);
        return rowComponent;
    }
}