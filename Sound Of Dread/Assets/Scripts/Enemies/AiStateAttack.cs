/*
    aqui e feita a AI do patrol state do monstro
*/
using UnityEngine;

public class AiStateAttack : AiState{
    private float timer = 0.0f;
    private float originalAnimationValue;

    public AiStateId GetId(){
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent){
        timer = 0.0f;
        //assim que entra neste state o enemy olha para o player
        agent.transform.LookAt(agent.playerTranform.position);
        // valor original do controlador para o ataque
        originalAnimationValue = 8.0f; 
        agent.animator.SetFloat(agent.transitionAnimation, originalAnimationValue);
        // mantem a sua posicao enquanto ataca
        agent.agentSpeed = 0.0f;
        agent.navMeshAgent.destination = agent.transform.position;
        agent.source.clip = agent.attackClip;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        timer += Time.deltaTime;
        if (timer >= agent.animator.GetCurrentAnimatorStateInfo(0).length + 1.0f && !agent.player.isDead){
            // quando a animacao acabar e se o jogador nao estiver morto entao percorre
            if ((agent.player.currentHealth - agent.damage) > 0){
                // verifica se a vida do player menos o dano do enimigo e menor que 0 antes de atacar
                agent.player.Attack(agent.damage, agent.gameObject);
                // se a distancia entre o jogador e o enemy for a que ele consegue ver entao segue o player 
                if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) > agent.agentStoppingDistance)
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }
            else{
                // player morre
                agent.player.isDead = true;
                agent.player.currentHealth = 0;
            }

            // reset no timer
            timer = 0.0f;
        }

        // se o player estiver morto o enimigo volta a patrolhar para nao ficar a atacar no vacuo
        if(agent.player.isDead) agent.stateMachine.ChangeState(AiStateId.Idle);

        // se o enimigo morrer fica no estado morto
        if (agent.isDead) agent.stateMachine.ChangeState(AiStateId.Dead);

        if (!agent.source.isPlaying) agent.source.PlayScheduled(agent.delayInSecondsAttack);
    }
}
