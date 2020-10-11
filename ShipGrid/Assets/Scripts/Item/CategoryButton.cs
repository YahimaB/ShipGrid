using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class CategoryButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text nameText;
    private UnityAction OnButtonClicked;

    public void SetUpButton(ModuleScriptableObject item, Action<ModuleScriptableObject> onButtonClicked)
    {
        icon.overrideSprite = item.Icon;
        nameText.text = item.Name;
        OnButtonClicked = () => onButtonClicked?.Invoke(item);
    }

    public void SetUpButton(ModuleCategory item, Action<ModuleCategory> onButtonClicked)
    {
        icon.overrideSprite = item.Icon;
        nameText.gameObject.SetActive(false);
        OnButtonClicked = () => onButtonClicked?.Invoke(item);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonClicked?.Invoke();
    }
}
