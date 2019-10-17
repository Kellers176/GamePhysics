using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public float rangeMin, rangeMax;
    float elapsedTime;
    public float timeInterval;
    float posX = 0, posY = 0;
    public Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        posX = Random.Range(rangeMin, rangeMax);
        posY = Random.Range(rangeMin, rangeMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

       
        this.GetComponent<Particle2D>().velocity.x = velocity.x + posX;
        this.GetComponent<Particle2D>().velocity.y = velocity.y + posY;


       
    }
}
