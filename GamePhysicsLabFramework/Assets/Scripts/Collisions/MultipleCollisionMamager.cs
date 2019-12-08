﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCollisionMamager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] circle;
    public Transform startPosition;
    public GameObject Box;
    public GameObject[] walls;
    //public GameObject AABB;
    //public GameObject OBB;
    //bool colliding = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //player.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(circle.GetComponent<AxisAlignedBoundingBox2D>());
        for (int i = 0; i < circle.Length; i++)
        {
            if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsSphere(circle[i].GetComponent<SphereCollisionHull3D>()))
            {
                if (circle[i].gameObject.tag == "Death")
                {
                    Debug.Log("Colliding");
                    this.transform.position = startPosition.position;
                    //this.gameObject.GetComponent<Particle3D>().SetPosition(startPosition.position);
                    this.gameObject.GetComponent<Particle3D>().ResetInfo();

                }
                this.GetComponent<SphereCollisionHull3D>().col.resolveContacts();
            }
        }
        if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsAABB(Box.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>()))
        {
            if (Box.gameObject.tag == "Death")
            {
                Debug.Log("DEATHHHH");
                this.gameObject.GetComponent<Particle3D>().SetPosition(startPosition.position);
                this.gameObject.GetComponent<Particle3D>().ResetInfo();
            }
        }
        for (int j = 0; j < walls.Length; j++)
        {
            if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsAABB(walls[j].GetComponent<AxisAlignedBoundingBoxCollisionHull3D>()))
            {
                 this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                 Debug.Log("colliding");
//                 this.GetComponent<SphereCollisionHull3D>().col.resolveContacts();
                
            }
            else
            {
                Debug.Log("Not colliding");
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            }
        }

    }



}
