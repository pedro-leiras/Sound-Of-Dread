using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform playerTranform;
    public Transform[] points;
    public int startingPoint = 0;

    [Header("Configs")]
    [SerializeField] public float agentSpeed = 1.0f;
    [SerializeField] public float agentStoppingDistance = 1.0f;
    [SerializeField] public float agentView = 10.0f;
    [SerializeField] public float timeForMonsterToStopLooking = 3.0f;

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.speed = agentSpeed;
        navMeshAgent.stoppingDistance = agentStoppingDistance;
        //nossa state machina que vai gerir os estados
        stateMachine = new AiStateMachine(this);
        if (playerTranform == null) playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
        //todos os estados sao registados aqui
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.RegisterState(new AiStateAttack());
        //muda os estados conforme o pretendido
        stateMachine.ChangeState(initialState);
    }


    void Update(){
        stateMachine.Update();
    }
}
