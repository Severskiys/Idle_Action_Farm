using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;

public class MoneyFlow : MonoBehaviour
{
    [SerializeField] private Barn _barn;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _coinInstPoint;
    [SerializeField] private int _oneCoinMoneyAmount;
    [SerializeField] private TMP_Text _coinCounterText;
    private int _currentPlayerMoney;


    private void OnEnable()
    {
        _barn.CoinEarned += OnCoinEarned;
    }

    private void OnDisable()
    {
        _barn.CoinEarned -= OnCoinEarned;
    }

    private void OnCoinEarned()
    {
        MoveCoin(CreateCoin());
    }

    private GameObject CreateCoin()
    {
        return Instantiate(_coinPrefab, _coinInstPoint.transform);
    }

    private void MoveCoin(GameObject coin)
    {
        Tween coinTween = coin.transform.DOMove(transform.position, 1.0f)
                                        .OnComplete(() =>
                                        {
                                            Destroy(coin);
                                            StartCoroutine(MoneyAddCoroutine());
                                            this.transform.DOShakeRotation(0.5f);

                                        });
    }

    private IEnumerator MoneyAddCoroutine()
    {
        for (int i = 0; i < _oneCoinMoneyAmount; i++)
        {
            _currentPlayerMoney++;
            _coinCounterText.text = _currentPlayerMoney.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        
    }
}
