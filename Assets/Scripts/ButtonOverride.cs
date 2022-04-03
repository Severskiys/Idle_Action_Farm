using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOverride : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isButtonPressed;
    public Action<bool> ButtonPresssed;
    public void OnPointerDown(PointerEventData eventData)
    {
        _isButtonPressed = true;
        ButtonPresssed?.Invoke(_isButtonPressed);
    }

     public void OnPointerUp(PointerEventData eventData)
    {
        _isButtonPressed = false;
        ButtonPresssed?.Invoke(_isButtonPressed);
    }
}