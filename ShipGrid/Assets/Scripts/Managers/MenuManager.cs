using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipButtonPrefab = default;

    [SerializeField]
    private List<ShipScriptableObject> ships = new List<ShipScriptableObject>();

    [SerializeField]
    private ShipManager shipManager = default;

    [SerializeField]
    private CategoriesManager categoriesManager = default;

    [SerializeField]
    private Button backButton = default;

    [SerializeField]
    private GameObject messageWindow = default;

    ShipButton currentShip = null;

    void Start()
    {
        foreach (var ship in ships)
        {
            ShipButton button = Instantiate(shipButtonPrefab, transform).GetComponent<ShipButton>();
            button.SetUpButton(ship, OnShipOpen);
        }

        backButton.onClick.AddListener(OnBackButton);
    }

    public void OnBackButton()
    {
        gameObject.SetActive(true);

        bool showMessage = false;

        if (currentShip != null)
        {
            currentShip.CompressedGrid = shipManager.Compress();

            foreach (var slot in currentShip.CompressedGrid)
            {
                if (slot != null && slot.Module == null)
                {
                    showMessage = true;
                    break;
                }
            }

            currentShip = null;
        }

        if (showMessage)
            messageWindow.SetActive(true);

    }

    public void OnShipOpen(ShipButton shipButton)
    {
        shipManager.CreateShip(shipButton.Ship);
        if (shipButton.CompressedGrid != null)
            shipManager.Decompress(shipButton.CompressedGrid);
        currentShip = shipButton;

        categoriesManager.Reset();

        gameObject.SetActive(false);
    }
}
