using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public List<Transform> Waypoints;           //waypoints on the enemy "route"
    NavMeshAgent navMeshAgent;                  //the NavMeshAgent component reference
    public int WaypointIndex;                   //which waypoint the enemy is currently going to
    public bool playerInSights = false;         //if the player is seen
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.SetDestination(Waypoints[0].position);

        //checks player visibility, if visible, enemy stops
        if (playerInSights == true){
            navMeshAgent.SetDestination(gameObject.transform.position);
        }
    }
}