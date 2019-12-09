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
            public float penetration;
        }

        public CollisionHull3D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 1;
        public bool collisionStatus = false;

        //float penetration = 0;

        protected Particle3D particle;
        public Collision col;
        Vector3 closingVelocity;

        float iterations = 3;
        float iterationsUsed = 0;


        public void setCollisionStatus(bool colStatus)
        {
            collisionStatus = colStatus;
        }

        public Vector3 calculateSeperatingVelocity(Contact contactHit)
        {
            //stuff
            Vector3 relativeVelocity = a.particle.velocity;
            

            return new Vector3(relativeVelocity.x * contact[0].normal.x, relativeVelocity.y * contact[0].normal.y, relativeVelocity.z * contact[0].normal.z);
        }

        public void resolveVelocity(Contact contactHit)
        {
            Vector3 seperatingVelocity = calculateSeperatingVelocity(contactHit);

            if (seperatingVelocity.x > 0 && seperatingVelocity.y > 0 && seperatingVelocity.z > 0)
            {
                return;
            }

            Vector3 newSepVelocity = -seperatingVelocity * contact[0].restitution;

            //accelerationnnn
            Vector3 accCausedVelocity = a.particle.acceleration;
            

            Vector3 accCausedSepVelocity = new Vector3(accCausedVelocity.x * contact[0].normal.x, accCausedVelocity.y * contact[0].normal.y, accCausedVelocity.z * contact[0].normal.z) * Time.deltaTime;

            if(accCausedSepVelocity.x < 0 && accCausedSepVelocity.y < 0 && accCausedSepVelocity.z < 0)
            {
                newSepVelocity += contact[0].restitution * accCausedSepVelocity;

                if (newSepVelocity.x < 0 && newSepVelocity.y < 0&& newSepVelocity.z < 0)
                   newSepVelocity = Vector3.zero;

            }

            Vector3 deltaVelocity = newSepVelocity - seperatingVelocity;
            float totalInverseMass = a.particle.GetInvMass();

           
            if (totalInverseMass <= 0)
            {
                return;
            } 

            Vector3 impulse = deltaVelocity / totalInverseMass;

            Vector3 impulsePerIMass = new Vector3(contact[0].normal.x * impulse.x, contact[0].normal.y * impulse.y, contact[0].normal.z * impulse.z);

            a.particle.SetVelocity(a.particle.velocity + impulsePerIMass * a.particle.GetInvMass());


        }

        public void resolve(Contact contactHit)
        {
            resolveVelocity(contactHit);
            resolveInterpenetration(contactHit);
        }

        public void resolveInterpenetration(Contact contactHit)
        {
            if(contact[0].penetration < 0)
            {
                return;

            }
            float totalInverseMass = a.particle.GetInvMass();

            if(b)
            {
               totalInverseMass += b.particle.GetInvMass();
            }

            if(totalInverseMass <= 0)
            {
                return;
            }

            Vector3 movePerIMass = contact[0].normal * (-contact[0].penetration / totalInverseMass);

            a.particle.SetPosition(a.particle.position + movePerIMass * a.particle.GetInvMass());

            if(b)
            {
                //b.particle.SetPosition(b.particle.position + movePerIMass * b.particle.GetInvMass());
            }

        }

        void SetIterations(float iter)
        {
            iterations = iter;
        }

        public void resolveContacts()
        {
            if (contactCount > 0)
            {
                for (int i = 0; i < contactCount; i++)
                {
                    ResolveCollision(contact[i], contactCount);
                }
            }
        }

        void ResolveCollision(Contact contactHit, int numContacts)
        {
            iterationsUsed = 0;
            while(iterationsUsed < iterations)
            {
                Vector3 max = Vector3.zero;
                int maxIndex = numContacts;

                for(int i = 0; i < numContacts; i++)
                {
                    Vector3 sepVel = calculateSeperatingVelocity(contactHit);
                    if(sepVel.x <= max.x && sepVel.y <= max.y && sepVel.z <= max.z)
                    {
                        max = sepVel;
                        maxIndex = i;
                    }
                }
                resolve(contactHit);

                iterationsUsed++;
            }




        }




        //       public void resolveCollision(Contact contactHit)
        //       {
        //           // Send Player Ball in direction of contactHit's normal at same or slightly less velocity
        //           //closingVelocity = a.particle.velocity - b.particle.velocity;
        //           closingVelocity = a.particle.velocity;
        //           // Lower velocity for less strange occurences
        //           closingVelocity *= 0.5f;
        //
        //           // check if particle A is the player ball using mass
        //           // send particle A away in direction of normal at closingVelocity
        //           if (a.particle.GetMass() == PLAYER_MASS)
        //           {
        //               Vector3 desiredDirection = contactHit.normal;
        //               Vector3 newVel = desiredDirection.normalized * closingVelocity.magnitude;
        //               a.particle.SetVelocity(newVel);
        //           }
        //           resolveInterpenetration(contactHit);
        //       }
        //
        //       public void resolveContact()
        //       {
        //           if (contactCount > 0)
        //           {
        //               for (int i = 0; i < contactCount; i++)
        //               {
        //                   resolveCollision(contact[i]);
        //               }
        //           }
        //       }
        //
        //       public void resolveInterpenetration(Contact contactHit)
        //       {
        //           if (contactHit.collisionDepth.x <= 0 &&
        //               contactHit.collisionDepth.y <= 0 &&
        //               contactHit.collisionDepth.z <= 0)
        //           {
        //               return;
        //           }
        //           else
        //           {
        //               // if is Player Ball
        //               // if a.particle.mass = PlayerBallMass
        //               // else if b.particle.mass = PlayerBallMass
        //               // This ensures that the walls do not move when hit by the player
        //               if (a.particle.GetMass() == PLAYER_MASS)
        //               {
        //                   a.particle.SetPositionX(a.transform.position.x - contactHit.collisionDepth.x);
        //                   a.particle.SetPositionY(a.transform.position.y - contactHit.collisionDepth.y);
        //                   a.particle.SetPositionZ(a.transform.position.z - contactHit.collisionDepth.z);
        //               }
        //               else if (b.particle.GetMass() == PLAYER_MASS)
        //               {
        //                   b.particle.SetPositionX(b.transform.position.x - contactHit.collisionDepth.x);
        //                   b.particle.SetPositionY(b.transform.position.y - contactHit.collisionDepth.y);
        //                   b.particle.SetPositionZ(b.transform.position.z - contactHit.collisionDepth.z);
        //               }
        //           }
        //       }
        //       public void orderContacts()
        //       {
        //           // If contact exists
        //           if (contact != null)
        //           {
        //               resolveContact();
        //           }
        //       }
        //       public void setParticleInfo(Particle3D collision)
        //       {
        //           particle = collision;
        //           col = new Collision();
        //       }
        //   }
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
