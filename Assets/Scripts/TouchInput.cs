using UnityEngine;
using UnityEngine.UI;
using System;

public class TouchInput : MonoBehaviour
{
    private Touch _touchMovement;
    private Vector2 _firstTouchPoint;
    private Vector2 _inputVector;
    private Vector2 _deltaInputVector;
    private float _currentTouchShift;

    [SerializeField] private ButtonOverride _harvestButton;
    [SerializeField] private float _maxTouchShift;
    [SerializeField] private Image _touchPad;
    [SerializeField] private Image _touchPadCircle;

    public Action<bool> Harvesting;

    private void OnEnable()
    {
        _harvestButton.ButtonPresssed += HarvestStarted;
    }

    private void OnDisable()
    {
        _harvestButton.ButtonPresssed -= HarvestStarted;
    }

    private void HarvestStarted(bool isHarvestStarted)
    {
        Harvesting?.Invoke(isHarvestStarted);
    }

    public Vector3 GetMovementInput()
    {
        //_harvestButton.transform.position
        if (Input.touchCount > 0)
        {
            _touchMovement = Input.GetTouch(0);
            _deltaInputVector = ReadInputDeltaVector(_touchMovement);
            return new Vector3(_deltaInputVector.x, 0, _deltaInputVector.y);
        }
        else
            return Vector3.zero;
    }

    private Vector2 ReadInputDeltaVector(Touch touch)
    {
        switch (_touchMovement.phase)
        {
            case TouchPhase.Began:
                _firstTouchPoint = _touchMovement.position;
                ShowTouchPad();
                return Vector2.zero;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                _currentTouchShift = Vector2.Distance(_touchMovement.position, _firstTouchPoint);
                _inputVector = (_currentTouchShift < _maxTouchShift) ? _touchMovement.position : Vector2.Lerp(_firstTouchPoint, _touchMovement.position, _maxTouchShift / _currentTouchShift);
                _touchPad.transform.position = _inputVector;
                return _inputVector - _firstTouchPoint;

            case TouchPhase.Ended:
                HideTouchPad();
                return Vector2.zero;

            default:
                return Vector2.zero;
        }
    }

    private void ShowTouchPad()
    {
        _touchPadCircle.transform.position = _touchMovement.position;
        _touchPad.transform.position = _touchMovement.position;
        _touchPad.gameObject.SetActive(true);
        _touchPadCircle.gameObject.SetActive(true);
    }
    private void HideTouchPad()
    {
        _touchPad.gameObject.SetActive(false);
        _touchPadCircle.gameObject.SetActive(false);
    }
}
