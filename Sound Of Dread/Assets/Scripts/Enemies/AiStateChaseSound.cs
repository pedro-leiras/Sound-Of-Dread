/*
    aqui e feita a AI do chase sound state do monstro
*/
using UnityEngine;

public class AiStateChaseSound : AiState{
    private float originalAnimationValue;

    public AiStateId GetId(){
        return AiStateId.ChaseSound;
    }

    public void Enter(AiAgent agent){
        // valor original do controlador para o ataque
        originalAnimationValue = 4.0f;
        agent.agentSpeed = agent.patrolSpeed;
        Debug.Log("State Chase Sound");
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent)
    {
        // calculos para a animacao do enimigo ser mais smooth
        float currentValue = agent.animator.GetFloat(agent.transitionAnimation);
        float newValue = Mathf.Lerp(currentValue, originalAnimationValue, Time.deltaTime * 2.0f);

        agent.animator.SetFloat(agent.transitionAnimation, newValue);

        // assim que chega ao ponto para e procura se tem alguem, se nao tiver vai patrolhar logo asseguir onde parou
        if (agent.puc.collisionPos != Vector3.zero && Vector3.Distance(agent.transform.position, agent.puc.collisionPos) < agent.listeningArea && agent.puc.isThrown){
            agent.navMeshAgent.destination = agent.puc.collisionPos;

            if (Vector3.Distance(agent.transform.position, agent.puc.collisionPos) < agent.agentStoppingDistance + 1.0f){
                agent.puc.collisionPos = Vector3.zero;
                agent.stateMachine.ChangeState(AiStateId.Idle);
            }

        } else
            agent.stateMachine.ChangeState(AiStateId.Idle);

        // se a distancia entre o jogador e o enemy for a que ele consegue ver entao segue o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);

        // se a distancia entre o jogador e o enemy for a que ele consegue atacar entao ataca o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentStoppingDistance && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.Attack);
    }
}