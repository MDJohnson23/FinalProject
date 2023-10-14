using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    GameObject playerObject;
    Player playerScript;
    
    //Navmesh

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && playerScript.hasKey == true)
        {
            Destroy(this.gameObject);
            playerScript.hasKey = false;
        }
    }
}
