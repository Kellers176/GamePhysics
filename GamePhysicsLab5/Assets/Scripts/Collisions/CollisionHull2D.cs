using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            public Vector2 point;
            public Vector2 normal;
            public float restitution;
            public float collisionDepth;
        }

        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        Vector2 closingVelocity;
    }


    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb,
    }
    private CollisionHullType2D type { get; }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    protected Particle2D particle;


    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        return false;
    }


    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBox2D other, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBox2D other, ref Collision c);

}
