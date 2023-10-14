using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float turnSpeed = 20f;
    [SerializeField] float moveSpeed = 0.06f;
    [SerializeField] float animSpeed = 12f;
    
    [SerializeField] public bool hasKey = false;
    [SerializeField] public bool hasPickup = false;
    [SerializeField] GameObject pickupDecoy;
    
    Animator animator;
    Rigidbody rdbody;
    Vector3 movement;
    Quaternion rotation = Quaternion.identity;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        rdbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("IsWalking", isWalking);
        animator.SetFloat("AnimSpeed", animSpeed);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        rotation = Quaternion.LookRotation(desiredForward);
    }

    private void Update() 
    {
        if (hasPickup && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(pickupDecoy, transform.position, transform.rotation);
            hasPickup = false;
        }
    }

    void OnAnimatorMove()
    {
        rdbody.MovePosition(rdbody.position + movement * moveSpeed);
        rdbody.MoveRotation(rotation);
    }
}
