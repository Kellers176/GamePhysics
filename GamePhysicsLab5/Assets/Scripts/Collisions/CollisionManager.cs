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

    float elapsedTime;
    float delay = 3;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        elapsedTime++;
        //player.GetComponent<AxisAlignedBoundingBox2D>().TestCollisionVsAABB(circle.GetComponent<AxisAlignedBoundingBox2D>());
        CollisionHull2D.Collision tempCollision = new CollisionHull2D.Collision();

        for(int i = 0; i < circle.Length; i++)
        {
            for (int j = i + 1; j < circle.Length; j++)
            {
                if (this.GetComponent<CircleCollisionHull2D>().TestCollisionVsCircle(circle[j].GetComponent<CircleCollisionHull2D>(), ref circle[i].GetComponent<CircleCollisionHull2D>().col))
                {
                    if(this.gameObject.tag == "ship" && elapsedTime > delay)
                    {
                        this.GetComponent<SpaceShipManager>().playerHealth -= 0.01f;
                        elapsedTime = 0;
                    }
                    colliding = true;
                }   
            }

        }

        for(int i = 0; i < circle.Length; i++)
        {
            tempCollision = circle[i].GetComponent<CircleCollisionHull2D>().col;
            if(colliding)
            {
                circle[i].GetComponent<CircleCollisionHull2D>().col.orderContacts();
            }
            colliding = false;
        }

        }


    
}
