/*
    aqui e feita a AI do chase player state do monstro
*/
using UnityEngine;

public class AiStateChasePlayer : AiState{
    float timer = 0.0f;
    float wallTimer = 0.0f;
    bool isBehindWall = false;
    private float originalAnimationValue;

    public AiStateId GetId(){
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent){
        // timer para que o monstro veja o player
        timer = agent.timeForMonsterToStopLooking;
        // timer para verificar se o player esta atras de uma parede
        wallTimer = 1.0f;
        // valor original do controlador para o ataque
        originalAnimationValue = 6.0f;
        agent.agentSpeed = agent.chaseSpeed;
        //agent.navMeshAgent.updateRotation = false;
        agent.source.clip = agent.chaseClip;
    }

    public void Exit(AiAgent agent){

    }

    public void Update(AiAgent agent){
        //agent.transform.rotation = Quaternion.LookRotation(agent.navMeshAgent.velocity.normalized);
        // calculos para a animacao do enimigo ser mais smooth
        float currentValue = agent.animator.GetFloat(agent.transitionAnimation);
        float newValue = Mathf.Lerp(currentValue, originalAnimationValue, Time.deltaTime * 2.0f);

        agent.animator.SetFloat(agent.transitionAnimation, newValue);

        if (!agent.navMeshAgent.enabled) return;

        agent.PlaySteps();

        bool canSeePlayer = CanSeePlayer(agent);
        if (canSeePlayer) timer = agent.timeForMonsterToStopLooking; // tempo que o inimigo segue desde o momento que nao consegue ver o player
        else timer -= Time.deltaTime;

        wallTimer -= Time.deltaTime;
        if (wallTimer <= 0.0f){
            isBehindWall = IsBehindWall(agent);
            wallTimer = 1.0f; // verifica se esta alguma parede/objeto a frente a cada segundo para ativar o ticker se segue ou nao
        }

        // se nao esta atras da parede simplesmente segue
        if (!isBehindWall) agent.navMeshAgent.destination = agent.playerTranform.position;

        // se a distancia for a pretendida, ou seja se o AI consegue ver o jogador e se estiver no range para tal
        // entao diminuir o timer
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) >= agent.agentView && canSeePlayer && !agent.player.isDead)
            timer -= Time.deltaTime;

        if (timer < 0.0f) 
            if(agent.points.Length > 0)
                agent.stateMachine.ChangeState(AiStateId.Patrol); // assim que o timer chegar a 0 o monstro para de se mover e fica no seu estado patrol
            else agent.stateMachine.ChangeState(AiStateId.Idle);

        // se a distancia entre o jogador e o enemy for a que ele consegue atacar entao ataca o player 
        if (Vector3.Distance(agent.transform.position, agent.playerTranform.position) < agent.agentStoppingDistance && !agent.player.isDead)
            agent.stateMachine.ChangeState(AiStateId.Attack);

        // se o enimigo morrer fica no estado morto
        if (agent.isDead) agent.stateMachine.ChangeState(AiStateId.Dead);

        if (!agent.source.isPlaying) agent.source.PlayScheduled(agent.delayInSecondsChase);
    
        IsCloseToDoor(agent);
    }

    private static bool CanSeePlayer(AiAgent agent){
        /*
            implementa basicamente um raycast/linha de visao para ver se o inimigo consegue ver o jogador
            retorna verdadeiro se o inimigo consegue ver o jogador e falso se o contrario
        */
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, agent.playerTranform.position - agent.transform.position, out hit)
            && Vector3.Distance(agent.transform.position, agent.playerTranform.position) >= agent.agentView)
            if (hit.collider.CompareTag("Player")) return true;
        return false;

    }

    private static bool IsBehindWall(AiAgent agent){
        /*
            implementa basicamente um raycast/linha de visao para ver se o inimigo consegue ver o jogador
            retorna verdadeiro se o jogador estiver atras da parede ou falso se o contrario
        */
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, agent.playerTranform.position - agent.transform.position, out hit)
            && Vector3.Distance(agent.transform.position, agent.playerTranform.position) >= agent.agentView)
            if (hit.collider.CompareTag("Wall")) return true;
        return false;
    }

    private bool IsCloseToDoor(AiAgent agent){
        Ray ray = new Ray(agent.transform.position, agent.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, agent.agentStoppingDistance) && hit.collider.CompareTag("door")){
            if(hit.collider && agent.agentCollider){
                Physics.IgnoreCollision(hit.collider, agent.agentCollider);
                if(hit.collider.gameObject.GetComponent<DoorController>().lockStatus == 0)
                    hit.collider.gameObject.GetComponent<DoorController>().OpenDoor();
                return true;
            }
        } 
        
        return false;
    }
}
