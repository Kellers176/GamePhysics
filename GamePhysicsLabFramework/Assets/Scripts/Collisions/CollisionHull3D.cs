using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    public enum CollisionHullType3D
    {
        hull_sphere,
        hull_aabb,
        hull_obb,
    }
    private CollisionHullType3D type { get; }

    protected CollisionHull3D(CollisionHullType3D type_set)
    {
        type = type_set;
    }

    protected Particle3D particle;


    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool TestCollision(CollisionHull3D a, CollisionHull3D b)
    {
        return false;
    }


    public abstract bool TestCollisionVsSphere(SphereCollisionHull3D other);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollisionHull3D other);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other);
}
