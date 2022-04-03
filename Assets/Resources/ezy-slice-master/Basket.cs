using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private GameObject _knittingPattern;
    [SerializeField] private float _grassAmountForOneKnitting;
    [SerializeField] private float _knittingSide = 1.55f;
    [SerializeField] private Vector3 _endKnittingScale = new Vector3(3, 3, 3);
    [SerializeField] private PlayerActions _playerActions;
    [SerializeField] private int _maxKnittingsCount;
    private float _currentGrassAmount;
    private List<GameObject> _knittings = new List<GameObject>();
    private List<Tween> _knittingPileShakeTwins = new List<Tween>();
    private Vector3 _newKnittingPosition;
    public Action<int> KnittingAddedInBasket;
    public Action<int> KnittingRemovedFromBasket;

    private void Awake()
    {
        DOTween.Init();
    }

    private void OnEnable()
    {
        _playerActions.PlayerIdle += PausePileShakeTween;
        _playerActions.PlayerMoving += ContinuePileShakeTween;
    }

    private void OnDisable()
    {
        _playerActions.PlayerIdle -= PausePileShakeTween;
        _playerActions.PlayerMoving -= ContinuePileShakeTween;
    }

    private void PausePileShakeTween()
    {
        foreach (Tween tween in _knittingPileShakeTwins)
        {
            tween.Pause();
        }
    }

    private void ContinuePileShakeTween()
    {
        foreach (Tween tween in _knittingPileShakeTwins)
        {
            tween.Play();
        }
    }

    public void FillBasket(float grassValue, float grassTimer)
    {
        _currentGrassAmount += grassValue;
        if (_currentGrassAmount >= _grassAmountForOneKnitting)
        {
            if (_knittings.Count < _maxKnittingsCount)
            {
                _currentGrassAmount = 0;
                int knittingAmount = AddKnitting();
                AnimateKnitting(knittingAmount);
                KnittingAddedInBasket?.Invoke(knittingAmount);
            }
        }
    }

    private int AddKnitting()
    {
        _knittings.Add(Instantiate(_knittingPattern, GetPositionForGrassShatters(), transform.rotation, transform));
        int knittingIndex = _knittings.Count;
        return knittingIndex;
    }

    private void AnimateKnitting(int knittingIndex)
    {
        _knittings[knittingIndex - 1].transform.DOScale(_endKnittingScale, 1.5f).SetEase(Ease.InOutBounce);
        _knittingPileShakeTwins.Add(_knittings[knittingIndex - 1].transform.DOLocalMoveX(0.05f * knittingIndex, 1f)
                                                                            .SetEase(Ease.Linear)
                                                                            .SetLoops(-1, LoopType.Yoyo));
    }

    public Vector3 GetPositionForGrassShatters()
    {
        _newKnittingPosition = new Vector3(transform.position.x,
                                           transform.position.y + _knittings.Count * _knittingSide,
                                           transform.position.z);
        return _newKnittingPosition;
    }

    public List<GameObject> GetKnittings()
    {
        return _knittings;
    }

    public int GetMaxKnittingsCount()
    {
        return _maxKnittingsCount;
    }

    public void DestroyCurrentKnitting(int knittingIndex)
    {
        Destroy(_knittings[knittingIndex]);
        _knittings.RemoveAt(knittingIndex);
        KnittingRemovedFromBasket?.Invoke(_knittings.Count);
    }
}
