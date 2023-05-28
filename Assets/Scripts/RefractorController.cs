using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RefractorController : MonoBehaviour
{
    #region Properties
    [SerializeField] private Vector3 splitSide1;
    [SerializeField] private Vector3 splitSide2;
    #endregion

    // Update is called once per frame
    public void getOutputs(out Vector3 p1, out Vector3 d1, out Vector3 p2, out Vector3 d2)
    {
        d1 = transform.TransformDirection(splitSide1);
        p1 = transform.position + d1;

        d2 = transform.TransformDirection(splitSide2);
        p2 = transform.position + d2;
    }
}
