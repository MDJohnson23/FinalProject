using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    // Change Enemy target. destroy decoy after few seconds.

    GameObject enemyObject;
    Enemy enemyScript;

    private void Awake() 
    {
        enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemyObject.GetComponent<Enemy>();
    }

    private void Update() 
    {
        enemyScript.target = this.transform;
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
