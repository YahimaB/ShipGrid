using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleItem : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    public static ModuleItem ActiveModule;
    private static bool isDragging = false;

    public int2 ModuleSize => Module != null ? Module.Size : int2.zero;
    public ModuleScriptableObject Module { get; private set; }

    public void SetUpItem(ModuleScriptableObject module, float slotSize)
    {
        Module = module;
        icon.overrideSprite = module.Icon;

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, module.Size.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, module.Size.y * slotSize);
        rect.localScale = Vector3.one;
    }

    private void Update()
    {
        if (isDragging)
        {
            ActiveModule.transform.position = Input.mousePosition;
        }
    }

    public static void SetSelectedItem(ModuleItem obj)
    {
        ActiveModule = obj;
        isDragging = true;
    }

    public static void ResetSelectedItem()
    {
        ActiveModule = null;
        isDragging = false;
    }
}
