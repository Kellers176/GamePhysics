using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{

    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    [Range (0.0f, 100.0f)]
    public float radius;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // pass if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) sq
        // 1. get the two centers
        // 2. difference between centers
        // 3. distance squared = dot(diff, diff) (Vector.squaremagnitude)
        // 4. sum of radii
        // 5. square sum
        // 6. DO THE TEST: distSq <= sumSq

        Vector2 collisionCenter = other.transform.position;
        Vector2 currentCenter = this.transform.position;

        Vector2 difference = collisionCenter - currentCenter;

        //I am confused
        float distanceSquared = Vector2.SqrMagnitude(difference);

        float sumOfRadii = other.radius + this.radius;

        float sumSquared = sumOfRadii * sumOfRadii;

        if(distanceSquared <= sumSquared)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBox2D other)
    {
        // calculate closest point by clamping circle center on each dimension
        // passes if closest point vs circle passes
        // 1. get the radius of the circle
        // 2. get the ymin, ymax
        // 3. get the xmin, xmax
        // 4. max0 >= radius
        // 5. radius >= min0

        float radius = this.radius;

        float minY = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY = other.transform.position.y + (other.transform.localScale.y * 0.5f);

        float minX = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float nearestX = this.transform.position.x * Mathf.Clamp(radius, minX, maxX);
        float nearestY = this.transform.position.y * Mathf.Clamp(radius, minY, maxY);

        if (nearestX >= radius && radius >= nearestY)
        {

        }





        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {
        // same as above, but first....
        // transform circle position by multiplying by box world matrix inverse
        // 1. .....

        return false;
    }
}
