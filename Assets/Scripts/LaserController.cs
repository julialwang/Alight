using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;



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

    Vector3 Reflect(Vector3 N, Vector3 V) {
        Vector3 reflect_direct = 2 * Vector3.Dot(N, V) * N - V;
        return reflect_direct;
    }
    bool Refract(float indexOfRefraction, bool in_object, Vector3 N, Vector3 V, out Vector3 T) {
        float n;
        double cos_i;
        if (in_object) {
            n = indexOfRefraction/1.0f;

        } else {
            n = 1.0f/indexOfRefraction;
            cos_i = Vector3.Dot(N, V);
        }
        cos_i = Vector3.Dot(N, V);

        double cos_t = Math.Sqrt(1 - Math.Pow(n, 2) * (1 - Math.Pow(cos_i, 2)));

        bool is_T_valid = (1 - Math.Pow(n, 2) * (1 - Math.Pow(cos_i, 2))) >= 0;
        T = (float)(n * cos_i - cos_t) * N - n * V;

        return is_T_valid;
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

    private void OnHit(RaycastHit hit, Vector3 orig_direction, int depth, bool in_object = false) {
        GameObject target = hit.collider.transform.gameObject;
        if (target.name == "Laser ColliderActual") {
            LaserDetectorController script = target.GetComponent<LaserDetectorController>();
            script.laserHitMe();
        }

        // if (mirror)
        //    shoot(hit.point, new_angle, depth + 1)

        else if (target.name == "Refractor") {
            RefractorController script = target.GetComponent<RefractorController>();
            Vector3 p1, p2, d1, d2;
            script.getOutputs(out p1, out d1, out p2, out d2);
            shoot(p1, d1, depth+1);
            shoot(p2, d2, depth+1);
        } else if (target.name == "RefractPlane") {
            Vector3 p = hit.point + orig_direction.normalized/100;
            float rF = (float) Variables.Object(target).Get("refraction_factor");
            bool surface = (bool) Variables.Object(target).Get("surface");

            Vector3 N = hit.normal;
            bool next_in_object;
            if (surface) {
                // If we collide with the surface of an object, we move from inside an object to outside
                next_in_object = !in_object;
            } else {
                // Otherwise, we don't change our state
                next_in_object = in_object;
            }

            Vector3 T;

            if (Refract(rF, in_object, N, orig_direction, out T)) {
                shoot(p, T, depth+1, next_in_object);
            }

            if (!in_object) {
                T = Reflect(N, -orig_direction);
                p = hit.point - orig_direction.normalized/100;
                shoot(hit.point, T, depth+1, in_object);
            }
        } else if (target.name == "ReflectPlane") {
            Vector3 p = hit.point + orig_direction.normalized/100;
            Vector3 N = hit.normal;
            Vector3 T = Reflect(N, -orig_direction);
            p = hit.point - orig_direction.normalized/100;
            shoot(hit.point, T, depth+1, in_object);
        }

    }

    private void shoot(Vector3 start_point, Vector3 direction, int depth = 0, bool in_object = false) {
        if (depth > _maxDepth) {
            return;
        }
        // Ignore layer 2
        int layerMask = ~(1 << 2);

        RaycastHit hit;
        if (Physics.Raycast(start_point, direction, out hit, Mathf.Infinity, layerMask)) {
            Vector3 end_point = hit.point;
            DrawLine(start_point, end_point, Color.white);
            OnHit(hit, direction, depth, in_object = in_object);
        } else {
            Vector3 new_end = start_point + direction * 10000;
            DrawLine(start_point, new_end, Color.white);
        }
    }


    #endregion
}
