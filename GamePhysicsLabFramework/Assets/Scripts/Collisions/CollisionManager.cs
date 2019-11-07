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
            if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsSphere(circle.GetComponent<SphereCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<SphereCollisionHull3D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBoxCollisionHull3D>()))
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
            Debug.Log("This is a box");
            if (this.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>().TestCollisionVsSphere(circle.GetComponent<SphereCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBoxCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }

            //OBB
            Debug.Log("This is a box");
            if (this.GetComponent<ObjectBoundingBoxCollisionHull3D>().TestCollisionVsSphere(circle.GetComponent<SphereCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<ObjectBoundingBoxCollisionHull3D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBoxCollisionHull3D>()))
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (this.GetComponent<ObjectBoundingBoxCollisionHull3D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBoxCollisionHull3D>()))
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
