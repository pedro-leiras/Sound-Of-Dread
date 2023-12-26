using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MovementStateManager : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    public float currentMoveSpeed = 3;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 6;
    public float crouchSpeed = 1.5f, crouchBackSpeed = 1;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;

    CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();

    public bool isDead;
    public int currentHealth;
    public int maxHealth = 100;
    private float regenDelay = 8f; // Time to wait for health regeneration ** mudei a regen delay para 8 para que estivesse mais balanceado com o dano do inimigo
    private int regenAmount = 1;   // Health regeneration per second
    private float lastDamageTime;

    public AudioSource FSAudioSource;
    public AudioSource DamageAudioSource;

    enum FSMaterial
    {
        Wood, Gravel, Stone, Empty
    }

    [Header("Footsteps")]
    public List<AudioClip> woodFS;
    public List<AudioClip> gravelFS;
    public List<AudioClip> stoneFS;

    private int _deathHash;
    public TimePuzzle timePuzzle;
    public LeverPuzzle leverPuzzle;
    public LPuzzle lPuzzle;
    public CheckpointManager checkpointManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
        currentHealth = 100;
        isDead = false;
        _deathHash = Animator.StringToHash("Death");

        TimePuzzle timePuzzle = FindObjectOfType<TimePuzzle>();
        LeverPuzzle leverPuzzle = FindObjectOfType<LeverPuzzle>();
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            GetDirectionAndMove();
        }
        Gravity();

        if (currentHealth != 100)
        {
            HandleHealth();
        }
        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);
        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);

    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        controller.Move(dir.normalized * currentMoveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Attack(int damage, GameObject Sender)
    {
        if (currentHealth > 0)
        {
            DamageAudioSource.Play();
            currentHealth -= damage;

            //damage vfx
            bl_DamageInfo info = new bl_DamageInfo(damage);
            info.Sender = Sender;
            bl_DamageDelegate.OnDamageEvent(info);

            currentHealth = Mathf.Max(currentHealth, 0);
            lastDamageTime = Time.time;
        }
        else if (currentHealth <= 0)
        {
            HandleHealth();
        }
    }

    public void HandleHealth()
    {
        //Regenerate health
        if (currentHealth > 1)
        {
            if (Time.time - lastDamageTime >= regenDelay)
            {
                RegenerateHealth();
            }
        }
        else if (currentHealth < 1)
        {

            isDead = true;
            anim.SetBool(_deathHash, true);

            //Gets what current leven we are on
            string currentLevel = GetCurrentLevelName();

            //Respawn
            StartCoroutine(RespawnAfterDelay(10f, currentLevel));

        }
    }
    public void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }
    
    private FSMaterial SurfaceSelect()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);

        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Wood Floor"))
            {
                return FSMaterial.Wood;
            }
            if (hit.collider.CompareTag("Gravel Floor"))
            {
                return FSMaterial.Gravel;
            }
            if (hit.collider.CompareTag("Stone Floor"))
            {
                return FSMaterial.Stone;
            }
        }

        return FSMaterial.Empty;
    }

    private void PlayFootstep()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();
        switch (surface)
        {
            case FSMaterial.Wood:
                clip = woodFS[UnityEngine.Random.Range(0, woodFS.Count)];
                break;
            case FSMaterial.Gravel:
                clip = gravelFS[UnityEngine.Random.Range(0, gravelFS.Count)];
                break;
            case FSMaterial.Stone:
                clip = stoneFS[UnityEngine.Random.Range(0, stoneFS.Count)];
                break;
            case FSMaterial.Empty:
                break;
            default:
                break;
        }

        if (surface != FSMaterial.Empty)
        {
            FSAudioSource.clip = clip;

            FSAudioSource.volume = UnityEngine.Random.Range(0.5f, 0.8f);
            FSAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            FSAudioSource.Play();
        }
    }

    private IEnumerator RespawnAfterDelay(float delay, string levelName)
    {
        yield return new WaitForSeconds(delay);

        // Find the appropriate checkpoint for the current level
        Transform respawnPoint = GetRespawnPointForLevel(levelName);

        // Reset player's health and position
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        isDead = false;
        anim.SetBool(_deathHash, false);

    }
   
    private Transform GetRespawnPointForLevel(string levelName)
    {
        foreach (var checkpoint in CheckpointManager.instance.checkpoints)
        {
            if (checkpoint.levelName == levelName)
            {
                return checkpoint.spawnPoint;
            }
        }


        return transform;
    }
    
    private string GetCurrentLevelName()
    {
        // Determine the current level
        if (timePuzzle.Level2Finish)
        {
            return "Level2";
        }
        else if (leverPuzzle.Level1Finish)
        {
            return "Level1";
        }
        else if (lPuzzle.Level3Finish)
        {
            return "Level3";
        }

        return "Checkpoint1";
    }
}
