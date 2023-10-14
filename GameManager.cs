using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameManager instance = null;
    public GameObject canvasPauseMenu;
    public GameObject canvasLives;
    public int lives = 9;

    bool hasPauseMenu = false;
    bool hasLivesCanvas = false;

    void Awake()
    {
        Time.timeScale = 1f;
        
        if (instance == null) 
        {
            instance = this;
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        if (!hasPauseMenu)
        {
            Instantiate(canvasPauseMenu);
            hasPauseMenu = true;
        }
        if(!hasLivesCanvas)
        {
            Instantiate(canvasLives);
            hasLivesCanvas = true;
        }
    }

    private void Update() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            lives = 9;
        }
    }
}
