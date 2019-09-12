using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        SetMass(startingMass);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Step 1-3
        // Integrate
        updatePositionExplicitEuler(Time.fixedDeltaTime);
        //updatePositionKinematic(Time.fixedDeltaTime);
        //updateRotationEulerExplicit(Time.fixedDeltaTime);
        //updateRotationKinematic(Time.fixedDeltaTime);

        // Step 2-2
        UpdateAcceleration();

        // Apply to transform
        transform.position = position;
        // transform.Rotate(0, 0, rotation);

        // Step 1-4
        // test
        // acceleration.x = -Mathf.Sin(Time.fixedTime);
        // angularAcceleration = -Mathf.Sin(Time.fixedTime);

        // Step 2-2
        // f_gravity: f = mg
        //Vector2 f_gravity = mass * new Vector2(0.0f, -9.8f);
        //AddForce(f_gravity);
        AddForce(ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up));
    }
}
