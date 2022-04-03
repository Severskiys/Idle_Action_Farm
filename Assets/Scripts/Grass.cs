using UnityEngine;
using System.Collections;
using EzySlice;
using System;
using DG.Tweening;

public class Grass : MonoBehaviour
{
    [SerializeField] private Transform _cutPoint;
    [SerializeField] private float _growTimerMax;
    [SerializeField] private float _growHeight;
    [SerializeField] private float _grassShatterDestroyTime;

    private float _growTimerCurrent;
    private float _grassAmount;
    private Vector3 _startGrowPosition;
    private Vector3 _endGrowPosition;
    private bool _growCoroutineEnded;
    private Basket _basket;

    public Action<float, float> GrassCutted;

    private void OnEnable()
    {
        _basket = FindObjectOfType<Basket>();
        GrassCutted += _basket.FillBasket;
        _startGrowPosition = transform.position;
        _endGrowPosition = new Vector3(_startGrowPosition.x, _startGrowPosition.y + _growHeight, _startGrowPosition.z);
        StartCoroutine(Grow());
    }

    private void OnDisable()
    {
        GrassCutted -= _basket.FillBasket;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cutter _cutter))
            GrassCut(_cutter.transform);
    }

    private void GrassCut(Transform cutterTransform)
    {
        CalculateCuttedGrassAmount();
        GrassCutted?.Invoke(_grassAmount, _grassShatterDestroyTime);
        SliceGrassRenderer(cutterTransform);
        RefreshGrow();
    }

    private void CalculateCuttedGrassAmount()
    {
        _grassAmount = _growTimerCurrent / _growTimerMax;
    }

    private void SliceGrassRenderer(Transform cutterTransform)
    {
        GameObject[] shatters = gameObject.SliceInstantiate(cutterTransform.position, Vector3.up, gameObject.GetComponent<Renderer>().material);
        if (shatters != null)
        {
            Destroy(shatters[0]);
            StartCoroutine(FlyToTheBasket(shatters[1]));
        }
    }

    private void RefreshGrow()
    {
        _growTimerCurrent = 0;
        transform.position = _startGrowPosition;
        if (_growCoroutineEnded)
            StartCoroutine(Grow());
    }

    private IEnumerator FlyToTheBasket(GameObject shatter)
    {
        shatter.transform.DOScale(Vector3.zero, 2.0f);
        while (shatter != null)
        {
            shatter.transform.position = Vector3.Slerp(shatter.transform.position, _basket.GetPositionForGrassShatters(), 0.05f);
            Destroy(shatter, _grassShatterDestroyTime);
            yield return null;
        }
    }

    private IEnumerator Grow()
    {
        _growCoroutineEnded = false;

        while (_growTimerCurrent <= 10)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endGrowPosition, Time.deltaTime*_growHeight/_growTimerMax);
            _growTimerCurrent += Time.deltaTime;
             yield return null;
        }
        _growCoroutineEnded = true;
    }
}
