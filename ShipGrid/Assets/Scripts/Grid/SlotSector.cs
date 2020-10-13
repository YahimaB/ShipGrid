using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using UnityEngine.Events;

public class SlotSector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private int QuadNum = 0;

    public UnityAction<bool, int> OnStateChange;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnStateChange?.Invoke(true, QuadNum);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnStateChange?.Invoke(false, QuadNum);
    }
}
