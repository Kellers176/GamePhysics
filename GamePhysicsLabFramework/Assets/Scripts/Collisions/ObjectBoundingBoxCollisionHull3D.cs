using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull3D : CollisionHull3D
{
    public ObjectBoundingBoxCollisionHull3D() : base(CollisionHullType3D.hull_obb) { }
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
        float radius = other.radius;

        float minY = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY = this.transform.position.y + (this.transform.localScale.y * 0.5f);

        float minX = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX = this.transform.position.x + (this.transform.localScale.x * 0.5f);

        float minZ = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        float nearestX = Mathf.Clamp(this.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(this.transform.position.y, minY, maxY);
        float nearestZ = Mathf.Clamp(this.transform.position.z, minZ, maxZ);

        //do stuff
        //multiply by the inverse matrix

        Vector3 position;


        position = this.transform.worldToLocalMatrix.MultiplyVector(other.transform.position);


        //check whether point is within circle
        if (TestPoint3D(radius, nearestX, nearestY, nearestZ, this.transform.position))
        {
            return true;
        }

        return false;
        
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollisionHull3D other)
    {
        //box 1
        float minY2 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY2 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX2 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX2 = this.transform.position.x + (this.transform.localScale.x * 0.5f);
        float minZ2 = this.transform.position.z - (this.transform.localScale.z * 0.5f);
        float maxZ2 = this.transform.position.z + (this.transform.localScale.z * 0.5f);

        //box2
        float minY1 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY1 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX1 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX1 = other.transform.position.x + (other.transform.localScale.x * 0.5f);
        float minZ1 = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ1 = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        Vector3 maxBox0 = new Vector3(maxX1, maxY1, maxZ1);
        Vector3 minBox0 = new Vector3(minX1, minY1, minZ1);

        Vector3 maxBox1 = new Vector3(maxX2, maxY2, maxZ2);
        Vector3 minBox1 = new Vector3(minX2, minY2, minZ2);

        if (Vector3Compare(maxBox0, minBox1) && Vector3Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

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

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other)
    {
        //box 1
        float minY2 = this.transform.position.y - (this.transform.localScale.y * 0.5f);
        float maxY2 = this.transform.position.y + (this.transform.localScale.y * 0.5f);
        float minX2 = this.transform.position.x - (this.transform.localScale.x * 0.5f);
        float maxX2 = this.transform.position.x + (this.transform.localScale.x * 0.5f);
        float minZ2 = this.transform.position.z - (this.transform.localScale.z * 0.5f);
        float maxZ2 = this.transform.position.z + (this.transform.localScale.z * 0.5f);

        //box2
        float minY1 = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY1 = other.transform.position.y + (other.transform.localScale.y * 0.5f);
        float minX1 = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX1 = other.transform.position.x + (other.transform.localScale.x * 0.5f);
        float minZ1 = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ1 = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        Vector3 maxBox0 = new Vector3(maxX1, maxY1, maxZ1);
        Vector3 minBox0 = new Vector3(minX1, minY1, minZ1);

        Vector3 maxBox1 = new Vector3(maxX2, maxY2, maxZ2);
        Vector3 minBox1 = new Vector3(minX2, minY2, minZ2);

        if (Vector3Compare(maxBox0, minBox1) && Vector3Compare(maxBox1, minBox0))
        {
            Vector2 rotationA = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
            Vector2 rotationB = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

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
                Vector2 rotationC = new Vector2(Mathf.Cos(-(this.transform.eulerAngles.y)), -Mathf.Sin(-(this.transform.eulerAngles.y)));
                Vector2 rotationD = new Vector2(Mathf.Sin(-(this.transform.eulerAngles.y)), Mathf.Cos(-(this.transform.eulerAngles.y)));

                float newMaxBox00X = (rotationC.x * newMaxBox0.x) + (rotationC.y * newMaxBox0.y);
                float newMaxBox00Y = (rotationD.x * newMaxBox0.x) + (rotationD.y * newMaxBox0.y);
                float newMaxBox11X = (rotationC.x * newMaxBox1.x) + (rotationC.y * newMaxBox1.y);
                float newMaxBox11Y = (rotationD.x * newMaxBox1.x) + (rotationD.y * newMaxBox1.y);

                Vector2 newMaxBox00 = new Vector2(newMaxBox00X, newMaxBox00Y);
                Vector2 newMaxBox11 = new Vector2(newMaxBox11X, newMaxBox11Y);

                if (Vector3Compare(newMaxBox00, minBox1) && Vector3Compare(newMaxBox11, minBox0))
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

    bool TestPoint3D(float radius, float nearX, float nearY, float nearZ, Vector3 centerCircle)
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