using System;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    #region Components

    [SerializeField] private Rigidbody _rigidbody;

    #endregion
    public AudioClip platformMovement;
    private AudioSource audioSource;

    #region Unity

    private void Start() {
        _targetY = transform.position.y;
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
                audioSource.PlayOneShot(platformMovement, 0.9f);
            } else {
                _rigidbody.MovePosition(transform.position + Vector3.down * (_movementSpeed * Time.fixedDeltaTime));
                audioSource.PlayOneShot(platformMovement, 0.9f);
            }
        }
    }

    #endregion

    #region Public Methods

    public void SetTargetY(float targetY)
    {
        _targetY = targetY;
    }

    #endregion

    #region Properties

    [SerializeField] private float _movementSpeed = 5;
    private float _targetY;

    #endregion
}