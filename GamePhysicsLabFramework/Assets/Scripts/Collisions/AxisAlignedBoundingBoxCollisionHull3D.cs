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


        //we need to check for depth;
        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollisionHull3D other)
    {
        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other)
    {
        return false;
    }
}
