using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    //step 2
    void updatePositionExplicitEuler(float dt)
    {
        //x(t + dt) = x(t) + v(t)dt
        //Euler's method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        position += velocity * dt;

        //****
        //v(t + dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    void updatePositionKinematic(float dt)
    {
        //x(t + dt) = x(t) + v(t)dt + (1/2)*acceleration(t)(dt)^2
        position += (velocity * dt) + (0.5f * acceleration * (dt * dt));
        velocity += acceleration * dt;
    }
    void updateRotationEulerExplicit(float dt)
    {
        rotation += angularVelocity * dt;
        angularVelocity += angularAcceleration * dt;
    }
    void updateRotationKinematic(float dt)
    {
        rotation += (angularVelocity * dt) + (0.5f * angularAcceleration * (dt * dt));
        angularVelocity += angularAcceleration * dt;
    }






    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        //step 3
        //integrate
        //updatePositionExplicitEuler(Time.fixedDeltaTime);
        updatePositionKinematic(Time.fixedDeltaTime);
        updateRotationEulerExplicit(Time.fixedDeltaTime);
        //apply to transform
        transform.position = position;
        transform.Rotate(0, 0, rotation);

        //step4
        //test
        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration = -Mathf.Sin(Time.fixedTime);
    }
}
