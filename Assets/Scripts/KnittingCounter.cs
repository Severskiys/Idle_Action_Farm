using UnityEngine;
using TMPro;

public class KnittingCounter : MonoBehaviour
{
    [SerializeField] Basket _basket;
    [SerializeField] TMP_Text _knitCounter;
    [SerializeField] TMP_Text _knitMax;

    private void Start()
    {
        _knitMax.text = _basket.GetMaxKnittingsCount().ToString();
    }
    private void OnEnable()
    {
        _basket.KnittingAddedInBasket += ChangeCounterValue;
        _basket.KnittingRemovedFromBasket += ChangeCounterValue;
    }

    private void OnDisable()
    {
        _basket.KnittingAddedInBasket -= ChangeCounterValue;
        _basket.KnittingRemovedFromBasket -= ChangeCounterValue;
    }

    private void ChangeCounterValue(int knitAmount)
    {
        _knitCounter.text = knitAmount.ToString();
    }
}
