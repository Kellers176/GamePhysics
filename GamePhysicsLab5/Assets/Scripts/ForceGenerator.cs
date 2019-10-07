﻿using System.Collections;
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

 //       Vector2 f_normal =  f_gravity.magnitude * surfaceNormal_unit;
        Vector2 f_normal = Vector3.Project(-f_gravity, surfaceNormal_unit);
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
        Vector2 f_friction_s; 
        //Vector2 f_friction_s = -f_opposing < (frictionCoefficient_static * new Vector2(Mathf.Abs(f_normal.x), Mathf.Abs(f_normal.y))) ? -(frictionCoefficient_static)*f_normal : new Vector2(0.0f, 0.0f);
       if(lessThanVec2(-f_opposing, (frictionCoefficient_static * new Vector2(f_normal.magnitude, f_normal.magnitude))))
        {
            f_friction_s = -f_opposing;
        }
        else
        {
            f_friction_s = frictionCoefficient_static * -f_opposing;
        }

        return f_friction_s;
    }

    public static Vector2 GenerateForce_friction_kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        Vector2 f_friction_k = -frictionCoefficient_kinetic * new Vector2(f_normal.magnitude, f_normal.magnitude) * /*unit?*/ particleVelocity;
        return f_friction_k;
    }
    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        Vector2 u = fluidVelocity * fluidDensity;
        //potential energy (u) = 1/2 * mass * V^2
        //Vector2 u = 0.5f * mass * fluidVelocity * fluidVelocity;
        // f_drag = (p * u^2 * area * coeff)/2
        //-normalParticle(strengthofDrag*normalParticle + strength*normalParticle^2)
        //        Vector2 f_drag = (particleVelocity * u * u * objectArea_crossSection * objectDragCoefficient) * 0.5f;
        //Vector2 f_drag = -particleVelocity.normalized * (objectDragCoefficient * particleVelocity.normalized + objectDragCoefficient * particleVelocity.normalized * particleVelocity.normalized) ;
        Vector2 f_drag = new Vector2(0, 0);
        Vector2 velocityDiff = particleVelocity - fluidVelocity;
        float velocityDiffMag = velocityDiff.magnitude;

        f_drag = objectDragCoefficient * (fluidDensity * (velocityDiff) * (velocityDiffMag) * 0.5f) * objectArea_crossSection;
        return f_drag;
    }

    public static Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f_spring = -coeff*(spring length - spring resting length)
        Vector2 displacement = particlePosition - anchorPosition;
        float length = displacement.magnitude;
        //use hooks law
        //-springStiffnessCoefficient * displacement * length;
        //Vector2 f_spring = -springStiffnessCoefficient * new Vector2(length - springRestingLength, length - springRestingLength);
        Vector2 f_spring = displacement * springStiffnessCoefficient * (springRestingLength - length) / length;
        return f_spring;
    }

    static bool lessThanVec2(Vector2 vec1, Vector2 vec2)
    {
        if(vec1.x < vec2.x && vec1.y < vec2.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}