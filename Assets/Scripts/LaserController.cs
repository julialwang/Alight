using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    #region Properties

    [SerializeField] private bool shoot_laser = true;

    #endregion

    #region Components

    [SerializeField] private Transform _transform;
    [SerializeField] private Material laser_mat;
    private GameObject cur_laser;
    
    public AudioClip powerOn;
    public AudioClip powerOff;
    private AudioSource audioSource;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Check if at target, snap to target
        if (cur_laser) {
             GameObject.Destroy(cur_laser);
        }
        if (shoot_laser) {
            Vector3 start_point = _transform.position;
            Quaternion rotation_pos = _transform.rotation;
            Vector3 direction = transform.TransformDirection(Vector3.right);

            // Ignore layer 2
            int layerMask = ~(1 << 2);

            RaycastHit hit;
            if (Physics.Raycast(start_point, direction, out hit, Mathf.Infinity, layerMask)) {
                Vector3 end_point = hit.point;
                DrawLine(start_point, end_point, Color.white);
                GameObject target = hit.collider.transform.gameObject;
                if (target.name == "Laser ColliderActual") {
                    LaserDetectorController script = target.GetComponent<LaserDetectorController>();
                    script.laserHitMe();
              }
            } else {
                Vector3 new_end = start_point + direction * 10000;
                DrawLine(start_point, new_end, Color.white);
            }
        }
    }

    void turnOn() {
        shoot_laser = true;
        audioSource.PlayOneShot(powerOn, 0.6f);
    }

    void turnOf() {
        shoot_laser = false;
        audioSource.PlayOneShot(powerOff, 0.6f);
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
         {

            GameObject laser = new GameObject();
            laser.transform.position = start;
            laser.AddComponent<LineRenderer>();
            LineRenderer line = laser.GetComponent<LineRenderer>();
            line.material = laser_mat;
            line.startColor = color;
            line.endColor = color;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            cur_laser = laser;
         }
    #endregion
}
