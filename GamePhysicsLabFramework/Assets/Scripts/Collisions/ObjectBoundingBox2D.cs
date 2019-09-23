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
        // see circle

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBox2D other)
    {
        // see AABB
        // 1. .....


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {
        // AABB-OBB part 2 twice
        // 1. .....

        return false;
    }
}
