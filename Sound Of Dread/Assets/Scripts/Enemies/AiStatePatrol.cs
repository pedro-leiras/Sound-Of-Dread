/*
    aqui e feita a AI do patrol state do monstro
*/
using UnityEngine;

public class AiStatePatrol : AiState{
    public int current; // waypoint em que se encontra
    public float distanceThreshold = 0.0f; // distancia entre cada pointinho de waypoint para nao ficar preso
    private float originalAnimationValue;

    public AiStateId GetId(){
        return AiStateId.Patrol;
    }

    public void Enter(AiAgent agent){ 
        // current toma valor do startingPoint que está. se no caso o enemy saiu do ponto 2 enquanto estava a patrulhar
        // para seguir o player ou para o que quer que seja ele volta a esse ponto para patrulhar
        current = agent.startingPoint;
        distanceThreshold = agent.agentStoppingDistance;
        // valor original do controlador para o ataque
        originalAnimationValue = 4.0f;
        agent.agentSpeed = agent.patrolSpeed;
    }

    public void Exit(AiAgent agent){
        // atribui aqui o starting point do ponto de patrulha ao qual ele esta neste momento
        agent.startingPoint = current;
    }

    public void Update(AiAgent agent)
    {
        // calculos para a animacao do enimigo ser mais smooth
        float currentValue = agent.animator.GetFloat(agent.transitionAnimation);
        float newValue = Mathf.Lerp(currentValue, originalAnimationValue, Time.deltaTime * 2.0f);

        agent.animator.SetFloat(agent.transitionAnimation, newValue);

        // assim que chega ao ponto move-se se nao muda a posicao onde
        // teria que estar e move-se para a proxima posicao
        if (Vector3.Distance(agent.transform.position, agent.points[current].position) > distanceThreshold)
            agent.navMeshAgent.destination = agent.points[current].position;
        else{
            current = (current + 1) % agent.points.Length;
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }

        // se a distancia entre o jogador e o enemy for a que ele consegue ver entao segue o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);

        // se a distancia entre o jogador e o enemy for a que ele consegue atacar entao ataca o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentStoppingDistance && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.Attack);
    }
}