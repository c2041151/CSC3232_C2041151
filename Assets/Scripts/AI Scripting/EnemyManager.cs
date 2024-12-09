using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float health;
    public float initialHealth = 100f;
    AiAgent agent;

    void Start(){
        agent = GetComponent<AiAgent>();
        health = initialHealth;
    }
    public void takeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Kill();
        }
        else{
            StartCoroutine(realiseTimer());
        }
    }

    private void Kill(){
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

    IEnumerator realiseTimer(){
        yield return new WaitForSeconds(1);

        agent.stateMachine.ChangeState(AiStateId.Attack);
    }
}
