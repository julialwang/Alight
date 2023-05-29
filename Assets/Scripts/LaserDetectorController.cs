using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserDetectorController : MonoBehaviour
{
     #region Properties

    private int _laserCountdown = 0;
    public UnityEvent laserHit;
    public UnityEvent laserLeave;

    #endregion
    public void laserHitMe()
    {
        if (_laserCountdown < 2) {
            laserHit.Invoke();
        }
        _laserCountdown = 0;
    }

    private void FixedUpdate()
    {
        if (_laserCountdown > 2) {
            laserLeave.Invoke();
        } else {
            _laserCountdown += 1;
        }

    }
}
