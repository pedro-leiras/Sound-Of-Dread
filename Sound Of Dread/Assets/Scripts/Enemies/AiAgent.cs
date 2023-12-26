using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform playerTranform;
    public MovementStateManager player;
    public GameObject container;
    public Transform[] points;
    public CapsuleCollider agentCollider;
    public AudioSource source;
    public AudioSource sourceSteps;
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
    public AudioClip pointingClip;
    public float delayInSecondsPointing = 1f;
    public AudioClip stepsClip;

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

    public DoorController[] doors;

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.speed = agentSpeed;
        navMeshAgent.stoppingDistance = agentStoppingDistance;
        //nossa state machina que vai gerir os estados
        stateMachine = new AiStateMachine(this);
        if (container == null) container = GameObject.FindGameObjectWithTag("Container");
        // if (puc == null) puc = container.GetComponentInChildren<PickUpController>();
        if (agentCollider == null) agentCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CapsuleCollider>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementStateManager>();
        if (playerTranform == null) playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 1f;
        source.spatialBlend = 1.0f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.maxDistance = 10.0f;
        source.playOnAwake = false;

        sourceSteps = gameObject.AddComponent<AudioSource>();
        sourceSteps.volume = 1f;
        sourceSteps.spatialBlend = 1.0f;
        sourceSteps.rolloffMode = AudioRolloffMode.Linear;
        sourceSteps.maxDistance = 10.0f;
        sourceSteps.playOnAwake = false;
        sourceSteps.clip = stepsClip;
        //todos os estados sao registados aqui
        stateMachine.RegisterState(new AiStateChasePlayer());
        stateMachine.RegisterState(new AiStatePatrol());
        stateMachine.RegisterState(new AiStateAttack());
        stateMachine.RegisterState(new AiStateIdle());
        stateMachine.RegisterState(new AiStateChaseSound());
        stateMachine.RegisterState(new AiStateDead());
        stateMachine.RegisterState(new AiStatePoiting());
        //muda os estados conforme o pretendido
        stateMachine.ChangeState(initialState);
    }


    void Update(){
        stateMachine.Update();
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Object") && isAttackable){
            agentHP -= 10;
            if (agentHP < 1)
            {
                foreach (DoorController door in doors)
                {
                    if (door.doorID == 21)
                    {
                        door.lockStatus = 0;
                    }
                    if (door.doorID == 22)
                    {
                        door.lockStatus = 0;
                    }
                }
                isDead = true;
            }
        }
    }

    public void PlaySteps(){
        if (sourceSteps.isPlaying) sourceSteps.Play();
       // Debug.Log("Step");
    }
}
