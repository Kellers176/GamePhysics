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
        //works
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
        //works
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

        Vector2 maxBox0 = new Vector2(maxX1, maxY1);
        Vector2 minBox0 = new Vector2(minX1, minY1);

        Vector2 maxBox1 = new Vector2(maxX2, maxY2);
        Vector2 minBox1 = new Vector2(minX2, minY2);

        if(Vector2Compare(maxBox0, minBox1) && Vector2Compare(maxBox1, minBox0))
        {
            return true;
        }
        

        return false;
    }

    bool Vector2Compare(Vector2 box1, Vector2 box2)
    {
        if(box1.x >= box2.x && box1.y >= box2.y)
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

        Vector2 maxBox0 = new Vector2(maxX1, maxY1);
        Vector2 minBox0 = new Vector2(minX1, minY1);

        Vector2 maxBox1 = new Vector2(maxX2, maxY2);
        Vector2 minBox1 = new Vector2(minX2, minY2);

        if (Vector2Compare(maxBox0, minBox1) && Vector2Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(other.transform.eulerAngles.y)), -Mathf.Sin(-(other.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(other.transform.eulerAngles.y)), Mathf.Cos(-(other.transform.eulerAngles.y)));

            //multiply by max extents
            //matrix

            //Vector2 newCenter = new Vector2(newCenterX, newCenterY);
            float newMaxBox0X = (rotationA.x * maxBox0.x) + (rotationA.y * maxBox0.y);
            float newMaxBox0Y = (rotationB.x * maxBox0.x) + (rotationB.y * maxBox0.y);
            float newMaxBox1X = (rotationA.x * maxBox1.x) + (rotationA.y * maxBox1.y);
            float newMaxBox1Y = (rotationB.x * maxBox1.x) + (rotationB.y * maxBox1.y);

            Vector2 newMaxBox0 = new Vector2(newMaxBox0X, newMaxBox0Y);
            Vector2 newMaxBox1 = new Vector2(newMaxBox1X, newMaxBox1Y);

            if (Vector2Compare(newMaxBox0, minBox1) && Vector2Compare(newMaxBox1, minBox0))
            {
                return true;
            }
        }

        return false;

    }
}
