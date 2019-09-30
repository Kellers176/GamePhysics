using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBox2D : CollisionHull2D
{
    public ObjectBoundingBox2D() : base(CollisionHullType2D.hull_obb) { }

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

        float radius = other.radius;

        float minY = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY = this.transform.position.y + (this.transform.localScale.y * 0.5f);

        float minX = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX = this.transform.position.x + (this.transform.localScale.x * 0.5f);


        //do stuff
        //multiply by the inverse matrix

        //Vector3 position;
        //position = other.transform.worldToLocalMatrix.MultiplyVector(this.transform.position);

        Vector2 rotationA = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
        Vector2 rotationB = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

        float newCenterX = (rotationA.x * other.transform.position.x) + (rotationA.y * other.transform.position.y);
        float newCenterY = (rotationB.x * other.transform.position.x) + (rotationB.y * other.transform.position.y);

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
        //done
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

        float minY1 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY1 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX1 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX1 = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float minY2 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY2 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX2 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX2 = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        Vector2 maxBox0 = new Vector2(maxX1, maxY1);
        Vector2 minBox0 = new Vector2(minX1, minY1);

        Vector2 maxBox1 = new Vector2(maxX2, maxY2);
        Vector2 minBox1 = new Vector2(minX2, minY2);

        if (Vector2Compare(maxBox0, minBox1) && Vector2Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

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
    bool Vector2Compare(Vector2 box1, Vector2 box2)
    {
        if (box1.x >= box2.x && box1.y >= box2.y)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {
        // AABB-OBB part 2 twice
        // 1. .....
        float minY1 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY1 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX1 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX1 = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float minY2 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY2 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX2 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX2 = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        Vector2 maxBox0 = new Vector2(maxX1, maxY1);
        Vector2 minBox0 = new Vector2(minX1, minY1);

        Vector2 maxBox1 = new Vector2(maxX2, maxY2);
        Vector2 minBox1 = new Vector2(minX2, minY2);

        if (Vector2Compare(maxBox0, minBox1) && Vector2Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

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
                Vector2 rotationC = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
                Vector2 rotationD = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

                //multiply by max extents
                //matrix

                //Vector2 newCenter = new Vector2(newCenterX, newCenterY);
                float newMaxBox00X = (rotationC.x * newMaxBox0.x) + (rotationC.y * newMaxBox0.y);
                float newMaxBox00Y = (rotationD.x * newMaxBox0.x) + (rotationD.y * newMaxBox0.y);
                float newMaxBox11X = (rotationC.x * newMaxBox1.x) + (rotationC.y * newMaxBox1.y);
                float newMaxBox11Y = (rotationD.x * newMaxBox1.x) + (rotationD.y * newMaxBox1.y);

                Vector2 newMaxBox00 = new Vector2(newMaxBox00X, newMaxBox00Y);
                Vector2 newMaxBox11 = new Vector2(newMaxBox11X, newMaxBox11Y);

                if (Vector2Compare(newMaxBox00, minBox1) && Vector2Compare(newMaxBox11, minBox0))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
