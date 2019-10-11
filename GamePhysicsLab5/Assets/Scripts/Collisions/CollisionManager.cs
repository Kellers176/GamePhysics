using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] circle;
    //public GameObject AABB;
    //public GameObject OBB;
    bool colliding = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(circle.GetComponent<AxisAlignedBoundingBox2D>());
        CollisionHull2D.Collision tempCollision = new CollisionHull2D.Collision();

        for(int i = 0; i < circle.Length; i++)
        {
            for (int j = 0; j < circle.Length; j++)
            {
                if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsCircle(circle[j].GetComponent<CircleCollisionHull2D>(), ref circle[i].GetComponent<CircleCollisionHull2D>().col))
                {
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                //this.GetComponent<Particle2D>().collisionHull.col.setA(this.GetComponent<CircleCollisionHull2D>());
                //this.GetComponent<Particle2D>().collisionHull.col.setB(circle.GetComponent<CircleCollisionHull2D>());
                    colliding = true;
                //tempCollision.setParticleInfo(this.GetComponent<Particle2D>());
                    //circle[i].GetComponent<CircleCollisionHull2D>().col.orderContacts();
                    //circle[i].GetComponent<CircleCollisionHull2D>().col.resolveContact();
                }   
            }

        }

        for(int i = 0; i < circle.Length; i++)
        {
            tempCollision = circle[i].GetComponent<CircleCollisionHull2D>().col;
            if(colliding)
            {
                circle[i].GetComponent<CircleCollisionHull2D>().col.orderContacts();
                circle[i].GetComponent<CircleCollisionHull2D>().col.resolveContact();
            }
            colliding = false;
        }


            //else if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>(), ref tempCollision))
            //{
            //    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            //}
            //else if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>(), ref tempCollision))
            //{
            //    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            //}
            //else
            //{
            //    this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            //}
        }
        //else if (gameObject.GetComponent<MeshFilter>().mesh.name == "Cube Instance")
        //{
        //    //AABB
        //    if (gameObject.name == "PlayerBox")
        //    {
        //
        //        Debug.Log("This is a box");
        //        if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsCircle(circle.GetComponent<CircleCollisionHull2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else if (this.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        //        }
        //    }
        //
        //
        //    //OBB
        //    if (gameObject.name == "PlayerBoxOBB")
        //    {
        //        Debug.Log("This is a box");
        //        if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsCircle(circle.GetComponent<CircleCollisionHull2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsAABB(AABB.GetComponent<AxisAlignedBoundingBox2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else if (this.GetComponent<ObjectBoundingBox2D>().TestCollisionVsOBB(OBB.GetComponent<ObjectBoundingBox2D>(), ref tempCollision))
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        }
        //        else
        //        {
        //            this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        //        }
        //    }
        //}

    
}
