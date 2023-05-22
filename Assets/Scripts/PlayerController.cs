using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    [Range(1, 10)] [SerializeField] private uint _movementSpeed = 5;
    [Range(0f, 30f)] [SerializeField] private uint _lookSpeed = 10;
    [Range(0f, 90f)] [SerializeField] private float _yRotationLimit = 88f;
    [Range(0f, 50f)] [SerializeField] private uint _jumpHeight = 12;

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
            _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        _rigidbody.velocity = Vector3.up * _rigidbody.velocity.y + transform.TransformDirection(
            new Vector3(horizontalInput, 0, verticalInput) * _movementSpeed);
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