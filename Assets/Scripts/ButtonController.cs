using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    #region Constant

    private const float BUTTON_BOTTOM_HEIGHT = -0.04f;

    #endregion

    #region Unity

    private void FixedUpdate()
    {
        // Reduce movement speed
        _buttonRigidbody.velocity = Vector3.ClampMagnitude(_buttonRigidbody.velocity, 0.25f);

        // Edit button position
        var buttonPosition = _buttonTransform.position;

        switch (buttonPosition.y)
        {
            // Button is being pressed downward
            case < BUTTON_BOTTOM_HEIGHT:
                buttonPosition.y = BUTTON_BOTTOM_HEIGHT;
                _buttonTransform.position = buttonPosition;
                _buttonRigidbody.velocity = Vector3.zero;

                // Handle press event
                if (!_isPressed)
                {
                    _isPressed = true;
                    buttonPressed.Invoke();
                }

                break;

            // Button is coming back up
            case > 0:
                buttonPosition.y = 0;
                _buttonTransform.position = buttonPosition;
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
}