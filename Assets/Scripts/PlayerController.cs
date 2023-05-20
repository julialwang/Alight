using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    [Range(50, 500)] [SerializeField] private uint _movementSpeed = 250;
    [Range(0f, 30f)] [SerializeField] private uint _lookSpeed = 10;
    [Range(0f, 90f)] [SerializeField] private float _yRotationLimit = 88f;

    private Vector2 _rotation = Vector2.zero;

    #endregion

    #region Components

    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        // Locking cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Player look direction
        var rotX = Input.GetAxis("Mouse X") * _lookSpeed;
        var rotY = Input.GetAxis("Mouse Y") * _lookSpeed;

        _rotation.x += rotX;
        _rotation.y += rotY;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);

        transform.localRotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        _camera.transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);
    }

    private void FixedUpdate()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        _rigidbody.velocity = Vector3.up * _rigidbody.velocity.y + transform.TransformDirection(
            new Vector3(horizontalInput, 0, verticalInput) *
            (_movementSpeed * Time.fixedDeltaTime));
    }
}