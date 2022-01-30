using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
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
            Debug.Log("RESETTING MOVEMENT");
            agent.SetDestination(enemyWaypoints[waypointIndex].position);
            StartCoroutine(EnemyMoveCycle());
            return; 
        }
        else if (player != null && !chasingPlayer)
        {
            chasingPlayer = true;
            StopAllCoroutines();
            agent.SetDestination(player.position);
        }

        if(player != null)
            Debug.Log("SEES PLAYER");

        if (detect.IsNearPlayer())
        {
            //TODO kill player
            Debug.Log("NEAR PLAYER");
        }
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

            if (waypointIndex + 1 < enemyWaypoints.Length || (reversing && waypointIndex - 1 >= 0))
            {
                waypointIndex = reversing ? waypointIndex - 1 : waypointIndex + 1;
                agent.SetDestination(enemyWaypoints[waypointIndex].position);
                continue;
            }

            if (reverseBackThroughOrder)
            {
                reversing = !reversing;
                continue;
            }
            else
            {
                waypointIndex = 0;
            }
            agent.SetDestination(enemyWaypoints[waypointIndex].position);
        }
    }
}
