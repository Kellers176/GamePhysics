﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Step 1-1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    // Step 2-1
    public float startingMass;
    float mass, massInv;
    Vector2 vectorReflect;
    Vector2 particleVelocity;
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
    public float torque;
    Vector2 localCenterOfMass;
    Vector2 worldCenterOfMass;
    Vector2 appliedForce;
    
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

    // Step 2-2
    public Vector2 force;
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
        inertia = 0.5f * mass * ((transform.localScale.x * 0.5f) * (transform.localScale.x * 0.5f));
    }
    void calculateSquareInertia()
    {
        inertia = (1 / 12) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
    }
    void updateAngularAcceleration()
    {
        //converts torque to angular acceleration
        //resets torque
        // T = IA -> A = I^-1 * T
        angularAcceleration = torque / inertia;
        torque = inertia * angularAcceleration;
    }

    void applyTorque()
    {
        //applied torque: T = pf x F
        //T is torque, pf is moment arm (point of applied force relative to center of mass), F is applied force at pf. 
        //center of mass may not be the center of the object
        //might help to add a separate member for center of mass in local and world space
        //center of mass = 0;
        //world = local object * transform of object matrix
        localCenterOfMass = new Vector2(0, 0);
//        localCenterOfMass = new Vector2(transform.localPosition.x, transform.localPosition.y);
        worldCenterOfMass = transform.localToWorldMatrix.MultiplyVector(localCenterOfMass);

        torque = Vector3.Cross(worldCenterOfMass, appliedForce).y;
    }


    // Start is called before the first frame update
    void Start()
    {
        SetMass(startingMass);
        particleVelocity = new Vector2(0.5f, 0);
        frictionCoefficient = 0.5f;
        vectorReflect = new Vector2(-Mathf.Sqrt(3) * 0.5f, 0.5f);
//        vectorReflect = new Vector2( Mathf.Sqrt(3) * 0.5f, 0.5f);
        fluidVelocity = new Vector2(1, 0);
        fluidDensity = 1.0f;
        objectArea_crossSection = 3.0f;
        objectDragCoefficient = 0.5f;
        anchorPosition = new Vector2(0, 0);
        springRestingLength = 2.0f;
        springStiffnessCoefficient = 5.0f;

        inertia = 0f;
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

       

//       if(gameObject.GetComponent<MeshFilter>().mesh.name == "Cube Instance")
//       {
//           if(gameObject.transform.localScale.x == transform.localScale.y)
//           {
//               //we are cube
//               Debug.Log("This is a cube");
//               calculateSquareInertia();
//           }
//           else
//           {
//               Debug.Log("This is a box");
//               calculateBoxInertia();
//           }
//       }
//       else if(gameObject.GetComponent<MeshFilter>().mesh.name == "Sphere Instance")
//       {
//           Debug.Log("This is a sphere");
//           calculateDiskInertia();
//       }
//       applyTorque();
//       updateAngularAcceleration();
//
//       // Apply to transform
       transform.position = position;
       particlePosition = transform.position;
       
        calculateBoxInertia();
        // Step 1-4
        // test
        // acceleration.x = -Mathf.Sin(Time.fixedTime);
        // angularAcceleration = -Mathf.Sin(Time.fixedTime);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //move up
            acceleration.y = 5;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            //movedown
            acceleration.y = -5;
        }
        else
        {
            acceleration.y = 0;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //rotate right
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            //rotate left
            appliedForce = new Vector2(1, 0);
            applyTorque();
            updateAngularAcceleration();

        }
        transform.Rotate(0, 0, rotation);
        
        // Step 2-2 --------------------------------------------------------------------------------------------------------------------------------
        // f_gravity: f = mg
        Vector2 gravity = ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up);
        Vector2 normal = ForceGenerator.GenerateForce_Normal(gravity, vectorReflect);
//        AddForce(ForceGenerator.GenerateForce_Sliding(gravity, normal));
//        AddForce(ForceGenerator.GenerateForce_Friction_Static(normal, particleVelocity, frictionCoefficient));
//        AddForce(ForceGenerator.GenerateForce_friction_kinetic(normal, particleVelocity, frictionCoefficient));
//        AddForce(ForceGenerator.GenerateForce_drag(particleVelocity, fluidVelocity, fluidDensity, objectArea_crossSection, objectDragCoefficient));
 //       AddForce(ForceGenerator.GenerateForce_spring(particlePosition, anchorPosition, springRestingLength, springStiffnessCoefficient));
    }

}
