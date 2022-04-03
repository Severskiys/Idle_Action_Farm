using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;

public class Barn : MonoBehaviour
{
    [SerializeField] private DropZoneEnter _dropZone;
    [SerializeField] private Basket _basket;
    [SerializeField] private Transform _knittingsDestination;
    [SerializeField] private float _knittingFlyTime;
    [SerializeField] private CinemachineVirtualCamera _dropCamera;

    public Action CoinEarned;

    private void OnEnable()
    {
        _dropZone.IsDropZoneEntered += CollectKnittings;
    }

    private void OnDisable()
    {
        _dropZone.IsDropZoneEntered -= CollectKnittings;
    }

    private void CollectKnittings(bool dropZoneEntered)
    {
        if (dropZoneEntered)
        {
            if (_basket.GetKnittings().Count != 0)
            {
                _dropCamera.Priority = 50;
                MoveKnittingsToBarn();
            }
        }
        else
            _dropCamera.Priority = 5;
    }

    private async void MoveKnittingsToBarn()
    {
        for (int i = _basket.GetKnittings().Count - 1; i >= 0; i--)
        {
            await _basket.GetKnittings()[i].transform.DOMove(_knittingsDestination.position, _knittingFlyTime)
                  .OnComplete(() => 
                  { 
                      _basket.DestroyCurrentKnitting(i);
                      CoinEarned?.Invoke();
                  })
                  .AsyncWaitForCompletion();
            await this.transform.DOShakeScale(_knittingFlyTime).AsyncWaitForCompletion();
        }
    }
}
