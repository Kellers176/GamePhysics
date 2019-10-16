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


            public Vector3 velocity;
        }

        public CollisionHull2D a = null, b = null;
        //contact points on an object
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;


        protected Particle2D particle;
        public Collision col;
        Vector2 closingVelocity;

        // Find if the two objects have collided
        // Generate normal and get velocity from object that collided if true

        // Resolves the collision between the contacts
        public void resolveCollision(Contact contactHit)
        {
            // Calculate closing Velocity between the two particles that contacted
            closingVelocity = (a.particle.velocity - b.particle.velocity) * contactHit.normal;

            // Objects aren't moving
            if (closingVelocity.x >= 0 && closingVelocity.y >= 0)
            {
                return;
            }

            // Calculate the delta Velocity with restitution coefficient
            Vector2 deltaVelocity = (-closingVelocity * contactHit.restitution) - closingVelocity;

            // Getting the total inverse mass of both particles
            float totalIMass = a.particle.GetInvMass() + b.particle.GetInvMass();
            if (totalIMass <= 0)
            {
                return;
            }

            // Calculate impulse and impulse per inverse mass
            Vector2 impulse = deltaVelocity / totalIMass;
            Vector2 impulsePerIMass = contactHit.normal * impulse;


            // Set the new velocity of particle a and the new velocity of particle b if b exists
                a.particle.SetVelocity(new Vector2(a.particle.velocity.x * impulsePerIMass.x * a.particle.GetInvMass(),
                                               a.particle.velocity.y * impulsePerIMass.y * a.particle.GetInvMass()));
            if (b.particle != null)
            {
                    b.particle.SetVelocity(new Vector2(b.particle.velocity.x * impulsePerIMass.x * b.particle.GetInvMass(),
                                                   b.particle.velocity.y * impulsePerIMass.y * b.particle.GetInvMass()));
                
            }
            contactCount = 0;

            resolveInterpenetration(contactHit);
        }

        // Resolve every contact that is occuring
        public void resolveContact()
        {
            // If there are more than 0 contacts, resolve them
            if (contactCount != 0)
            {
                for (int i = 0; i < contactCount; i++)
                {
                    resolveCollision(contact[i]);
                }
            }
        }

        public void resolveInterpenetration(Contact contactHit)
        {
            Vector2[] movement = new Vector2[2];

            if(contactHit.collisionDepth <= 0)
            {
                return;
            }

            float totalIMass = a.particle.GetInvMass() + b.particle.GetInvMass();
            if (totalIMass <= 0)
            {
                return;
            }

            Vector2 movementPerIMass = contactHit.normal * (contactHit.collisionDepth / totalIMass);
            movement[0] = movementPerIMass * a.particle.GetInvMass();
            movement[1] = movementPerIMass * -b.particle.GetInvMass();


            //a.particle.position.x = (a.transform.position.x + movement[0].x);
            //a.particle.position.y = (a.transform.position.y + movement[0].y);
            //b.particle.position.x = (a.transform.position.x + movement[1].x);
            //b.particle.position.y = (a.transform.position.y + movement[1].y);

            a.particle.SetPosition(new Vector2(a.transform.position.x + movement[0].x, a.transform.position.y + movement[0].y));
            b.particle.SetPosition(new Vector2(b.transform.position.x + movement[1].x, b.transform.position.y + movement[1].y));
        }

        
        // Keep the most important contacts first
        public void orderContacts()
        {
            // If contact exists
            if (contact != null)
            {
                Contact temp;
                for (int i = 0; i < contactCount - 1; i++)
                {
                    Vector2 currentClosingVel = (a.particle.velocity - b.particle.velocity) * contact[i].normal;
                    Vector2 nextClosingVel = (a.particle.velocity - b.particle.velocity) * contact[i + 1].normal;
                    if (currentClosingVel.magnitude > nextClosingVel.magnitude)
                    {
                        // Sort up
                        temp = contact[i];
                        contact[i] = contact[i + 1];
                        contact[i + 1] = temp;
                        i = -1;
                    }
                }

                resolveContact();
            }
        }
        public void setParticleInfo(Particle2D collision)
        {
            particle = collision;
            col = new Collision();
        }
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
    public Collision col;
    public bool isColliding = false;
    [SerializeField]
    public float restitutionCoeff = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
       //particle = GetComponent<Particle2D>();
       //col = new Collision();
    }

    public void setParticleInfo()
    {
        particle = GetComponent<Particle2D>();
        col = new Collision();
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
