/*
    aqui e feita a AI do idle state do monstro
*/
using System.Linq;
using UnityEngine;

public class AiStateIdle : AiState{
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
        agent.source.clip = agent.idleClip;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        timer += Time.deltaTime;

        if (timer >= agent.animator.GetCurrentAnimatorStateInfo(0).length - 1.0f && timer >= agent.idleTimer && agent.points.Length != 0) agent.stateMachine.ChangeState(AiStateId.Patrol);

        // se a distancia entre o jogador e o enemy for a que ele consegue ver entao segue o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);

        // distancia do objecto entre a posicao do enemy e a colisao do objecto
        if (agent.puc != null && agent.puc.collisionPos != Vector3.zero && Vector3.Distance(agent.transform.position, agent.puc.collisionPos) < agent.listeningArea && agent.puc.isThrown){
            agent.stateMachine.ChangeState(AiStateId.ChaseSound);
            Debug.Log("object pos: " + agent.puc.collisionPos);
        }

        // se o enimigo morrer fica no estado morto
        if (agent.isDead) agent.stateMachine.ChangeState(AiStateId.Dead);

        if (!agent.source.isPlaying) agent.source.PlayScheduled(agent.delayInSecondsIdle);
    }
}
