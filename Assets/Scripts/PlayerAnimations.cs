using System;
using UnityEngine;

[RequireComponent(typeof(PlayerActions))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject _scytheModel;
    [SerializeField] private Animator _animator;
    private PlayerActions _playerActions;

    private void Awake()
    {
        _playerActions = GetComponent<PlayerActions>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _playerActions.PlayerHarvesting += OnPlayerHarvesting;
        _playerActions.PlayerMoving += OnPlayerMoving;
        _playerActions.PlayerIdle += OnPlayerIdle;
    }

    private void OnDisable()
    {
        _playerActions.PlayerHarvesting -= OnPlayerHarvesting;
        _playerActions.PlayerMoving -= OnPlayerMoving;
        _playerActions.PlayerIdle -= OnPlayerIdle;
    }

    private void OnPlayerIdle()
    {
        _scytheModel.SetActive(false);
        _animator.Play("Idle");
    }

    private void OnPlayerMoving()
    {
        _scytheModel.SetActive(false);
        _animator.Play("Walking");
    }

    private void OnPlayerHarvesting()
    {
        _scytheModel.SetActive(true);
        _animator.Play("Harvesting");
    }
}
