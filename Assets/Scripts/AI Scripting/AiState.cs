using System.Collections;
using UnityEngine;

public enum AiStateId{
    Idle,
    Death,
    Chasing,
    Attack
}

public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);

}
