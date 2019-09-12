using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }
    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        Vector2 f_normal = Vector3.Project(new Vector3(f_gravity.x, f_gravity.y, 1.0f), new Vector3(surfaceNormal_unit.x, surfaceNormal_unit.y, 1.0f));
        return f_normal;
    }
    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        //f_sliding = f_gravity + f_normal
        Vector2 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }
    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal
        // (max amount is coeff*|f_normal|)
        Vector2 f_friction_s = -f_opposing < (frictionCoefficient_static * new Vector2(Mathf.Abs(f_normal.x), Mathf.Abs(f_normal.y))) ? -(frictionCoefficient_static)*f_normal : new Vector2(0.0f, 0.0f);
        return f_friction_s;
    }
}
