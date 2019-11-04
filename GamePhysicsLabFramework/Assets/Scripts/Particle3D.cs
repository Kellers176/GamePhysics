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
    //Step 3
    public float inertia;
    public float inverseInertia;
    Vector2 appliedForce;

    Matrix4x4 worldTransformationMatrix;
    Matrix4x4 invWorldTransformationMatrix;

    Vector3 localCenterOfMass, worldCenterOfMass;

    Matrix4x4 localInertiaTensor, worldInertiaTensor;
    //Matriox4x4.trs (transform, rotation, scale)

    Matrix4x4 transformMat;

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
        Vector3 crossProduct = (quat.w * vec) + (Vector3.Cross(vec, tempQuat));
        
        Quaternion finalQuat = new Quaternion(-Vector3.Dot(vec, tempQuat), crossProduct.x, crossProduct.y, crossProduct.z);

        return finalQuat;
    }

    // Turn quaternion into a matrix
    public Matrix4x4 QuaternionToMatrix(Quaternion quat)
    {
        Matrix4x4 tempMat = new Matrix4x4();
        tempMat.m00 = (1 - ((2 * (quat.y * quat.y)) + (2 * (quat.z * quat.z))));
        tempMat.m01 = (2 * quat.x * quat.y) + (2 * quat.z * quat.w);
        tempMat.m02 = (2 * quat.x * quat.z) - (2 * quat.y * quat.w);
        tempMat.m03 = 0;
        tempMat.m10 = (2 * quat.x * quat.y) - (2 * quat.z * quat.w);
        tempMat.m11 = (1 - ((2 * (quat.x * quat.x)) + (2 * (quat.z * quat.z))));
        tempMat.m12 = (2 * quat.y * quat.z) + (2 * quat.x * quat.w);
        tempMat.m13 = 0;
        tempMat.m20 = (2 * quat.x * quat.z) + (2 * quat.y * quat.w);
        tempMat.m21 = (2 * quat.y * quat.z) - (2 * quat.x * quat.w);
        tempMat.m22 = (1 - ((2 * (quat.x * quat.x)) + (2 * (quat.y * quat.y))));
        tempMat.m23 = 0;
        tempMat.m30 = 0;
        tempMat.m31 = 0;
        tempMat.m32 = 0;
        tempMat.m33 = 1;

        return tempMat;
    }

    // calculate the transform matrix
    Matrix4x4 calcWorldTransformMat()
    {
        Matrix4x4 tempMat = new Matrix4x4();
        //tempMat = translation * rotation * scale;
        tempMat = calcTransformMat() * QuaternionToMatrix(newRotation) * calcScaleMat(transform.localScale);
        return tempMat;
    }

    // calculate the translation matrix
    Matrix4x4 calcTransformMat()
    {
        Matrix4x4 tempMat = new Matrix4x4();
        tempMat.m00 = 1;
        tempMat.m01 = 0;
        tempMat.m02 = 0;
        tempMat.m03 = 0;
        tempMat.m10 = 0;
        tempMat.m11 = 1;
        tempMat.m12 = 0;
        tempMat.m13 = 0;
        tempMat.m20 = 0;
        tempMat.m21 = 0;
        tempMat.m22 = 1;
        tempMat.m23 = 0;
        tempMat.m30 = position.x;
        tempMat.m31 = position.y;
        tempMat.m32 = position.z;
        tempMat.m33 = 1;
        return tempMat;
    }

    // calculate the scale matrix
    Matrix4x4 calcScaleMat(Vector3 scale)
    {
        Matrix4x4 tempMat = new Matrix4x4();
        tempMat.m00 = scale.x;
        tempMat.m01 = 0;
        tempMat.m02 = 0;
        tempMat.m03 = 0;
        tempMat.m10 = 0;
        tempMat.m11 = scale.y;
        tempMat.m12 = 0;
        tempMat.m13 = 0;
        tempMat.m20 = 0;
        tempMat.m21 = 0;
        tempMat.m22 = scale.z;
        tempMat.m23 = 0;
        tempMat.m30 = 0;
        tempMat.m31 = 0;
        tempMat.m32 = 0;
        tempMat.m33 = 1;
        return tempMat;
    }

    Matrix4x4 setInverse(Matrix4x4 mat)
    {
        Matrix4x4 m = new Matrix4x4();
        m.m00 = (mat.m11 * mat.m22) - (mat.m12 * mat.m21);
        m.m01 = (mat.m02 * mat.m21) - (mat.m01 * mat.m22);
        m.m02 = (mat.m01 * mat.m12) - (mat.m02 * mat.m11);
        m.m03 = 0;
        m.m10 = (mat.m12 * mat.m20) - (mat.m10 * mat.m22);
        m.m11 = (mat.m00 * mat.m22) - (mat.m02 * mat.m20);
        m.m12 = (mat.m02 * mat.m10) - (mat.m00 * mat.m12);
        m.m13 = 0;
        m.m20 = (mat.m10 * mat.m21) - (mat.m11 * mat.m20);
        m.m21 = (mat.m01 * mat.m20) - (mat.m00 * mat.m21);
        m.m22 = (mat.m00 * mat.m11) - (mat.m01 * mat.m10);
        m.m23 = 0;
        m.m30 = 0;
        m.m31 = 0;
        m.m32 = 0;
        m.m33 = 1;
        float det = (mat.m00 * mat.m11 * mat.m22) +
                    (mat.m10 * mat.m21 * mat.m02) +
                    (mat.m20 * mat.m01 * mat.m12) -
                    (mat.m00 * mat.m21 * mat.m12) -
                    (mat.m20 * mat.m11 * mat.m02) -
                    (mat.m10 * mat.m01 * mat.m22);
        Matrix4x4 deter = new Matrix4x4();
        deter.m00 = deter.m01 = deter.m02 = deter.m10 =
            deter.m11 = deter.m12 = deter.m20 = deter.m21 =
            deter.m22 = (1 / det);
        deter.m03 = deter.m13 = deter.m23 = deter.m30 = deter.m31
            = deter.m32 = 0;
        deter.m33 = 1;
        m = m * deter;
        return m;
    }

    /// <summary>
    /// Transforms the given vector by the transformatinal inverse of matrix : from the book
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    Vector3 transformInverse(Matrix4x4 mat, Vector3 vec)
    {
        Vector3 tmp = vec;

        tmp.x -= mat[3];
        tmp.y -= mat[7];
        tmp.z -= mat[11];

        return new Vector3(
            tmp.x * mat[0] + tmp.y * mat[4] + tmp.z * mat[8],
            tmp.x * mat[1] + tmp.y * mat[5] + tmp.z * mat[9],
            tmp.x * mat[2] + tmp.y * mat[6] + tmp.z * mat[10]
            );

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
        Quaternion temp;
        temp = multiplyQuatBy3DVec(angularVelocity, rotation);

        Quaternion temp2 = multiplyQuatByScalar(0.5f, temp);

        newRotation = new Quaternion(rotation.x + temp2.x, rotation.y + temp2.y, rotation.z + temp2.z, rotation.w + temp2.w);
        Quaternion.Normalize(newRotation);

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
        //angularAcceleration = torque * inverseInertia;
        //       torque = inertia * angularAcceleration;

        // angularAccel = (objectWorldOrientation * inertiaTensorInverse * objectWorldOrientationInverse) * torque
        // do our world transform, multiply it by inverse inertia and then multiply that by inverse world and torque
        // localInertiaTensor needs to be inverse
        angularAcceleration = worldTransformationMatrix * localInertiaTensor * transformInverse(worldTransformationMatrix,torque);
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
        SetITHollowBox(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        transformMat = calcTransformMat();
        worldTransformationMatrix = calcWorldTransformMat();

        applyTorque(new Vector2(5, 3), appliedForce);
        // Step 2-2
        updateAngularAcceleration();

        //acceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f, 0, 0);
        //angularAcceleration = new Vector3(-Mathf.Sin(Time.fixedTime) * 5f,0,0);

        //       // Apply to transform
        transform.position = position;
        //particlePosition = transform.position;
        transform.rotation = newRotation;
        // transform.Rotate(0, 0, rotation);

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
        localInertiaTensor.m33 = 1;


        //change of basis: take our local torque, apply our tensor to it, move it back to world
        //worldInertiaTensor = (localInertiaTensor, torque);

    }
    void SetITHollowSphere(float radius)
    {
        //      |2/3mr^2                        |
        // I =  |           2/3mr^2             |
        //      |                       2/3mr^2 |
        localInertiaTensor.m00 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m11 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m22 = 2 / 3 * mass * ((radius / 2) * (radius / 2));
        localInertiaTensor.m33 = 1;

    }
    void SetITSolidBox(float height, float depth, float width)
    {
        //      |1/12m(h^2 + d^2)                                       |
        // I =  |                   1/12m(d^2 + w^2)                    |
        //      |                                       1/12m(w^2 + h^2)|
        localInertiaTensor.m00 = 1 / 12 * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 1 / 12 * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 1 / 12 * mass * ((width * width) + (height * height));
        localInertiaTensor.m33 = 1;



    }
    void SetITHollowBox(float height, float depth, float width)
    {
        //      |5/3m(h^2 + d^2)                                       |
        // I =  |                   5/3m(d^2 + w^2)                    |
        //      |                                       5/3m(w^2 + h^2)|
        localInertiaTensor.m00 = 5 / 3 * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 5 / 3 * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 5 / 3 * mass * ((width * width) + (height * height));
        localInertiaTensor.m33 = 1;


    }
    void SetITSolidCylinder(float radius, float height)
    {
        //      |1/12m(3r^2 + h^2)                                  |
        // I =  |                   1/12m(3r^2 + h^2)               |
        //      |                                          1/2mr^2  |
        localInertiaTensor.m00 = 1 / 12 * mass * ((3 * (radius * radius)) + (height * height));
        localInertiaTensor.m11 = 1 / 12 * mass * ((3 * (radius * radius)) + (height * height));
        localInertiaTensor.m22 = 1 / 2 * mass * (radius * radius);
        localInertiaTensor.m33 = 1;



    }
    void SetITSolidCone(float height, float radius)
    {
        //      |3/5mh^2 + 3/20mr^2                                  |
        // I =  |                   3/5mh^2 + 3/20mr^2               |
        //      |                                          3/10mr^2  |
        localInertiaTensor.m00 = (3 / 5 * mass * (height * height)) + (3 / 20 * mass * (radius * radius));
        localInertiaTensor.m11 = (3 / 5 * mass * (height * height)) + (3 / 20 * mass * (radius * radius));
        localInertiaTensor.m22 = 3 / 10 * mass * (radius * radius);
        localInertiaTensor.m33 = 1;



    }



}