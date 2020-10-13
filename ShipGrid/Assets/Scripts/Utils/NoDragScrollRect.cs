using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoDragScrollRect : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private ScrollRect scrollRect = default;

    public void OnBeginDrag(PointerEventData data)
    {
        scrollRect.StopMovement();
        scrollRect.enabled = false;
    }

    public void OnEndDrag(PointerEventData data)
    {
        scrollRect.enabled = true;
    }
}