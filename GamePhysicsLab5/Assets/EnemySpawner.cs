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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > timeInterval)
        {
            prefab = Instantiate(prefab, GetComponent<Transform>().position, Quaternion.Euler(0, 0, 0));

            posX = Random.Range(rangeMin, rangeMax);
            posY = Random.Range(rangeMin, rangeMax);
        }

        prefab.GetComponent<Particle2D>().velocity.x = velocity.x + posX;
        prefab.GetComponent<Particle2D>().velocity.y = velocity.y + posY;

        posX = 0;
        posY = 0;
        elapsedTime = 0.0f;
    }
}
