using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform playerTranform;
    public PlayerController player;
    public Transform[] points;
    public CapsuleCollider agentCollider;
    public AudioSource source;
    public AudioClip patrolClip;
    public float delayInSecondsPatrol = 1f;
    public AudioClip chaseClip;
    public float delayInSecondsChase = 1f;
    public AudioClip attackClip;
    public float delayInSecondsAttack = 1f;
    public AudioClip idleClip;
    public float delayInSecondsIdle = 1f;
    public AudioClip deathClip;
    public float delayInSecondsDeath = 1f;

    //onde vai começar a patrolhar
    [HideInInspector]
    public int startingPoint = 0;
    [HideInInspector]
    public int transitionAnimation = Animator.StringToHash("Transition");
    [HideInInspector]
    public PickUpController puc;
    [HideInInspector]
    public bool isDead = false;

    [Header("Configs")]
    [SerializeField] public float agentSpeed = 1.0f;
    [SerializeField] public float agentStoppingDistance = 1.0f;
    [SerializeField] public float agentView = 10.0f;
    [SerializeField] public float timeForMonsterToStopLooking = 3.0f;
    [SerializeField] public int damage = 0;
    [SerializeField] public float idleTimer = 0.0f;
    [SerializeField] public float patrolSpeed = 0.0f;
    [SerializeField] public float chaseSpeed = 0.0f;
    [SerializeField] public float listeningArea = 100.0f;
    [SerializeField] public bool isAttackable = false;
    [SerializeField] public int agentHP = 100;

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.speed = agentSpeed;
        navMeshAgent.stoppingDistance = agentStoppingDistance;
        //nossa state machina que vai gerir os estados
        stateMachine = new AiStateMachine(this);
        if (agentCollider == null) agentCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CapsuleCollider>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerTranform == null) playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
        if (puc == null) puc = GameObject.FindGameObjectWithTag("Object").GetComponent<PickUpController>();
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.2f;
        //todos os estados sao registados aqui
        stateMachine.RegisterState(new AiStateChasePlayer());
        stateMachine.RegisterState(new AiStatePatrol());
        stateMachine.RegisterState(new AiStateAttack());
        stateMachine.RegisterState(new AiStateIdle());
        stateMachine.RegisterState(new AiStateChaseSound());
        stateMachine.RegisterState(new AiStateDead());
        //muda os estados conforme o pretendido
        stateMachine.ChangeState(initialState);
    }


    void Update(){
        stateMachine.Update();
    }

    private void OnCollisionEnter(Collision collision){
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag("Object") && isAttackable){
            agentHP -= 10;
            if (agentHP == 0) isDead = true;
        }
    }
}
