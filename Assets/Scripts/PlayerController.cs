using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants

    private const float LOOK_SPEED = 10.0f;

    #endregion

    #region Properties

    [SerializeField] private float _movementSpeed = 200.0f;
    [Range(0f, 90f)] [SerializeField] private float _yRotationLimit = 88f;

    private Vector2 _rotation = Vector2.zero;

    #endregion

    #region Components

    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

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
        var rotX = Input.GetAxis("Mouse X") * LOOK_SPEED;
        var rotY = Input.GetAxis("Mouse Y") * LOOK_SPEED;

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

        _rigidbody.velocity = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput) *
                                                           (_movementSpeed * Time.fixedDeltaTime));
        print(1f / Time.fixedDeltaTime);
    }
}