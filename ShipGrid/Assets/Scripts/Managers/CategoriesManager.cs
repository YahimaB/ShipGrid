using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategoriesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuButtonPrefab = default;

    [SerializeField]
    private GameObject modulePrefab = default;

    [SerializeField]
    private Transform contentPanel = default;

    [SerializeField]
    private Button backButton = default;

    [SerializeField]
    private List<MainCategory> mainCategories = new List<MainCategory>();

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

        switch (newCategory)
        {
            case null:
                foreach (var category in mainCategories)
                    AddButton(category);
                break;
            case MainCategory mainCategory:
                foreach (var subCategory in mainCategory.SubCategories)
                    AddButton(subCategory);
                break;
            case SubCategory subCategory:
                foreach (var module in subCategory.Modules)
                    AddButton(module);
                break;
        }

        currentCategory = newCategory;
    }

    private void AddButton(ModuleScriptableObject module)
    {
        CategoryButton button = Instantiate(menuButtonPrefab, contentPanel).GetComponent<CategoryButton>();
        button.GetComponent<RectTransform>().localScale = Vector3.one;
        button.SetUpButton(module, OnModuleButton);
    }

    private void AddButton(ModuleCategory category)
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

    [Serializable]
    private class MainCategory : ModuleCategory
    {
        public List<SubCategory> SubCategories = new List<SubCategory>();
    }

    [Serializable]
    private class SubCategory : ModuleCategory
    {
        public List<ModuleScriptableObject> Modules = new List<ModuleScriptableObject>();
    }
}

[Serializable]
public class ModuleCategory
{
    public string Name;
    public Sprite Icon;
}


