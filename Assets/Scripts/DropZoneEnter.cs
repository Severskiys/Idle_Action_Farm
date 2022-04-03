using System;
using UnityEngine;

public class DropZoneEnter : MonoBehaviour
{
    private bool _isDropZoneEntered = false;
    public Action<bool> IsDropZoneEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _isDropZoneEntered = true;
            IsDropZoneEntered?.Invoke(_isDropZoneEntered);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _isDropZoneEntered = false;
            IsDropZoneEntered?.Invoke(_isDropZoneEntered);
        }
    }
}
