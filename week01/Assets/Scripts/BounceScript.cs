﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
    }
}
