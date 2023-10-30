/*
    aqui e feita a AI do idle state do monstro
*/
using UnityEngine;

public class AiIdleState : AiState{
    private float originalAnimationValue;
    private float timer;

    public AiStateId GetId(){
        return AiStateId.Idle;
    }

    public void Enter(AiAgent agent){
        // valor original do controlador para o ataque
        originalAnimationValue = 0.0f;
        // da o valor original para a transition da blend tree para 0 para ficar em idle
        agent.animator.SetFloat(agent.transitionAnimation, originalAnimationValue);
        timer = 0.0f;
        // enemy fica parado no sitio
        agent.agentSpeed = 0.0f;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        timer += Time.deltaTime;

        if (timer >= agent.animator.GetCurrentAnimatorStateInfo(0).length - 1.0f && timer >= agent.idleTimer) agent.stateMachine.ChangeState(AiStateId.Patrol);

        // se a distancia entre o jogador e o enemy for a que ele consegue ver entao segue o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}
