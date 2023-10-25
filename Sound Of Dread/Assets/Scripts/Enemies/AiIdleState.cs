/*
    aqui e feita a AI do idle state do monstro
*/
using UnityEngine;

public class AiStateIdle : AiState{
    private float timer = 0.0f;
    private bool animationStarted = false;

    public AiStateId GetId(){
        return AiStateId.Idle;
    }

    public void Enter(AiAgent agent){
        timer = 0.0f;
        agent.animator.Play("Idle");
        animationStarted = true;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        timer += Time.deltaTime;
        if (animationStarted && timer >= agent.animator.GetCurrentAnimatorStateInfo(0).length - 1.0f) agent.stateMachine.ChangeState(AiStateId.Patrol);

        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}