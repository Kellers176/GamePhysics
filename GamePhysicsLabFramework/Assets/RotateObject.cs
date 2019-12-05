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
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        
        Quaternion targetRotation = Quaternion.LookRotation(mousePos - transform.position);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);

        //Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 direction = Input.mousePosition - position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

      
    }
}
