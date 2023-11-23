/*
    aqui e feita a AI do pointing state do monstro
*/
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
        agent.source.clip = agent.pointingClip;
        agent.animator.SetFloat(agent.transitionAnimation, originalAnimationValue);
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        
    }
}
