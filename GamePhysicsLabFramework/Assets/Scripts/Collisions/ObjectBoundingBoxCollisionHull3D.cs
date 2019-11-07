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
