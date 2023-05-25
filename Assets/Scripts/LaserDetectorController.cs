using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserDetectorController : MonoBehaviour
{
     #region Properties

    private int _isLasered = 0;
    public UnityEvent laserHit;
    public UnityEvent laserLeave;

    #endregion
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Laser") {
            if (_isLasered == 0) {
                laserHit.Invoke();
            }
            _isLasered += 1;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Laser") {
            _isLasered -= 1;
            if (_isLasered == 0) {
                laserLeave.Invoke();
            }
        }
    }

}
