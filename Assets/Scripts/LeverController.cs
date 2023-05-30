using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour
{

    #region Unity

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Reduce movement speed
        _leverRigidbody.velocity = Vector3.ClampMagnitude(_leverRigidbody.velocity, 0.25f);

        // Edit lever position
        var leverPosition = _leverTransform.localPosition;
        
        if (leverPosition.x < -_lever_width/2) {
                leverPosition.x = -_lever_width/2;
                _leverTransform.localPosition = leverPosition;
                _leverRigidbody.velocity = Vector3.zero;

                // Handle press event
                if (!_isPressed)
                {
                    _isPressed = true;
                    leverPressed.Invoke();
                }
                audioSource.PlayOneShot(leverClick, 0.8f);
            // Button is coming back up
        } else if (leverPosition.x > _lever_width/2) {
                leverPosition.x = _lever_width/2;
                _leverTransform.localPosition = leverPosition;
                _leverRigidbody.velocity = Vector3.zero;

                // Handle release event
                if (_isPressed)
                {
                    _isPressed = false;
                    leverReleased.Invoke();
                }
                audioSource.PlayOneShot(leverClick, 0.8f);
        }
    }

    #endregion

    #region Properties

    public UnityEvent leverPressed;
    public UnityEvent leverReleased;

    #endregion

    #region Components

    [SerializeField] private Transform _leverTransform;
    [SerializeField] private Rigidbody _leverRigidbody;
    [SerializeField] private float _lever_width = 1f;
    [SerializeField] private bool _isPressed = false;
    public AudioClip leverClick;
    private AudioSource audioSource;

    #endregion
}