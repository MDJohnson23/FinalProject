using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    Scene currentScene;

    private void Start() 
    {
        currentScene = SceneManager.GetActiveScene();
    }
    private void OnTriggerEnter(Collider other) 
    {
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }
}
