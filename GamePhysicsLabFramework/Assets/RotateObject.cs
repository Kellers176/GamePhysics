using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Vector3 prevPosition;
    Vector3 prevDelta;
    void Start()
    {
        prevPosition = Vector3.zero;
        prevDelta = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            //Set a rate at which we should turn
            float turnSpeed = 20 * Time.deltaTime;
            //Connect turning rate to horizonal motion for smooth transition
            float rotate = Input.GetAxis("Vertical") * turnSpeed;
            //            //Get current rotation
            //            float currentRotation = gameObject.transform.rotation.z;
            //            //Add current rotation to rotation rate to get new rotation
            //            Quaternion rotation = Quaternion.Euler (0, 0, currentRotation + rotate);
            //            //Move object to new rotation
            //            gameObject.transform.rotation = rotation;
            gameObject.transform.Rotate(Vector3.forward * rotate);
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            //Set a rate at which we should turn
            float turnSpeed = 20 * Time.deltaTime;
            //Connect turning rate to horizonal motion for smooth transition
            float rotate = Input.GetAxis("Horizontal") * turnSpeed;
            //            //Get current rotation
            //            float currentRotation = gameObject.transform.rotation.z;
            //            //Add current rotation to rotation rate to get new rotation
            //            Quaternion rotation = Quaternion.Euler (0, 0, currentRotation + rotate);
            //            //Move object to new rotation
            //            gameObject.transform.rotation = rotation;
            gameObject.transform.Rotate(rotate, 0, 0);
        }
    }
}
