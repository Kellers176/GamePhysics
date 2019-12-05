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
    float inertia;
    float inverseInertia;
    public Vector3 appliedForce;
    public Vector3 forceVar;

    public Matrix4x4 worldTransformationMatrix;
    Matrix4x4 invWorldTransformationMatrix;

    Vector3 localCenterOfMass, worldCenterOfMass;

    Matrix4x4 localInertiaTensor, worldInertiaTensor;
    //Matriox4x4.trs (transform, rotation, scale)

    Matrix4x4 transformMat;
    public Matrix4x4 RotationMat;
    Matrix4x4 ScaleMat;

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
        tempMat.m00 = (1.0f - ((2.0f * (quat.y * quat.y)) - (2.0f * (quat.z * quat.z))));
        tempMat.m01 = (2.0f * quat.x * quat.y) + (2.0f * quat.z * quat.w);
        tempMat.m02 = (2.0f * quat.x * quat.z) - (2.0f * quat.y * quat.w);
        tempMat.m03 = 0.0f;
        tempMat.m10 = (2.0f * quat.x * quat.y) - (2.0f * quat.z * quat.w);
        tempMat.m11 = (1.0f - ((2.0f * (quat.x * quat.x)) - (2.0f * (quat.z * quat.z))));
        tempMat.m12 = (2.0f * quat.y * quat.z) + (2.0f * quat.x * quat.w);
        tempMat.m13 = 0.0f;
        tempMat.m20 = (2.0f * quat.x * quat.z) + (2.0f * quat.y * quat.w);
        tempMat.m21 = (2.0f * quat.y * quat.z) - (2.0f * quat.x * quat.w);
        tempMat.m22 = (1.0f - ((2.0f * (quat.x * quat.x)) - (2.0f * (quat.y * quat.y))));
        tempMat.m23 = 0.0f;
        tempMat.m30 = 0.0f;
        tempMat.m31 = 0.0f;
        tempMat.m32 = 0.0f;
        tempMat.m33 = 1.0f;

        return tempMat;
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
    Vector3 force;
    public void AddForce(Vector3 newForce)
    {
        // D'Alembert
        force += newForce;
    }
    void UpdateAcceleration()
    {
        // Newton2
        acceleration = massInv * force;

        force.Set(0.0f, 0.0f, 0.0f);
    }

    void SetPositionX(float posX)
    {
        position.x = posX;
    }
    void SetPositionY(float posY)
    {
        position.y = posY;
    }
    void SetPositionZ(float posZ)
    {
        position.z = posZ;
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
        newRotation = Quaternion.Normalize(newRotation);

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
        inertia = (1.0f / 12.0f) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
    }
    void calculateDiskInertia()
    {
        // I = 1/12m(dx^2 + dy^2)
        inertia = 0.5f * mass * ((transform.localScale.x * 0.5f) * (transform.localScale.x * 0.5f));
    }
    void calculateSquareInertia()
    {
        inertia = (1.0f / 12.0f) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
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
        //change of basis
        angularAcceleration = worldTransformationMatrix * worldInertiaTensor * transformInverse(worldTransformationMatrix, torque);
        torque = Vector3.zero;
    }

    void applyTorque(Vector3 force, Vector3 pointOfForce)
    {
        //applied torque: T = I * a
        //T is torque, pf is moment arm (point of applied force relative to center of mass), F is applied force at pf. 
        //center of mass may not be the center of the object
        localCenterOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        
        Vector3 momentArm = force - localCenterOfMass;

        //torque += (momentArm.x * force.y - momentArm.y * force.x);
        torque += Vector3.Cross(momentArm, pointOfForce);
    }


    // Start is called before the first frame update
    void Start()
    {
        SetMass(startingMass);
        

        inertia = 0f;
        appliedForce = new Vector3(0, 1, 0);
        forceVar = new Vector3(0.3f, 0.2f, 0.1f);



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
        //       updateRotationEulerExplicit(Time.fixedDeltaTime);
        //updateRotationKinematic(Time.fixedDeltaTime);

        UpdateAcceleration();

//        inverseInertia = 1 / inertia;
//        SetITSolidBox(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
//        RotationMat = QuaternionToMatrix(newRotation);
//        Debug.Log(newRotation);
//        worldTransformationMatrix = new Matrix4x4(
//            new Vector4(RotationMat.m00, RotationMat.m01, RotationMat.m02, position.x),
//            new Vector4(RotationMat.m10, RotationMat.m11, RotationMat.m12, position.y),
//            new Vector4(RotationMat.m20, RotationMat.m21, RotationMat.m22, position.z),
//            new Vector4(0,0,0,1)
//            );
//        applyTorque(forceVar, appliedForce);
//        transform.rotation = newRotation;

    }


    void SetITSolidSphere(float scale)
    {
        //      |2/5mr^2                        |
        // I =  |           2/5mr^2             |
        //      |                       2/5mr^2 |
        //could be set in start
        localInertiaTensor.m00 = 2.0f / 5.0f * mass * ((scale / 2.0f) * (scale / 2.0f));
        localInertiaTensor.m11 = 2.0f / 5.0f * mass * ((scale / 2.0f) * (scale / 2.0f));
        localInertiaTensor.m22 = 2.0f / 5.0f * mass * ((scale / 2.0f) * (scale / 2.0f));


        //change of basis: take our local torque, apply our tensor to it, move it back to world
        //worldInertiaTensor = (localInertiaTensor, torque);
        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;

    }
    void SetITHollowSphere(float radius)
    {
        //      |2/3mr^2                        |
        // I =  |           2/3mr^2             |
        //      |                       2/3mr^2 |
        localInertiaTensor.m00 = 2.0f / 3.0f * mass * ((radius / 2.0f) * (radius / 2.0f));
        localInertiaTensor.m11 = 2.0f / 3.0f * mass * ((radius / 2.0f) * (radius / 2.0f));
        localInertiaTensor.m22 = 2.0f / 3.0f * mass * ((radius / 2.0f) * (radius / 2.0f));

        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;

    }
    void SetITSolidBox(float height, float depth, float width)
    {
        //      |1/12m(h^2 + d^2)                                       |
        // I =  |                   1/12m(d^2 + w^2)                    |
        //      |                                       1/12m(w^2 + h^2)|
        localInertiaTensor.m00 = 1.0f / 12.0f * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 1.0f / 12.0f * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 1.0f / 12.0f * mass * ((width * width) + (height * height));

        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;


    }
    void SetITHollowBox(float height, float depth, float width)
    {
        //      |5/3m(h^2 + d^2)                                       |
        // I =  |                   5/3m(d^2 + w^2)                    |
        //      |                                       5/3m(w^2 + h^2)|
        localInertiaTensor.m00 = 5.0f / 3.0f * mass * ((height * height) + (depth * depth));
        localInertiaTensor.m11 = 5.0f / 3.0f * mass * ((depth * depth) + (width * width));
        localInertiaTensor.m22 = 5.0f / 3.0f * mass * ((width * width) + (height * height));

        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;


    }
    void SetITSolidCylinder(float radius, float height)
    {
        //      |1/12m(3r^2 + h^2)                                  |
        // I =  |                   1/12m(3r^2 + h^2)               |
        //      |                                          1/2mr^2  |
        localInertiaTensor.m00 = 1.0f / 12.0f * mass * ((3.0f * (radius * radius)) + (height * height));
        localInertiaTensor.m11 = 1.0f / 12.0f * mass * ((3.0f * (radius * radius)) + (height * height));
        localInertiaTensor.m22 = 1.0f / 2.0f * mass * (radius * radius);


        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;



    }
    void SetITSolidCone(float height, float radius)
    {
        //      |3/5mh^2 + 3/20mr^2                                  |
        // I =  |                   3/5mh^2 + 3/20mr^2               |
        //      |                                          3/10mr^2  |
        localInertiaTensor.m00 = (3.0f / 5.0f * mass * (height * height)) + (3.0f / 20.0f * mass * (radius * radius));
        localInertiaTensor.m11 = (3.0f / 5.0f * mass * (height * height)) + (3.0f / 20.0f * mass * (radius * radius));
        localInertiaTensor.m22 = 3.0f / 10.0f * mass * (radius * radius);

        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor.m00 = 1.0f / localInertiaTensor.m00;
        worldInertiaTensor.m11 = 1.0f / localInertiaTensor.m11;
        worldInertiaTensor.m22 = 1.0f / localInertiaTensor.m22;

    }



}
