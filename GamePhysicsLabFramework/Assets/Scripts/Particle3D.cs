﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    // Step 1-1
    public Vector3 position, velocity, acceleration;
    public Quaternion rotation;
    Quaternion newRotation;
    public Vector3 angularVelocity, angularAcceleration, torque;

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
    Vector2 localCenterOfMass;
    Vector2 worldCenterOfMass;
    Vector2 appliedForce;

    float w, x, y, z;


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

    public Quaternion multiplyQuatByScalar(float i, Quaternion quat)
    {
        Quaternion temp = new Quaternion(quat.w * i, quat.x * i, quat.y * i, quat.z * i);
        return temp;
    }

    public Quaternion multiplyQuatBy3DVec(Vector3 vec, Quaternion quat)
    {

        Vector3 tempQuat = new Vector3(quat.x, quat.y, quat.z);
        Vector3 crossProduct = (0 * tempQuat) + (quat.w * vec) + (Vector3.Cross(vec, tempQuat));

        
        Quaternion finalQuat = new Quaternion((0 * quat.w) - Vector3.Dot(vec, tempQuat), crossProduct.x, crossProduct.y, crossProduct.z);

        return finalQuat;
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

        //derivative for the quaternion
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
        //rotation.x += angularVelocity.x * dt;
        //rotation.y += angularVelocity.y * dt;
        //rotation.z += angularVelocity.z * dt;
        Quaternion temp;
        //scaled vector
        //multiplyQuatByScalar(0.5f, ref rotation);
        temp =  multiplyQuatBy3DVec(angularVelocity, rotation);
        Quaternion temp2 = multiplyQuatByScalar(0.5f /** dt*/, temp);
        newRotation = new Quaternion(rotation.w + temp2.w, rotation.x + temp2.x, rotation.y + temp2.y, rotation.z + temp2.z);
        //multiplyQuatBy3DVec(angularVelocity * dt, ref rotation);
        //Quaternion invRotation = new Quaternion(rotation.w, -rotation.x, -rotation.y, -rotation.z);
        //rotation *= invRotation;
        angularVelocity += angularAcceleration * dt;
    }
//    void updateRotationKinematic(float dt)
//    {
//        //1/2 * wt * qt
//        multiplyQuatBy3DVec(angularVelocity * dt + (0.5f * angularAcceleration * (dt * dt)), ref rotation);
//        //multiplyQuatByScalar(0.5f, ref rotation);
//        angularVelocity += angularAcceleration * dt;
//    }

    //Step 3
    void calculateBoxInertia()
    {
        // I = 1/12m(dx^2 + dy^2)
        inertia = (1 / 12) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
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

        //torque = Vector3.Cross(worldCenterOfMass, appliedForce).y;
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
        appliedForce = new Vector2(1, 0);
        w = 0;
        x = 0;
        y = 0;
        z = 0;
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
        //updateRotationEulerExplicit(Time.fixedDeltaTime);
        //updateRotationKinematic(Time.fixedDeltaTime);

        // Step 2-2
        UpdateAcceleration();

        acceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f, 0, 0);
        angularAcceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f,0,0);

        //       // Apply to transform
        transform.position = position;
        particlePosition = transform.position;
        transform.rotation = newRotation;
        // transform.Rotate(0, 0, rotation);

    }

}
