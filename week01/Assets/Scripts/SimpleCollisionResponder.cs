using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCollisionResponder : MonoBehaviour
{
    Rigidbody rb;
    int direction;

    Renderer rend;
    Color newColor;
    float r, g, b;

    int timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Random.Range(0, 2);
        r = Random.Range(0, 2);
        g = Random.Range(0, 2);
        b = Random.Range(0, 2);
        newColor = new Vector4(r, g, b, 1);
        
        timer++;
        //rb.AddExplosionForce(100f, gameObject.transform.position, 100f);   
        //rb.AddExplosionForce(100f, gameObject.transform.position, 100f);   
        //rb.AddExplosionForce(100f, gameObject.transform.position, 20f);   
        //rb.AddForce(Vector3.left, ForceMode.Impulse);
        //rb.AddForce(Vector3.right, ForceMode.Impulse);
        this.transform.localScale *= 1.01f;
        if(direction == 0)
        {
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.back, ForceMode.Impulse);
        }


        if(timer >= 120)
        {
            Destroy(this.gameObject);
        }
        //rb.AddForce(Vector3.back, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rend.material.color = newColor;
    }

}
