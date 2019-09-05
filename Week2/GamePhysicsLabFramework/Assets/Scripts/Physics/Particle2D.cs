using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;
    const int size = 5;

    public TMP_Dropdown myDropdown;
    public TMP_Dropdown myDrowpdown2;

    public TextMeshProUGUI[] myArray = new TextMeshProUGUI[size];

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

    void ResetValues()
    {
        rotation = angularAcceleration = 0;
        angularVelocity = 1;
        //position = acceleration = new Vector2(0,0);
        //velocity = new Vector2(1, 0);
    }


    void FixedUpdate()
    {
        //step 3
        //integrate
        //updatePositionExplicitEuler(Time.fixedDeltaTime);

        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration = -Mathf.Sin(Time.fixedTime);


        //updateRotationEulerExplicit(Time.fixedDeltaTime);

        
        switch (myDropdown.value)
        {
            case 0:
                //ResetValues();
                updatePositionExplicitEuler(Time.fixedDeltaTime);
                break;
        
            case 1:
                //ResetValues(); 
                updatePositionKinematic(Time.fixedDeltaTime);
                break;
        }

        switch(myDrowpdown2.value)
        {
            case 0:
                updateRotationEulerExplicit(Time.fixedDeltaTime);
                break;

            case 1:
                updateRotationKinematic(Time.fixedDeltaTime);
                break;
        }

            //apply to transform
        transform.position = position;
        transform.Rotate(0, 0, rotation);
        //transform.eulerAngles = new Vector3(0, 0, rotation);

        //step4
        //test

        myArray[0].text = position.ToString("0.0000");
        myArray[1].text = velocity.ToString("0.0000");
        myArray[2].text = acceleration.ToString("0.0000");
        myArray[3].text = rotation.ToString("0.0000");
        myArray[4].text = angularVelocity.ToString("0.0000");
        myArray[5].text = angularAcceleration.ToString("0.0000");
    }



}
