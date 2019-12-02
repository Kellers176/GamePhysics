using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray hitPoint = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        float hitDistance = 0;
        if (Physics.Raycast(hitPoint, out hit, Mathf.Infinity) && Input.GetMouseButton(0))
        {
            Vector3 targetPoint = hitPoint.GetPoint(hitDistance);

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);
        }
    }
}
