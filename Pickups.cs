using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    // Add to player a bool similar to key system.

    GameObject playerObject;
    Player playerScript;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.hasPickup = true;
            Destroy(gameObject);
        }
    }
}
