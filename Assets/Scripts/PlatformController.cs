using System;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    #region Components

    [SerializeField] private Rigidbody _rigidbody;
    public AudioClip platformMovement;
    private AudioSource audioSource;

    #endregion


    #region Unity

    private void Start() {
        _targetY = transform.position.y;
        _curRot = 0;
        _targetRot = 0; 
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Check if at target, snap to target
        var position = transform.position;
        if (Math.Abs(position.y - _targetY) < 0.1f)
        {
            position.y = _targetY;
            transform.position = position;
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            // Move towards target
            if (transform.position.y < _targetY) {
                _rigidbody.MovePosition(transform.position + Vector3.up * (_movementSpeed * Time.fixedDeltaTime));
                audioSource.PlayOneShot(platformMovement, 0.7f);
            } else {
                _rigidbody.MovePosition(transform.position + Vector3.down * (_movementSpeed * Time.fixedDeltaTime));
                audioSource.PlayOneShot(platformMovement, 0.7f);
            }
        }

        if (_curRot != _targetRot) {
            Quaternion dR;
            // Move towards target
            if (_curRot < _targetRot) {
                dR = Quaternion.Euler(Vector3.up * (_rotationSpeed * Time.fixedDeltaTime));
                _curRot++;
            } else {
                dR = Quaternion.Euler(Vector3.down * (_rotationSpeed * Time.fixedDeltaTime));
                _curRot--;
            }
            _rigidbody.MoveRotation(_rigidbody.rotation * dR);
        }
    }

    #endregion

    #region Public Methods

    public void SetTargetY(float targetY)
    {
        _targetY = targetY;
    }

    public void SetTargetRot(int rotationAmount) {
        _targetRot = rotationAmount;
    }


    #endregion

    #region Properties

    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _rotationSpeed = 30;
    private float _targetY;
    private int _targetRot;
    private int _curRot;

    #endregion
}