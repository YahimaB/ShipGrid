using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    public static float SlotSize { get; private set; }

    private ShipScriptableObject currentShape;
    private SlotItem[,] slotGrid;

    public SlotItem[,] BuildShip(ShipScriptableObject shipShape)
    {
        currentShape = shipShape;
        slotGrid = new SlotItem[currentShape.Height, currentShape.Width];
        ResizePanel();
        CreateSlots();
        return slotGrid;
    }

    private void ResizePanel()
    {
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransform rect = GetComponent<RectTransform>();
        SlotSize = Mathf.Min(parentRect.rect.width / currentShape.Width, parentRect.rect.height / currentShape.Height);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize * currentShape.Width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize * currentShape.Height);

        rect.localScale = Vector3.one;
    }

    private void CreateSlots()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int y = 0; y < currentShape.Height; y++)
        {
            for (int x = 0; x < currentShape.Width; x++)
            {
                if (currentShape[y, x])
                {
                    SlotItem slot = Instantiate(slotPrefab, transform).GetComponent<SlotItem>();

                    slot.transform.name = "slot[" + y + "," + x + "]";

                    RectTransform rect = slot.transform.GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.pivot = new Vector2(0, 1);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize);
                    rect.localScale = Vector3.one;
                    rect.anchoredPosition = new Vector3(x * SlotSize, - y * SlotSize, 0);                  

                    slot.GridPos = new int2(x, y);
                    slotGrid[y, x] = slot;
                }
            }
        }
    }
}
