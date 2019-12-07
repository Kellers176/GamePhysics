using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionHull3D : CollisionHull3D
{

    public SphereCollisionHull3D() : base(CollisionHullType3D.hull_sphere) { }


    [Range(0.0f, 100.0f)]
    public float radius;



    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TestCollisionVsSphere(SphereCollisionHull3D other)
    {
        //done
        Vector3 collisionCenter = other.transform.position;
        Vector3 currentCenter = this.transform.position;

        Vector3 difference = collisionCenter - currentCenter;

        //I am confused
        float distanceSquared = Vector3.SqrMagnitude(difference);

        float sumOfRadii = other.radius + this.radius;

        float sumSquared = sumOfRadii * sumOfRadii;

        if (distanceSquared <= sumSquared)
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollisionHull3D other)
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
        float radius = this.radius;

        float minY = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY = other.transform.position.y + (other.transform.localScale.y * 0.5f);

        float minX = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float minZ = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        float nearestX = Mathf.Clamp(this.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(this.transform.position.y, minY, maxY);
        float nearestZ = Mathf.Clamp(this.transform.position.z, minZ, maxZ);


        //check whether point is within circle
        if (TestPoint(radius, nearestX, nearestY, nearestZ, this.transform.position))
        {
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other)
    {
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
        //do we need world position?

        //done
        float radius = this.radius;

        float minY = other.transform.position.y - (other.transform.localScale.y * 0.5f);
        float maxY = other.transform.position.y + (other.transform.localScale.y * 0.5f);

        float minX = other.transform.position.x - (other.transform.localScale.x * 0.5f);
        float maxX = other.transform.position.x + (other.transform.localScale.x * 0.5f);

        float minZ = other.transform.position.z - (other.transform.localScale.z * 0.5f);
        float maxZ = other.transform.position.z + (other.transform.localScale.z * 0.5f);

        float nearestX = Mathf.Clamp(this.transform.position.x, minX, maxX);
        float nearestY = Mathf.Clamp(this.transform.position.y, minY, maxY);
        float nearestZ = Mathf.Clamp(this.transform.position.z, minZ, maxZ);
        //do stuff
        //multiply by the inverse matrix

        Vector3 position;
        //position = other.GetComponent<Particle3D>().worldTransformationMatrix.MultiplyVector(this.transform.position);
        //position = other.transform.worldToLocalMatrix.MultiplyVector(this.transform.position);
        position = other.GetComponent<Particle3D>().worldTransformationMatrix.MultiplyVector(this.transform.position);


        //check whether point is within circle
        if (TestPoint(radius, nearestX, nearestY, nearestZ, position))
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
