using System;
using UnityEngine;

[RequireComponent(typeof(TouchInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _scytheAcceleration;
    [SerializeField] private float _maxSpeed;
    private TouchInput _touchInput;
    private Rigidbody _rigidbody;
    private float _currentAcceleration;
    private Vector3 _movementDirection;
    private bool _isHarvesting;
    public Action PlayerMoving, 
                  PlayerIdle,
                  PlayerHarvesting;

    private void Awake()
    {
        _touchInput = GetComponent<TouchInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _touchInput.Harvesting += IsHarvesting;
    }

    private void OnDisable()
    {
        _touchInput.Harvesting += IsHarvesting;
    }

    private void Update()
    {
        Move();
        LookAtDirection();
        CheckPlayerState();
    }

    private void IsHarvesting(bool isHarvesting)
    {
        _isHarvesting = isHarvesting;
    }

    private void Move()
    {
        _movementDirection = _touchInput.GetMovementInput();
        if (_movementDirection == Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        _currentAcceleration = (_isHarvesting) ? _scytheAcceleration : _acceleration;
        _rigidbody.AddForce(_movementDirection * _currentAcceleration * Time.deltaTime, ForceMode.VelocityChange);

        if (_rigidbody.velocity.magnitude >= _maxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
    }

    private void LookAtDirection()
    {
        if (_movementDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementDirection);
    }

    private void CheckPlayerState()
    {
        if (_isHarvesting)
        {
            PlayerHarvesting?.Invoke();

        } else if (_movementDirection == Vector3.zero)
        {
            PlayerIdle?.Invoke();
        }
        else
        {
            PlayerMoving?.Invoke();
        }

    }
}
