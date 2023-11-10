/*
    aqui e basicamente o controlador da AI onde se pode adicionar
    os varios tipos/ids de estados que o monstro vai ter
*/
public enum AiStateId{
    ChasePlayer, //seguir player
    Idle, //idle estado normal
    Attack, //atacar player
    Patrol,
    ChaseSound, //seguir o som
    Dead, //enemy morre
    Poiting
}

public interface AiState{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}