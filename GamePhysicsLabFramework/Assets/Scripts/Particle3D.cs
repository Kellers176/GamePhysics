using System.Collections;
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
    //Step 3
    public float inertia;
    public float inverseInertia;
    Vector2 appliedForce;

    Matrix4x4 worldTransformationMatrix;
    Matrix4x4 invWorldTransformationMatrix;

    Vector3 localCenterOfMass, worldCenterOfMass;

    Matrix4x4 localInertiaTensor, worldInertiaTensor;
    //Matriox4x4.trs (transform, rotation, scale)


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
        angularAcceleration = torque * inverseInertia;
        //       torque = inertia * angularAcceleration;
    }

    void applyTorque(Vector3 force, Vector3 pointOfForce)
    {
        //applied torque: T = I * a
        //T is torque, pf is moment arm (point of applied force relative to center of mass), F is applied force at pf. 
        //center of mass may not be the center of the object
        
        Vector2 momentArm = pointOfForce - position;

        //torque += (momentArm.x * force.y - momentArm.y * force.x);
        torque = Vector3.Cross(momentArm, pointOfForce);
    }

    void SetITSolidSphere(float scale)
    {
        //      |2/5mr^2                        |
        // I =  |           2/5mr^2             |
        //      |                       2/5mr^2 |
        //could be set in start
        localInertiaTensor.m00 = 2 / 5 * mass * ((scale / 2) * (scale / 2));
        localInertiaTensor.m11 = 2 / 5 * mass * ((scale / 2) * (scale / 2));
        localInertiaTensor.m22 = 2 / 5 * mass * ((scale / 2) * (scale / 2));
        localInertiaTensor.m33 = 0;




        //change of basis: take our local torque, apply our tensor to it, more it back to world


    }
    void SetITHollowSphere(float radius)
    {
        //      |2/3mr^2                        |
        // I =  |           2/3mr^2             |
        //      |                       2/3mr^2 |
        localInertiaTensor.m00 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m11 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m22 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m33 = 0;

    }
    void SetITSolidBox(float height, float depth, float width)
    {
        //      |1/12m(h^2 + d^2)                                       |
        // I =  |                   1/12m(d^2 + w^2)                    |
        //      |                                       1/12m(w^2 + h^2)|
        localInertiaTensor.m00 = 1 / 12 * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 1 / 12 * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 1 / 12 * mass * ((width * width) + (height * height));
        localInertiaTensor.m33 = 0;



    }
    void SetITHollowBox(float height, float depth, float width)
    {
        //      |5/3m(h^2 + d^2)                                       |
        // I =  |                   5/3m(d^2 + w^2)                    |
        //      |                                       5/3m(w^2 + h^2)|
        localInertiaTensor.m00 = 5 / 3 * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 5 / 3 * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 5 / 3 * mass * ((width * width) + (height * height));
        localInertiaTensor.m33 = 0;


    }
    void SetITSolidCylinder(float radius, float height)
    {
        //      |1/12m(3r^2 + h^2)                                  |
        // I =  |                   1/12m(3r^2 + h^2)               |
        //      |                                          1/2mr^2  |
        localInertiaTensor.m00 = 1 / 12 * mass * ((3 * (radius * radius)) + (height * height));
        localInertiaTensor.m11 = 1 / 12 * mass * ((3 * (radius * radius)) + (height * height));
        localInertiaTensor.m22 = 1 / 2 * mass * (radius * radius);
        localInertiaTensor.m33 = 0;



    }
    void SetITSolidCone(float height, float radius)
    {
        //      |3/5mh^2 + 3/20mr^2                                  |
        // I =  |                   3/5mh^2 + 3/20mr^2               |
        //      |                                          3/10mr^2  |
        localInertiaTensor.m00 = (3 / 5 * mass * (height * height)) + (3 / 20 * mass * (radius * radius));
        localInertiaTensor.m11 = (3 / 5 * mass * (height * height)) + (3 / 20 * mass * (radius * radius));
        localInertiaTensor.m22 = 3 / 10 * mass * (radius * radius);
        localInertiaTensor.m33 = 0;



    }



    // Start is called before the first frame update
    void Start()
    {
        SetMass(startingMass);
        

        inertia = 0f;
        appliedForce = new Vector2(1, 0);

        //normal = cos(direction), sin(direction)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //normal
        // Step 1-3
        // Integrate
        //updatePositionExplicitEuler(Time.fixedDeltaTime);
        //updatePositionKinematic(Time.fixedDeltaTime);
        updateRotationEulerExplicit(Time.fixedDeltaTime);
        //updateRotationKinematic(Time.fixedDeltaTime);

        inverseInertia = 1 / inertia;


        applyTorque(new Vector2(5, 3), appliedForce);
        // Step 2-2
        UpdateAcceleration();

        acceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f, 0, 0);
        angularAcceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f,0,0);

        //       // Apply to transform
        transform.position = position;
        //particlePosition = transform.position;
        transform.rotation = newRotation;
        // transform.Rotate(0, 0, rotation);

    }

}
