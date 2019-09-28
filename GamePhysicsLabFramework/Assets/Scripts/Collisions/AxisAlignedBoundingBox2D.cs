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
        // pass if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) sq
        // 1. get the two centers
        // 2. difference between centers
        // 3. distance squared = dot(diff, diff) (Vector.squaremagnitude)
        // 4. sum of radii
        // 5. square sum
        // 6. DO THE TEST: distSq <= sumSq

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBox2D other)
    {
        // for each dimension, max extent of A greater than min extent of B
        // 1. .....


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBox2D other)
    {
        // same as above twice
        // first, test AABB vs max extents
        //then, multiply by OBB inverse matrix, do text again
        // 1. .....

        return false;
    }
}
