using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Step 1-1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    public float speed;

    // Step 2-1
    public float startingMass;
    float mass, massInv;
    Vector2 vectorReflect;
    public Vector2 particleVelocity;
    float frictionCoefficient;
    Vector2 fluidVelocity;
    float fluidDensity;
    float objectArea_crossSection;
    float objectDragCoefficient;
    Vector2 anchorPosition;
    Vector2 particlePosition;
    float springRestingLength;
    float springStiffnessCoefficient;

    //Step 3
    float inertia;
    public float inverseInertia;
    public float torque;
    Vector2 appliedForce;
    bool pressingLeft = false;
    bool pressingRight = false;

    public void SetMass(float newMass)
    {
        //mass = newMass > 0.0f ? newMass : 0.0f;
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;
    }
    public float GetMass()
    {
        return mass;
    }
    public void SetVelocity(Vector2 newVelocity)
    {
        particleVelocity = newVelocity;
    }
    public float GetInvMass()
    {
        return massInv;
    }

    // Step 2-2
    Vector2 force;
    public void AddForce(Vector2 newForce)
    {
        // D'Alembert
        force += newForce;
    }
    void UpdateAcceleration()
    {
        // Newton2
        acceleration = massInv * force;

        force.Set(0.0f, 0.0f);
    }

    // Step 1-2
    void updatePositionExplicitEuler(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's method:
        // F(t+dt) = F(t) + f(t)dt
        //                + (dF/dt)dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }
    void updatePositionKinematic(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt + 1/2a(t)dt^2
        position += velocity * dt + (0.5f * acceleration * (dt * dt));

        velocity += acceleration * dt;
    }
    void updateRotationEulerExplicit(float dt)
    {
        rotation += angularVelocity * dt;
        angularVelocity += angularAcceleration * dt;
    }
    void updateRotationKinematic(float dt)
    {
        rotation += angularVelocity * dt + (0.5f * angularAcceleration * (dt * dt));
        angularVelocity += angularAcceleration * dt;
    }

    //Step 3
    void calculateBoxInertia()
    {
        // I = 1/12m(dx^2 + dy^2)
        inertia = (1 / 12f) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
    }
    void calculateDiskInertia()
    {
        // I = 1/12m(dx^2 + dy^2)
        inertia = 0.5f * mass * ((transform.localScale.x * 0.5f) + (transform.localScale.x * 0.5f));
    }
    void calculateCubeInertia()
    {
        inertia = (1 / 6f) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
    }
    void updateAngularAcceleration()
    {
        //converts torque to angular acceleration
        //resets torque
        // T = IA -> A = I^-1 * T
        angularAcceleration = torque * inverseInertia;
        torque = 0;
    }

    void applyTorque(Vector2 force, Vector2 pointOfForce)
    {
        //applied torque: T = pf x F
        //T is torque, pf is moment arm (point of applied force relative to center of mass), F is applied force at pf. 
        //center of mass may not be the center of the object
        //might help to add a separate member for center of mass in local and world space
        //center of mass = 0;
        //world = local object * transform of object matrix
        //        localCenterOfMass = transform.localPosition;
        //        localCenterOfMass = new Vector2(transform.localPosition.x, transform.localPosition.y);
        //        worldCenterOfMass = transform.localToWorldMatrix.MultiplyVector(localCenterOfMass);
        //obj.x * force.y - obj.y * force.x
        Vector2 momentArm = pointOfForce - position;

        torque += (momentArm.x * force.y - momentArm.y * force.x);

        // += (force.x * (momentArm - new Vector2(0,0)).magnitude);
        //        torque += Vector3.Cross(worldCenterOfMass, appliedForce).y;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMass(startingMass);
        particleVelocity = new Vector2(0.5f, 0);
        frictionCoefficient = 0.5f;
        vectorReflect = new Vector2(-Mathf.Sqrt(3) * 0.5f, 0.5f);
        fluidVelocity = new Vector2(1, 0);
        fluidDensity = 1.0f;
        objectArea_crossSection = 3.0f;
        objectDragCoefficient = 0.5f;
        anchorPosition = new Vector2(0, 0);
        springRestingLength = 2.0f;
        springStiffnessCoefficient = 5.0f;

        speed = 500f;

        inertia = 0f;
        appliedForce = new Vector2(0.1f, 0.1f);
        //normal = cos(direction), sin(direction)
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //normal
        // Step 1-3
        // Integrate
        updatePositionExplicitEuler(Time.fixedDeltaTime);
        //updatePositionKinematic(Time.fixedDeltaTime);
        updateRotationEulerExplicit(Time.fixedDeltaTime);
        //updateRotationKinematic(Time.fixedDeltaTime);

        // Step 2-2
        UpdateAcceleration();

       if (Input.GetKey(KeyCode.UpArrow))
       {
            //move up
            Vector2 force =  transform.up * speed * Time.fixedDeltaTime;
            AddForce(force);
            //position += force;
        }
       else if(Input.GetKey(KeyCode.DownArrow))
       {
            //movedown
            Vector2 force = -transform.up * speed * Time.fixedDeltaTime;
            AddForce(force);
            //position += force;
       }
       if(Input.GetKey(KeyCode.RightArrow))
       {
            //rotate right
            //angularAcceleration -= 0.01f;
            rotation = rotation % 360;
            calculateDiskInertia();
            inverseInertia = 1 / inertia;
            
            applyTorque((Vector2)Vector3.up + (Vector2)transform.position, appliedForce);
            updateAngularAcceleration();
        }
       else if(Input.GetKey(KeyCode.LeftArrow))
       {
            //rotate left
            //angularAcceleration += 0.01f;
            //
            rotation = rotation % 360;
            calculateDiskInertia();
            inverseInertia = 1 / inertia;
            
            applyTorque((Vector2)Vector3.up + (Vector2)transform.position, -appliedForce);
            updateAngularAcceleration();

        }
        

       if(transform.position.y < -1.5f)
       {
          position.y = 11;
       }
       if(transform.position.y > 11)
       {
            position.y = -1.5f;
       }
        if (transform.position.x < -11)
        {
            position.x = 11;
        }
        if (transform.position.x > 11)
        {
            position.x = -11;
        }
        // Apply to transform
        transform.position = position;
        particlePosition = transform.position;

        transform.Rotate(0, 0, angularAcceleration);

    }

}
