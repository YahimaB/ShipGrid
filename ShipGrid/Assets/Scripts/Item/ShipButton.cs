using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShipButton : MonoBehaviour
{
    [SerializeField]
    private Image icon = default;
    [SerializeField]
    private Text nameText = default;
    [SerializeField]
    private Button button = default;

    public ShipScriptableObject Ship { get; private set; }
    public CompressedSlot[,] CompressedGrid { get; set; }

    public void SetUpButton(ShipScriptableObject ship, Action<ShipButton> onButtonPressed)
    {
        button.onClick.RemoveAllListeners();

        icon.overrideSprite = ship.Icon;
        nameText.text = ship.Name;
        Ship = ship;
        CompressedGrid = null;
        
        button.onClick.AddListener(() => onButtonPressed?.Invoke(this));
    }
    
}
