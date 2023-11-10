/*
    aqui e feita a AI do pointing state do monstro
*/
using UnityEditor;
using UnityEngine;

public class AiStatePoiting : AiState{
    private float originalAnimationValue;

    public AiStateId GetId(){
        return AiStateId.Poiting;
    }

    [System.Obsolete]
    public void Enter(AiAgent agent){
        // valor original do controlador para o ataque
        originalAnimationValue = 12.0f;
        // mantem a sua posicao enquanto ataca
        agent.agentSpeed = 0.0f;
        agent.agentCollider.enabled = false;
        agent.navMeshAgent.Stop();
        agent.source.clip = agent.pointingClip;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        // calculos para a animacao do enemy ser mais smooth
        float currentValue = agent.animator.GetFloat(agent.transitionAnimation);
        float newValue = Mathf.Lerp(currentValue, originalAnimationValue, Time.deltaTime * 2.0f);

        agent.animator.SetFloat(agent.transitionAnimation, newValue);
    }
}
