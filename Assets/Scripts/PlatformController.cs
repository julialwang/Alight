using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    #region Properties

    [SerializeField] private float _movementSpeed = 5;
    private float _targetY;

    #endregion

    #region Components

    [SerializeField] private Rigidbody _rigidbody;

    #endregion

    public void SetTargetY(float targetY)
    {
        _targetY = targetY;
    }

    private void FixedUpdate()
    {
        // Check if at target, snap to target
        if (Math.Abs(transform.position.y - _targetY) < 0.1f)
        {
            transform.position = new Vector3(transform.position.x, _targetY, transform.position.z);
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            // Move towards target
            if (transform.position.y < _targetY)
            {
                _rigidbody.MovePosition(transform.position + Vector3.up * (_movementSpeed * Time.fixedDeltaTime));
            }
            else
            {
                _rigidbody.MovePosition(transform.position + Vector3.down * (_movementSpeed * Time.fixedDeltaTime));
            }
        }
    }
}