/*
    aqui e feita a AI do patrol state do monstro
*/
using UnityEngine;

public class AiPatrolState : AiState{
    public int current; // waypoint em que se encontra
    public float distanceThreshold = 0.1f; // distancia entre cada pointinho de waypoint para nao ficar preso

    public AiStateId GetId(){
        return AiStateId.Patrol;
    }

    public void Enter(AiAgent agent){
        agent.animator.Play("Patrol");
        current = agent.startingPoint;
    }

    public void Exit(AiAgent agent){
        agent.startingPoint = current;
    }

    public void Update(AiAgent agent){
        // assim que chega ao ponto move-se se nao muda a posicao onde
        // teria que estar e move-se para a proxima posicao
        if (Vector3.Distance(agent.transform.position, agent.points[current].position) > distanceThreshold)
            agent.navMeshAgent.destination = agent.points[current].position;
        else current = (current + 1) % agent.points.Length;

        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentView)
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}