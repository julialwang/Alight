using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    #region Properties
    Vector3 prev_pos;
    Quaternion prev_rot;
    #endregion

    #region Components

    [SerializeField] private Transform _transform;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        raycast();
        prev_pos = _transform.position;
        prev_rot = _transform.rotation;
    }

    private void FixedUpdate()
    {
        // Check if at target, snap to target
        var position = _transform.position;
        var rotation = _transform.rotation;
        if (prev_pos != position || prev_rot != rotation) {
            raycast();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        raycast();
    }

    void OnTriggerExit(Collider other)
    {
        raycast();
    }

    private void raycast()
    {
        Vector3 start_point = _transform.position;
        Quaternion rotation_pos = _transform.rotation;
        Vector3 direction = transform.TransformDirection(Vector3.right);

        RaycastHit hit;
        float new_x;
        if (Physics.Raycast(start_point, direction, out hit, Mathf.Infinity)) {
            new_x = hit.distance / (_transform.lossyScale.x / _transform.localScale.x);
        } else {
            new_x = 10000000;
        }

        _transform.localScale = new Vector3(new_x,
                                            _transform.localScale.y,
                                            _transform.localScale.z);
    }

    #endregion
}
