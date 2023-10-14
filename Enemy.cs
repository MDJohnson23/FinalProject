using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{    
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;
    [Range(0, 1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
    [SerializeField] GameObject gameOverText;

    Animator animator;
    NavMeshAgent navMeshAgent;
    public Transform target;
    Vector3 guardPosition;
    Vector3 nextPosition;
    GameObject gmObject;
    GameManager gmScript;

    AudioSource[] audioSourceArray;
    AudioSource footSteps;
    [SerializeField] AudioClip[] footStepArray;
    
    AudioSource voiceLines;
    [SerializeField] AudioClip[] voiceLineArray;

    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceWaypointArrival = Mathf.Infinity;
    int currentWaypointIndex = 0;
    float maxSpeed = 6f;
    float animSpeed = 1f;
    bool playerCaught = false;
    bool gameOverTextSpawned = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        guardPosition = transform.position;
        gmObject = GameObject.FindGameObjectWithTag("GameManager");
        gmScript = gmObject.GetComponent<GameManager>();
        audioSourceArray = GetComponents<AudioSource>();
        footSteps = audioSourceArray[0];
        voiceLines = audioSourceArray[1];
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (InAttackRangeOfPlayer())
        {
            PlayVoiceLines();
            ChasePlayer();
        }
        else if (timeSinceLastSawPlayer < suspicionTime)
        {
            SuspicionBehavior();
        }
        else
        {
            PatrolBehavior();
        }

        if(playerCaught)
        {
            StartCoroutine(GameOver());
        }

        UpdateAnimator();
        UpdateTimers();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("AnimSpeed", animSpeed);
        if (speed > 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else 
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceWaypointArrival += Time.deltaTime;
    }

    private void PatrolBehavior()
    {
        nextPosition = guardPosition;

        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceWaypointArrival = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        if (timeSinceWaypointArrival > waypointDwellTime)
        {
            MoveTo();
        }
    }

    private void MoveTo()
    {
        navMeshAgent.destination = nextPosition;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(patrolSpeedFraction);
        navMeshAgent.isStopped = false;
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehavior()
    {
        navMeshAgent.isStopped = true;
    }

    private void AttackBehavior()
    {
        timeSinceLastSawPlayer = 0;
    }

    private bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(patrolSpeedFraction * 2);
        transform.LookAt(target);
        animator.SetBool("IsWalking", true);
    }

    public void PlayVoiceLines()
    {
        voiceLines.clip = voiceLineArray[UnityEngine.Random.Range(0, voiceLineArray.Length)];
        voiceLines.Play();
        print(voiceLines.clip);
        //voiceLines.PlayOneShot(voiceLineArray[UnityEngine.Random.Range(0, voiceLineArray.Length)]);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            playerCaught = true;
        }   
    }

    public void FootStepSound()
    {
        footSteps.clip = footStepArray[UnityEngine.Random.Range(0, footStepArray.Length)];
        footSteps.Play();
    }

    IEnumerator GameOver()
    {
        Time.timeScale = 0;
        if (!gameOverTextSpawned)
        {
            Instantiate(gameOverText);
            gameOverTextSpawned = true;
            gmScript.lives--;
            print(gmScript.lives);
        }
        yield return new WaitForSecondsRealtime(3);
        if (gmScript.lives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else {SceneManager.LoadScene(0);}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
