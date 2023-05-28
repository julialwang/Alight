using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LaserController : MonoBehaviour
{
    #region Properties

    [SerializeField] private bool shoot_laser = true;
    [Range(1, 10)][SerializeField] private uint _maxDepth = 5;

    #endregion

    #region Components

    [SerializeField] private Transform _transform;
    [SerializeField] private Material laser_mat;
    private List<GameObject> cur_lasers;

    #endregion

    #region Unity

    private void Start()
    {
        cur_lasers = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        // Check if at target, snap to target
        if (cur_lasers.Any()) {
            foreach (GameObject laser in cur_lasers) {
                 GameObject.Destroy(laser);
            }
             cur_lasers = new List<GameObject>();
        }
        if (shoot_laser) {
            Vector3 start_point = _transform.position;
            Vector3 direction = transform.TransformDirection(Vector3.right);
            shoot(start_point, direction);
        }
    }

    void turnOn() {
        shoot_laser = true;
    }

    void turnOff() {
        shoot_laser = false;
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
            cur_lasers.Add(laser);
         }

    private void OnHit(RaycastHit hit, Vector3 orig_direction, int depth) {
        GameObject target = hit.collider.transform.gameObject;
        if (target.name == "Laser ColliderActual") {
            LaserDetectorController script = target.GetComponent<LaserDetectorController>();
            script.laserHitMe();
        }

        // if (mirror)
        //    shoot(hit.point, new_angle, depth + 1)

        if (target.name == "Refractor") {
            RefractorController script = target.GetComponent<RefractorController>();
            Vector3 p1, p2, d1, d2;
            script.getOutputs(out p1, out d1, out p2, out d2);
            shoot(p1, d1, depth+1);
            shoot(p2, d2, depth+1);
        }
    }

    private void shoot(Vector3 start_point, Vector3 direction, int depth = 0) {
        if (depth > _maxDepth) {
            return;
        }
        // Ignore layer 2
        int layerMask = ~(1 << 2);

        RaycastHit hit;
        if (Physics.Raycast(start_point, direction, out hit, Mathf.Infinity, layerMask)) {
            Vector3 end_point = hit.point;
            DrawLine(start_point, end_point, Color.white);
            OnHit(hit, direction, depth);
        } else {
            Vector3 new_end = start_point + direction * 10000;
            DrawLine(start_point, new_end, Color.white);
        }
    }


    #endregion
}
