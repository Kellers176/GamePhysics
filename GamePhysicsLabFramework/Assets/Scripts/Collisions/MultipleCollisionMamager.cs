using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCollisionMamager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] circle;
    public Transform startPosition;
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
                    this.transform.position = startPosition.position;
                //colliding = true;
            }
        }
        for (int j = 0; j < walls.Length; j++)
        {
            if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsOBB(walls[j].GetComponent<ObjectBoundingBoxCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                //this.GetComponent<SphereCollisionHull3D>().col.resolveContact();
            }
            else
            {
                //this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }

        //for (int i = 0; i < circle.Length; i++)
        //{
        //    tempCollision = circle[i].GetComponent<SphereCollisionHull3D>().col;
        //    if (colliding)
        //    {
        //        circle[i].GetComponent<SphereCollisionHull3D>().col.orderContacts();
        //    }
        //    colliding = false;
        //}

    }



}
