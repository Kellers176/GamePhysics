using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxCollisionHull3D : CollisionHull3D
{
    public AxisAlignedBoundingBoxCollisionHull3D() : base(CollisionHullType3D.hull_aabb) { }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool TestCollisionVsSphere(SphereCollisionHull3D other)
    {
        // calculate closest point by clamping circle center on each dimension
        // passes if closest point vs circle passes
        // 1. get the radius of the circle
        // 2. get the ymin, ymax
        // 3. get the xmin, xmax
        // 4. clamp the circle to the box so that we are able to get proper collision
        // 5. max0 >= radius
        // 6. radius >= min0
        //done
        float radius = other.radius;

        float minY = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY = this.transform.position.y + (this.transform.localScale.y * 0.5f);

        float minX = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        float minZ = this.transform.position.z - (this.transform.localScale.z * 0.5f);
        float maxZ = this.transform.position.z + (this.transform.localScale.z * 0.5f);

        float nearestX = Mathf.Clamp(other.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(other.transform.position.y, minY, maxY);
        float nearestZ = Mathf.Clamp(other.transform.position.z, minZ, maxZ);


        //check whether point is within circle
        if (TestPoint(radius, nearestX, nearestY, nearestZ, other.transform.position))
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollisionHull3D other)
    {
        //box 1
        float minY1 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY1 = this.transform.position.y + (this.transform.localScale.y * 0.5f);        
        float minX1 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX1 = this.transform.position.x + (this.transform.localScale.x * 0.5f);  
        float minZ1 = this.transform.position.z - (this.transform.localScale.z * 0.5f);
        float maxZ1 = this.transform.position.z + (this.transform.localScale.z * 0.5f);

        //box2
        float minY2 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY2 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX2 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX2 = other.transform.position.x + (other.transform.localScale.x * 0.5f);
        float minZ2 = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ2 = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        Vector3 maxBox0 = new Vector3(maxX1, maxY1, maxZ1);
        Vector3 minBox0 = new Vector3(minX1, minY1, minZ1);

        Vector3 maxBox1 = new Vector3(maxX2, maxY2, maxZ2);
        Vector3 minBox1 = new Vector3(minX2, minY2, minZ2);

        if (Vector3Compare(maxBox0, minBox1) && Vector3Compare(maxBox1, minBox0))
        {
            return true;
        }


        return false;

    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other)
    {
        //box 1
        float minY1 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY1 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX1 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX1 = this.transform.position.x + (this.transform.localScale.x * 0.5f);
        float minZ1 = this.transform.position.z - (this.transform.localScale.z * 0.5f);
        float maxZ1 = this.transform.position.z + (this.transform.localScale.z * 0.5f);

        //box2
        float minY2 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY2 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX2 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX2 = other.transform.position.x + (other.transform.localScale.x * 0.5f);
        float minZ2 = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ2 = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        Vector3 maxBox0 = new Vector3(maxX1, maxY1, maxZ1);
        Vector3 minBox0 = new Vector3(minX1, minY1, minZ1);

        Vector3 maxBox1 = new Vector3(maxX2, maxY2, maxZ2);
        Vector3 minBox1 = new Vector3(minX2, minY2, minZ2);

        if (Vector3Compare(maxBox0, minBox1) && Vector3Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(other.transform.eulerAngles.y)), -Mathf.Sin(-(other.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(other.transform.eulerAngles.y)), Mathf.Cos(-(other.transform.eulerAngles.y)));

            //     |1           0           0       |
            //Qx = |0       cos(theta)   -sin(theta)|
            //     |0       sin(theta)   cos(theta) |

            //     |cos(theta)      0        sin(theta) |
            //Qy = |0               1           0       |
            //     |-sin(theta)     0        cos(theta) |

            //     |cos(theta)      -sin(theta)     0 |
            //Qz = |sin(theta)      cos(theta)      0 |
            //     |0                   0           1 |

            //multiply by max extents
            //matrix

            //Vector2 newCenter = new Vector2(newCenterX, newCenterY);
            float newMaxBox0X = (rotationA.x * maxBox0.x) + (rotationA.y * maxBox0.y);
            float newMaxBox0Y = (rotationB.x * maxBox0.x) + (rotationB.y * maxBox0.y);
            float newMaxBox1X = (rotationA.x * maxBox1.x) + (rotationA.y * maxBox1.y);
            float newMaxBox1Y = (rotationB.x * maxBox1.x) + (rotationB.y * maxBox1.y);
            //max box z
            Vector2 newMaxBox0 = new Vector2(newMaxBox0X, newMaxBox0Y);
            Vector2 newMaxBox1 = new Vector2(newMaxBox1X, newMaxBox1Y);

            if (Vector3Compare(newMaxBox0, minBox1) && Vector3Compare(newMaxBox1, minBox0))
            {
                return true;
            }
        }
        return false;
    }
    bool Vector3Compare(Vector3 box1, Vector3 box2)
    {
        if (box1.x >= box2.x && box1.y >= box2.y && box1.z >= box2.z)
        {
            return true;
        }

        return false;
    }
    bool TestPoint(float radius, float nearX, float nearY, float nearZ, Vector3 centerCircle)
    {
        //(xp,yp) = (nearX, nearY)
        // (xc,yc) = (circleCenter.x, circleCenter.y)
        float distance = Mathf.Sqrt(((nearX - centerCircle.x) * (nearX - centerCircle.x)) + ((nearY - centerCircle.y) * (nearY - centerCircle.y)) + ((nearZ - centerCircle.z) * (nearZ - centerCircle.z)));

        if (distance < radius)
        {
            return true;
        }

        return false;
    }
}
