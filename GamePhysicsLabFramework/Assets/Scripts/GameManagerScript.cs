using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public TextMeshProUGUI ballCount;
    public int PlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        ballCount.text = PlayerHealth.ToString();

        if(PlayerHealth <= 0)
        {
            SceneManager.LoadScene("YouLoseScene");
        }
    }
}
