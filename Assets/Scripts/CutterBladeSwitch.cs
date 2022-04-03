using UnityEngine;

public class CutterBladeSwitch : MonoBehaviour
{
    [SerializeField] BoxCollider _boxCollider;

    public void ActivateCutter()
    {
        _boxCollider.enabled = true;
    }

    public void DeactivateCutter()
    {
        _boxCollider.enabled = false;
    }
}
