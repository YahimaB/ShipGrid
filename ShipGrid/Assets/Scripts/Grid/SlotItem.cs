using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.EventSystems;

public class SlotItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private SlotSector[] slotSectors = new SlotSector[4];

    [HideInInspector]
    public int2 GridPos;

    [HideInInspector]
    public ModuleItem Module;
    public int2 ModuleSize => Module.ModuleSize;

    [HideInInspector]
    public int2 ModuleStartPos;

    [HideInInspector]
    public int2 SectorOffset;

    public static SlotItem ActiveSlot;

    private ShipManager shipManager;

    private void Start()
    {
        text.text = GridPos.y + "," + GridPos.x;
        shipManager = GetComponentInParent<ShipManager>();

        foreach (var sector in slotSectors)
            sector.OnStateChange = OnSectorChanged;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActiveSlot = this;

        if (Module != null && ModuleItem.ActiveModule == null)
        {
            shipManager.ColorChangeLoop(Color.blue, ModuleSize, ModuleStartPos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ActiveSlot = null;

        if (Module != null && ModuleItem.ActiveModule == null)
        {
            shipManager.ColorChangeLoop(Color.white, ModuleSize, ModuleStartPos);
        }
    }

    public void OnSectorChanged(bool enter, int quadNum)
    {
        if (enter)
            ActiveSlot = this;

        if (ModuleItem.ActiveModule != null)
        {
            if (ModuleItem.ActiveModule.ModuleSize.x % 2 == 0)
            {
                SectorOffset.x = quadNum % 2 == 0 ? -1 : 0;
            }
            else
            {
                SectorOffset.x = 0;
            }
            if (ModuleItem.ActiveModule.ModuleSize.y % 2 == 0)
            {
                SectorOffset.y = quadNum > 2 ? -1 : 0;
            }
            else
            {
                SectorOffset.y = 0;
            }

            shipManager.RefreshColor(enter);
        }
    }
}

public class CompressedSlot
{
    public int2 GridPos;
    public int2 ModuleStartPos;
    public ModuleScriptableObject Module;

    public CompressedSlot(SlotItem slot)
    {
        GridPos = slot.GridPos;
        ModuleStartPos = slot.ModuleStartPos;
        if (slot.Module != null)
            Module = slot.Module.Module;
    }
}
