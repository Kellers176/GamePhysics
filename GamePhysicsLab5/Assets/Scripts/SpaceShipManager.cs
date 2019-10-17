using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceShipManager : MonoBehaviour
{

    Particle2D particle;
    public Slider greenBar;

    public float playerHealth;
    public TextMeshProUGUI text;
     
    // Start is called before the first frame update
    void Start()
    {
        particle = this.gameObject.GetComponent<Particle2D>();
        playerHealth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        greenBar.value = playerHealth;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //move up
            particle.MoveUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //movedown
            particle.MoveDown();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //rotate right
            particle.MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //rotate left
            particle.MoveLeft();
        }

        if(playerHealth < 0)
        {
            text.enabled = true;
        }
    }
}
