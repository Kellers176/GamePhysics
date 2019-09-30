using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject circle;
    public GameObject AABB;
    public GameObject OBB;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(circle.GetComponent<AxisAlignedBoundingBox2D>());


        if (gameObject.GetComponent<MeshFilter>().mesh.name == "Sphere Instance")
        {
            Debug.Log("This is a circle");
            //circle
            if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsCircle(circle.GetComponent<CircleCollisionHull2D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
        else if (gameObject.GetComponent<MeshFilter>().mesh.name == "Cube Instance")
        {
            //AABB
            if (gameObject.name == "PlayerBox")
            {

                Debug.Log("This is a box");
                if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsCircle(circle.GetComponent<CircleCollisionHull2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                }
            }


            //OBB
            if (gameObject.name == "PlayerBoxOBB")
            {
                Debug.Log("This is a box");
                if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsCircle(circle.GetComponent<CircleCollisionHull2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>()))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                }
            }
        }

    }
}
