using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(AiAgent agent)
    {
        agent.animator.SetFloat("Speed", 0);
    }
    public void Update(AiAgent agent)
    {
        Debug.Log("Enemy is idle");
        //checks player visibility, if visible, enemy stops
        if (agent.playerInSights == true){
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }

    public void Exit(AiAgent agent)
    {

    }
}
