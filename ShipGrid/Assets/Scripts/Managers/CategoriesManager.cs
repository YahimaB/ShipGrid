using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CategoriesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuButtonPrefab;

    [SerializeField]
    private GameObject modulePrefab;

    [SerializeField]
    private Transform contentPanel;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private List<ModuleCategory> baseModuleTypes = new List<ModuleCategory>();

    [SerializeField]
    private Stack<ModuleCategory> order = new Stack<ModuleCategory>();
    private ModuleCategory currentCategory = null;

    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackButton());
    }

    public void Reset()
    {
        order = new Stack<ModuleCategory>();
        RefreshList(null);
    }

    private void RefreshList(ModuleCategory newCategory, bool isBack = false)
    {
        if (!isBack)
            order.Push(currentCategory);
        backButton.gameObject.SetActive(order.Count > 1);

        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        if (newCategory == null)
        {
            foreach (var category in baseModuleTypes)
                AddButton(category);
        }
        else if (newCategory.HoldsModules)
        {
            foreach (var module in newCategory.Modules)
                AddButton(module);
        }
        else
        {
            foreach (var category in newCategory.Categories)
                AddButton(category);
        }

        currentCategory = newCategory;
    }

    private void AddButton(ModuleScriptableObject module)//may add a list variable when sort list is added later
    {
        CategoryButton button = Instantiate(menuButtonPrefab, contentPanel).GetComponent<CategoryButton>();
        button.GetComponent<RectTransform>().localScale = Vector3.one;
        button.SetUpButton(module, OnModuleButton);
    }

    private void AddButton(ModuleCategory category)//may add a list variable when sort list is added later
    {
        CategoryButton button = Instantiate(menuButtonPrefab, contentPanel).GetComponent<CategoryButton>();
        button.GetComponent<RectTransform>().localScale = Vector3.one;
        button.SetUpButton(category, OnCategoryButton);
    }

    private void OnModuleButton(ModuleScriptableObject module)
    {
        var canvas = GetComponentInParent<Canvas>().rootCanvas;
        ModuleItem moduleItem = Instantiate(modulePrefab, canvas.transform).GetComponent<ModuleItem>();
        moduleItem.SetUpItem(module, ShipBuilder.SlotSize);

        ModuleItem.SetSelectedItem(moduleItem);
    }

    private void OnCategoryButton(ModuleCategory category)
    {
        RefreshList(category);
    }

    private void OnBackButton()
    {
        ModuleCategory category = order.Pop();
        RefreshList(category, true);
    }
}


[Serializable]
public class ModuleCategory
{
    public string Name;
    public Sprite Icon;
    public bool HoldsModules;

    public List<ModuleCategory> Categories = new List<ModuleCategory>();
    public List<ModuleScriptableObject> Modules = new List<ModuleScriptableObject>();
}
