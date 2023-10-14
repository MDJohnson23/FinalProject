using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    Text livesText;
    GameManager livesCount;

    void Start()
    {
        livesCount = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        livesText = GameObject.Find("Lives Text").GetComponent<Text>();
        livesText.text = livesCount.lives.ToString();

    }
}
