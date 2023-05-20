using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    #region Constant

    private const float BUTTON_BOTTOM_HEIGHT = -0.06f;

    #endregion

    #region Properties

    private bool _isPressed;
    public UnityEvent buttonPressed;
    public UnityEvent buttonReleased;

    #endregion

    #region Components

    [SerializeField] private Transform _buttonTransform;
    [SerializeField] private Rigidbody _buttonRigidbody;

    #endregion

    private void FixedUpdate()
    {
        _buttonRigidbody.velocity = Vector3.ClampMagnitude(_buttonRigidbody.velocity, 0.25f);

        switch (_buttonTransform.position.y)
        {
            case < BUTTON_BOTTOM_HEIGHT:
                _buttonTransform.position = new Vector3(_buttonTransform.position.x, BUTTON_BOTTOM_HEIGHT,
                    _buttonTransform.position.z);
                _buttonRigidbody.velocity = Vector3.zero;
                
                // Handle press event
                if (!_isPressed)
                {
                    _isPressed = true;
                    buttonPressed.Invoke();
                }
                break;
            case > 0:
                _buttonTransform.position = new Vector3(_buttonTransform.position.x, 0, _buttonTransform.position.z);
                _buttonRigidbody.velocity = Vector3.zero;
                
                // Handle release event
                if (_isPressed)
                {
                    _isPressed = false;
                    buttonReleased.Invoke();
                }
                break;
        }
    }
}