using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowDrg : MonoBehaviour
{

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     mPosDelta = Input.mousePosition - mPrevPos;
        //     if (Vector3.Dot(transform.up, Vector3.up) >= 0)
        //     {
        //         transform.Rotate(transform.forward, -Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
        //     } // if
        //     else
        //     {
        //         transform.Rotate(transform.forward, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
        //     } // else

        //     // transform.Rotate( transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
        // } // if

        // mPrevPos = Input.mousePosition;
    } // update

    public float angle = 3.0f;
    void OnMouseDrag()
    {
        float x = Input.GetAxis("Mouse X");
        this.transform.RotateAround(transform.position, this.transform.forward * x, angle);
    }

} // end of class
