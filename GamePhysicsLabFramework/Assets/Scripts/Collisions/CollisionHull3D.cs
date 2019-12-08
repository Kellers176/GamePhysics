using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    const int PLAYER_MASS = 5;

    public class Collision
    {
        public struct Contact
        {
            public Vector3 point;
            public Vector3 normal;
            public float restitution;
            public Vector3 collisionDepth;
        }

        public CollisionHull3D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool collisionStatus = false;


        protected Particle3D particle;
        public Collision col;
        Vector3 closingVelocity;


        public Vector3 calculateSeperatingVelocity()
        {
            //stuff
            Vector3 relativeVelocity = a.particle.velocity;
            if(b)
            {
                relativeVelocity -= b.particle.velocity;

            }

            return new Vector3(relativeVelocity.x * contact[0].normal.x, relativeVelocity.y * contact[0].normal.y, relativeVelocity.z * contact[0].normal.z);
        }

        public void resolveVelocity(float duration)
        {
            Vector3 seperatingVelocity = calculateSeperatingVelocity();

            if(seperatingVelocity.x > 0 && seperatingVelocity.y > 0 && seperatingVelocity.z > 0)
            {
                return;
            }

            Vector3 newSepVelocity = -seperatingVelocity * contact[0].restitution;

            Vector3 deltaVelocity = newSepVelocity - seperatingVelocity;


            float totalInverseMass = a.particle.GetInvMass();

            if (b)
            {
                totalInverseMass += a.particle.GetInvMass();
            }
            if(totalInverseMass <= 0)
            {
                return;
            }

            Vector3 impulse = deltaVelocity / totalInverseMass;

            Vector3 impulsePerIMass = new Vector3(contact[0].normal.x * impulse.x, contact[0].normal.y * impulse.y, contact[0].normal.z * impulse.z);

            a.particle.SetVelocity(a.particle.velocity + impulsePerIMass * a.particle.GetInvMass());

            if(b)
            {
                b.particle.SetVelocity(b.particle.velocity + impulsePerIMass * -b.particle.GetInvMass());
            }

        }




        public void resolveCollision(Contact contactHit)
        {
            // Send Player Ball in direction of contactHit's normal at same or slightly less velocity
            //closingVelocity = a.particle.velocity - b.particle.velocity;
            closingVelocity = a.particle.velocity;
            // Lower velocity for less strange occurences
            closingVelocity *= 0.5f;

            // check if particle A is the player ball using mass
            // send particle A away in direction of normal at closingVelocity
            if (a.particle.GetMass() == PLAYER_MASS)
            {
                Vector3 desiredDirection = contactHit.normal;
                Vector3 newVel = desiredDirection.normalized * closingVelocity.magnitude;
                a.particle.SetVelocity(newVel);
            }
            resolveInterpenetration(contactHit);
        }

        public void resolveContact()
        {
            if (contactCount > 0)
            {
                for (int i = 0; i < contactCount; i++)
                {
                    resolveCollision(contact[i]);
                }
            }
        }

        public void resolveInterpenetration(Contact contactHit)
        {
            if (contactHit.collisionDepth.x <= 0 &&
                contactHit.collisionDepth.y <= 0 &&
                contactHit.collisionDepth.z <= 0)
            {
                return;
            }
            else
            {
                // if is Player Ball
                // if a.particle.mass = PlayerBallMass
                // else if b.particle.mass = PlayerBallMass
                // This ensures that the walls do not move when hit by the player
                if (a.particle.GetMass() == PLAYER_MASS)
                {
                    a.particle.SetPositionX(a.transform.position.x - contactHit.collisionDepth.x);
                    a.particle.SetPositionY(a.transform.position.y - contactHit.collisionDepth.y);
                    a.particle.SetPositionZ(a.transform.position.z - contactHit.collisionDepth.z);
                }
                else if (b.particle.GetMass() == PLAYER_MASS)
                {
                    b.particle.SetPositionX(b.transform.position.x - contactHit.collisionDepth.x);
                    b.particle.SetPositionY(b.transform.position.y - contactHit.collisionDepth.y);
                    b.particle.SetPositionZ(b.transform.position.z - contactHit.collisionDepth.z);
                }
            }
        }
        public void orderContacts()
        {
            // If contact exists
            if (contact != null)
            {
                resolveContact();
            }
        }
        public void setParticleInfo(Particle3D collision)
        {
            particle = collision;
            col = new Collision();
        }
    }

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
    public Collision col;

    public void setParticleInfo()
    {
        particle = GetComponent<Particle3D>();
        col = new Collision();
    }


    // Start is called before the first frame update
    void Start()
    {
        //particle = GetComponent<Particle3D>();
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
