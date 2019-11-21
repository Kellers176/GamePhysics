using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TestDLLScript
{
    [DllImport("TestDLL")]
    public static extern int InitParticle2D();
    [DllImport("TestDLL")]
    public static extern float GetMass();
    [DllImport("TestDLL")]
    public static extern int SetMass(float newMass);
    [DllImport("TestDLL")]
    public static extern int AddForce(float newForceX, float newForceY, float newForceZ);
    [DllImport("TestDLL")]
    public static extern int UpdateAcceleration();
    [DllImport("TestDLL")]
    public static extern int UpdatePositionExplicitEuler(float dt);
    [DllImport("TestDLL")]
    public static extern int UpdatePositionKinematic(float dt);
    [DllImport("TestDLL")]
    public static extern int UpdateRotationExplicitEuler(float dt);
    [DllImport("TestDLL")]
    public static extern int UpdateRotationKinematic(float dt);
    [DllImport("TestDLL")]
    public static extern int TermParticle2D();
}
