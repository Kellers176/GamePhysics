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
                    if(this.gameObject.tag == "ship")
                        this.GetComponent<SpaceShipManager>().playerHealth -= 0.01f;
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
