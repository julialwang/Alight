using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    [Range(1, 10)][SerializeField] private uint _movementSpeed = 5;
    [Range(0f, 30f)][SerializeField] private uint _lookSpeed = 10;
    [Range(0f, 90f)][SerializeField] private float _yRotationLimit = 88f;
    private uint _jumpHeight = 12;

    private Vector2 _rotation = Vector2.zero;

    #endregion

    #region Components

    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    #endregion
    public AudioClip jump;
    public AudioClip walk;
    private AudioSource audioSource;
    private bool isJumping;
    private bool isWalking;

    #region Unity

    void Start() {
        audioSource = GetComponent<AudioSource>();
        isWalking = false;
    }

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
            audioSource.PlayOneShot(jump, 0.7f);
        }
    }

    private void FixedUpdate()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 | verticalInput != 0) {
            isWalking = true;
            _rigidbody.velocity = Vector3.up * _rigidbody.velocity.y + transform.TransformDirection(
            new Vector3(horizontalInput, 0, verticalInput) * _movementSpeed);
        } else {
            isWalking = false;
        }

        if (isWalking && !audioSource.isPlaying) {
            audioSource.PlayOneShot(walk, 0.7f);
        }
    }

    private bool IsGrounded()
    {
        float raycastDistance = 0.2f;
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + (Vector3.up * 0.1f); // Offset the raycast origin slightly above the player's position
        return Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance);
    }

    #endregion
}