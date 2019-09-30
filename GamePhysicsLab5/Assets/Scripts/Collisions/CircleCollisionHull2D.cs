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
        //works
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
        //works
        // calculate closest point by clamping circle center on each dimension
        // passes if closest point vs circle passes
        // 1. get the radius of the circle
        // 2. get the ymin, ymax
        // 3. get the xmin, xmax
        // 4. clamp the circle to the box so that we are able to get proper collision
        // 5. max0 >= radius
        // 6. radius >= min0

        float radius = this.radius;

        float minY = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY = other.transform.position.y + (other.transform.localScale.y * 0.5f);

        float minX = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float nearestX = Mathf.Clamp(this.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(this.transform.position.y, minY, maxY);


        //check whether point is within circle
        if (TestPoint(radius, nearestX, nearestY, this.transform.position)) 
        {
            return true;
        }

        return false;
    }

    bool TestPoint(float radius, float nearX, float nearY, Vector2 centerCircle)
    {
        //(xp,yp) = (nearX, nearY)
        // (xc,yc) = (circleCenter.x, circleCenter.y)
        float distance = Mathf.Sqrt(((nearX - centerCircle.x) * (nearX - centerCircle.x)) + ((nearY - centerCircle.y) * (nearY - centerCircle.y)));

        if(distance < radius)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {

        //done
        // same as above, but first....
        // transform circle position by multiplying by box world matrix inverse
        // 1. get the radius of the circle
        // get the min and max of the box
        // create matrix for r and u (normal calculation)
        // | cos(theta)     sin(theta) |
        // |-sin(theta)    cos(theta)  |
        // r = (+cos(theta), +sin(theta))
        // u = (-sin(theta),  cos(theta))
        // Step1: Project all vertices onto normal
        // p^1 (box) = (p(circle) * normal) * normal
        // Step 2: AABB test
        //repeat both steps for each of the normals

        float radius = this.radius;

        float minY = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY = other.transform.position.y + (other.transform.localScale.y * 0.5f);

        float minX = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX = other.transform.position.x + (other.transform.localScale.x * 0.5f);


        //do stuff
        //multiply by the inverse matrix

        //Vector3 position;
        //position = other.transform.worldToLocalMatrix.MultiplyVector(this.transform.position);

        Vector2 rotationA = new Vector2(Mathf.Cos(-(other.transform.eulerAngles.y)), -Mathf.Sin(-(other.transform.eulerAngles.y)));
        Vector2 rotationB = new Vector2(Mathf.Sin(-(other.transform.eulerAngles.y)), Mathf.Cos(-(other.transform.eulerAngles.y)));

        float newCenterX = (rotationA.x * this.transform.position.x) + (rotationA.y * this.transform.position.y);
        float newCenterY = (rotationB.x * this.transform.position.x) + (rotationB.y * this.transform.position.y);

        Vector2 newCenter = new Vector2(newCenterX, newCenterY);

        float nearestX = Mathf.Clamp(newCenter.x, minX, maxX);
        float nearestY = Mathf.Clamp(newCenter.y, minY, maxY);


        //check whether point is within circle
        if (TestPoint(radius, nearestX, nearestY, newCenter))
        {
            return true;
        }

        return false;


    }
}
