/*
    aqui e feita a AI do patrol state do monstro
*/
using UnityEngine;

public class AiStateAttack : AiState{
    private float timer = 0.0f;
    public AiStateId GetId(){
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent){
        timer = 0.0f;
        agent.transform.LookAt(agent.playerTranform.position);
        agent.animator.Play("Attack");
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        timer += Time.deltaTime;
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) > agent.agentStoppingDistance && timer > 4)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}
