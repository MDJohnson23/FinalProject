using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    GameObject playerObject;
    Player playerScript;

    // add bool to player script hasKey as false. change to true ontrigger in this script
    // Then in door script check bool hasKey on player. open door if true.

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.hasKey = true;
            Destroy(gameObject);
        }
    }
}
