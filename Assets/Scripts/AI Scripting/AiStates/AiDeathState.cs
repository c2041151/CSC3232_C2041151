using UnityEngine;

public class AiDeathState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        GameObject.Destroy(agent.gameObject);
    }

    public void Update(AiAgent agent)
    {
        Debug.Log("enemy is dead");
    }

    public void Exit(AiAgent agent)
    {

    }
}
