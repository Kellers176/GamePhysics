using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBox2D : CollisionHull2D
{
    public AxisAlignedBoundingBox2D() : base(CollisionHullType2D.hull_aabb) { }

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
        // calculate closest point by clamping circle center on each dimension
        // passes if closest point vs circle passes
        // 1. get the radius of the circle
        // 2. get the ymin, ymax
        // 3. get the xmin, xmax
        // 4. clamp the circle to the box so that we are able to get proper collision
        // 5. max0 >= radius
        // 6. radius >= min0

        float radius = other.radius;

        float minY = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY = this.transform.position.y + (this.transform.localScale.y * 0.5f);

        float minX = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        float nearestX = Mathf.Clamp(other.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(other.transform.position.y, minY, maxY);

        if (TestPoint(radius, nearestX, nearestY, other.transform.position))
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

        if (distance < radius)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBox2D other)
    {
        // for each dimension, max extent of A greater than min extent of B
        // 1. get the ymin, ymax of both boxes
        // 2. get the xmin, xmax of both boxes
        // 3. max of box 1 >= min of box 2
        // 4. max of box 2 >= min of box 1

        float minY1 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY1 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX1 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX1 = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        float minY2 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY2 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX2 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX2 = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float nearestX1 = Mathf.Clamp(position.x, minX1, maxX1);
        float nearestY1 = Mathf.Clamp(position.y, minY1, maxY1);
        float nearestX2 = Mathf.Clamp(other.position.x, minX2, maxX2);
        float nearestY2 = Mathf.Clamp(other.position.y, minY2, maxY2);

        float distance = Mathf.Sqrt(((nearestX1 - nearestX2) * (nearestX1 - nearestX2)) + ((nearestY1 - nearestY2) * (nearestY1 - nearestY2)));
        float sumOfDistancesX = this.transform.position.x + other.transform.position.x;
        float sumOfDistancesY = this.transform.position.y + other.transform.position.y;

        if (distance <= sumOfDistancesX && distance <= sumOfDistancesY)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {
        // same as above twice
        // first, test AABB vs max extents
        // then, multiply by OBB inverse matrix, do test again
        // 1. get the ymin, ymax, xmin, xmax of both boxes
        // 2. max of box 1 >= min of box 2
        // 3. max of box 2 >= min of box 1
        // 4. create OBB inverse matrix
        // 5. multiply inverse matrix by max extents
        // 6. max of box 1 >= min of box 2
        // 7. max of box 2 >= min of box 1

        float minY1 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY1 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX1 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX1 = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        float minY2 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY2 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX2 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX2 = other.transform.position.x + (other.transform.localScale.x * 0.5f);



        return false;
    }
}
