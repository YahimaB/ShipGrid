using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private ShipBuilder shipBuilder = default;

    [SerializeField]
    private Transform modulesHolder = default;

    [SerializeField]
    private GameObject modulePrefab = default;

    private int2 gridSize;
    private SlotItem[,] slotGrid;

    private int2 ActiveModulePos;
    private CheckState lastCheck;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (ModuleItem.ActiveModule != null)
            {
                if (SlotItem.ActiveSlot != null && lastCheck != CheckState.Invalid)
                    StoreModule(ModuleItem.ActiveModule);
                else
                    DestroyModule(ModuleItem.ActiveModule);
                ModuleItem.ResetSelectedItem();
            }
            else if (SlotItem.ActiveSlot != null && SlotItem.ActiveSlot.Module != null)
            {
                EmptySlot(SlotItem.ActiveSlot);
            }
        }
    }

    public void CreateShip(ShipScriptableObject ship)
    {
        foreach (Transform child in modulesHolder)
        {
            Destroy(child.gameObject);
        }

        slotGrid = shipBuilder.BuildShip(ship);
        gridSize = new int2(ship.Width, ship.Height);
    }

    public CompressedSlot[,] Compress()
    {
        CompressedSlot[,] compressedGrid = new CompressedSlot[gridSize.y, gridSize.x];

        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                SlotItem slot = slotGrid[i, j];

                if (slot != null)
                    compressedGrid[i, j] = new CompressedSlot(slot);
            }
        }

        return compressedGrid;
    }

    public void Decompress(CompressedSlot[,] compressedGrid)
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                CompressedSlot slot = compressedGrid[i, j];
                if (slot != null && slot.Module != null)
                {
                    var canvas = GetComponentInParent<Canvas>().rootCanvas;
                    ModuleItem moduleItem = Instantiate(modulePrefab, canvas.transform).GetComponent<ModuleItem>();
                    moduleItem.SetUpItem(slot.Module, ShipBuilder.SlotSize);
                    ActiveModulePos = slot.ModuleStartPos;
                    StoreModule(moduleItem);
                }
            }
        }
    }

    public void ColorChangeLoop(Color32 color, int2 size, int2 startPos)
    {
        SlotItem slot;

        for (int i = startPos.y; i < startPos.y + size.y; i++)
        {
            for (int j = startPos.x; j < startPos.x + size.x; j++)
            {
                if (i < 0 || i >= gridSize.y || j < 0 || j >= gridSize.x)
                    continue;

                slot = slotGrid[i, j];
                if (slot != null)
                    slot.GetComponent<Image>().color = color;
            }
        }
    }

    public void RefreshColor(bool enter)
    {
        int2 size = ModuleItem.ActiveModule.ModuleSize;
        int2 halfOffset = new int2(size.x / 2, size.y / 2);
        ActiveModulePos = SlotItem.ActiveSlot.GridPos - (halfOffset + SlotItem.ActiveSlot.SectorOffset);

        if (enter)
        {
            lastCheck = SlotCheck(ActiveModulePos, size);
            switch (lastCheck)
            {
                case CheckState.Empty:
                    ColorChangeLoop(Color.green, size, ActiveModulePos);
                    break;
                case CheckState.Overlap:
                    ColorChangeLoop(Color.yellow, size, ActiveModulePos);
                    break;
                case CheckState.Invalid:
                    ColorChangeLoop(Color.red, size, ActiveModulePos);
                    break;
            }
        }
        else
        {
            ColorChangeLoop(Color.white, size, ActiveModulePos);
        }
    }

    private CheckState SlotCheck(int2 startPos, int2 size)
    {
        bool freeSpace = true;
        SlotItem slotItem;

        for (int i = startPos.y; i < startPos.y + size.y; i++)
        {
            for (int j = startPos.x; j < startPos.x + size.x; j++)
            {
                if (i < 0 || i >= gridSize.y || j < 0 || j >= gridSize.x)
                    return CheckState.Invalid;

                slotItem = slotGrid[i, j];
                if (slotItem == null)
                    return CheckState.Invalid;

                freeSpace &= slotItem.Module == null;
            }
        }

        if (!freeSpace)
            return CheckState.Overlap;
        else
            return CheckState.Empty;
    }

    private void StoreModule(ModuleItem module)
    {
        SlotItem slot;
        int2 size = module.ModuleSize;
        for (int y = ActiveModulePos.y; y < ActiveModulePos.y + size.y; y++)
        {
            for (int x = ActiveModulePos.x; x < ActiveModulePos.x + size.x; x++)
            {
                slot = slotGrid[y, x];

                if (slot.Module != null)
                    EmptySlot(slot);

                slot.Module = module;
                slot.ModuleStartPos = ActiveModulePos;
            }
        }

        ColorChangeLoop(Color.white, size, ActiveModulePos);

        module.transform.SetParent(modulesHolder);
        module.GetComponent<RectTransform>().pivot = new Vector2(0,1);
        module.transform.position = slotGrid[ActiveModulePos.y, ActiveModulePos.x].transform.position;
    }

    private void EmptySlot(SlotItem slotObject)
    {
        ModuleItem module = slotObject.Module;
        int2 startPos = slotObject.ModuleStartPos;
        int2 size = slotObject.ModuleSize;

        SlotItem slot;
        for (int y = startPos.y; y < startPos.y + size.y; y++)
        {
            for (int x = startPos.x; x < startPos.x + size.x; x++)
            {
                slot = slotGrid[y, x];
                slot.Module = null;
                slot.ModuleStartPos = int2.zero;
            }
        }

        ColorChangeLoop(Color.white, size, startPos);
        Destroy(module.gameObject);
    }

    private void DestroyModule(ModuleItem module)
    {
        ColorChangeLoop(Color.white, module.ModuleSize, ActiveModulePos);
        Destroy(module.gameObject);
    }

    private enum CheckState
    {
        Empty = 0,
        Overlap = 1,
        Invalid = 2
    }
}
