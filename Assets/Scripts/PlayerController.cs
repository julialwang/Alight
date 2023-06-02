using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    [SerializeField] private float _movementForce = 4000;
    [SerializeField] private float _maxMovementSpeed = 400;
    [SerializeField] private uint _jumpForce = 300;
    [Range(0f, 30f)] [SerializeField] private uint _lookSpeed = 10;
    [Range(0f, 90f)] [SerializeField] private float _yRotationLimit = 88f;

    private Vector2 _rotation = Vector2.zero;

    #endregion

    #region Components

    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    #endregion

    #region Unity

    // Update is called once per frame
    private void Update()
    {
        // Locking cursor
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        else if (Input.GetMouseButtonDown(0)) Cursor.lockState = CursorLockMode.Locked;

        // Player look direction
        var rotX = Input.GetAxis("Mouse X") * _lookSpeed;
        var rotY = Input.GetAxis("Mouse Y") * _lookSpeed;

        _rotation.x += rotX;
        _rotation.y += rotY;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);

        transform.localRotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        _camera.transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);
        
        // Player jump registration
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        _rigidbody.AddRelativeForce(new Vector3(horizontalInput, 0, verticalInput) * _movementForce);
        var rigidbodyVelocity = _rigidbody.velocity;
        var lateralVelocity = new Vector3(rigidbodyVelocity.x, 0, rigidbodyVelocity.z);
        _rigidbody.AddForce(-lateralVelocity * (_movementForce / _maxMovementSpeed), ForceMode.Acceleration);
    }

    private bool IsGrounded()
    {
        const float raycastDistance = 0.2f;
        var raycastOrigin =
            transform.position + (Vector3.up * 0.1f); // Offset the raycast origin slightly above the player's position
        return Physics.Raycast(raycastOrigin, Vector3.down, out _, raycastDistance);
    }

    #endregion
}