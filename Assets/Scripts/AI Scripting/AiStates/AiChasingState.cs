using System.Linq;
using UnityEngine;
public class AiChasingState : AiState
{

    int waypointIndex = 0;
    public AiStateId GetId()
    {
        return AiStateId.Chasing;
    }

    public void Enter(AiAgent agent)
    {
        agent.animator.SetFloat("Speed", 2);

        if(!agent.waypoints.Any()){
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
        else{
            agent.navMeshAgent.SetDestination(agent.waypoints[0].position);
        }
    }

    public void Update(AiAgent agent)
    {

        //change animation based on distance to finish point, if not moving, stop walk animation
        if(Vector3.Distance(agent.waypoints[waypointIndex].position, agent.transform.position) <=3){
            if(waypointIndex>=agent.waypoints.Count-1){
                waypointIndex = 0;

            }
            else if (agent.waypoints.Count==1){
                agent.stateMachine.ChangeState(AiStateId.Idle);
            }
            else{
                waypointIndex++;
            }
            agent.navMeshAgent.SetDestination(agent.waypoints[waypointIndex].position);
            agent.animator.SetFloat("Speed", 0);
        }
        else{
            agent.animator.SetFloat("Speed", 1.5f);
        }

        //checks player visibility, if visible, enemy stops
        if (agent.playerInSights == true){
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }

    public void Exit(AiAgent agent)
    {

    }
}
