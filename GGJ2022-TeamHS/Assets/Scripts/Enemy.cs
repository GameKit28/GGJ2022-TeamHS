using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public GameObject idleEnemy;
    public GameObject huntingEnemy;

    private AudioSource audioSource;

    public Transform[] enemyWaypoints;
    public bool reverseBackThroughOrder = false;
    public float waitTimeBeforeMoving = 1;

    private float currentWaitTime = 0;
    private NavMeshAgent agent;
    private EnemyDetection detect;
    private int waypointIndex;
    private bool reversing = false;
    private bool chasingPlayer;

    private bool IsAgentAtDestination => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        idleEnemy.SetActive(true);
        huntingEnemy.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        detect = GetComponent<EnemyDetection>();
        if (enemyWaypoints == null || enemyWaypoints.Length < 1 || enemyWaypoints[0] == null)
        {
            Debug.LogError("Enemies need more than 1 waypoint to work.");
            return;
        }
        transform.position = enemyWaypoints[waypointIndex].position;
        StartCoroutine(EnemyMoveCycle());
    }

    private void FixedUpdate()
    {
        if (detect == null) return;

        var player = detect.CanSeePlayer();
        if (player == null && chasingPlayer) 
        {
            chasingPlayer = false;
            idleEnemy.SetActive(true);
            huntingEnemy.SetActive(false);
            Debug.Log("RESETTING MOVEMENT");
            agent.SetDestination(enemyWaypoints[waypointIndex].position);
            StartCoroutine(EnemyMoveCycle());
            return; 
        }
        else if (player != null && !chasingPlayer)
        {
            chasingPlayer = true;
            idleEnemy.SetActive(false);
            huntingEnemy.SetActive(true);
            StopAllCoroutines();
            SetAudioPitch();
            if(!audioSource.isPlaying)
                audioSource.Play();
        }

        if (chasingPlayer)
        {
            agent.SetDestination(player.position);
        }

        if (player != null)
        {
            Debug.Log("SEES PLAYER");

            if (detect.IsNearPlayer())
            {
                Debug.Log("NEAR PLAYER");
                var playerHealth = player.GetComponent<PlayerHealthController>();
                playerHealth.KillPlayer();
            }
        }
    }

    private void SetAudioPitch() {
        float rPitch = Random.Range(0.8f, 1.35f);
        audioSource.pitch = rPitch;
    }

    private IEnumerator EnemyMoveCycle()
    {
        while (true)
        {
            yield return null;
            if (!IsAgentAtDestination) 
            {
                continue;
            }
            yield return new WaitForSeconds(waitTimeBeforeMoving);

            if ((!reversing && waypointIndex + 1 < enemyWaypoints.Length) || (reversing && waypointIndex - 1 >= 0))
            {
                waypointIndex = reversing ? waypointIndex - 1 : waypointIndex + 1;
                agent.SetDestination(enemyWaypoints[waypointIndex].position);
                continue;
            }

            if (reverseBackThroughOrder)
            {
                reversing = !reversing;
                waypointIndex = reversing ? waypointIndex - 1 : waypointIndex + 1;
            }
            else
            {
                waypointIndex = 0;
            }
            agent.SetDestination(enemyWaypoints[waypointIndex].position);
        }
    }
}
