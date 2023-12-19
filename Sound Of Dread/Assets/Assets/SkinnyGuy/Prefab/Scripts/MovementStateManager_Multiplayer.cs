using EasyRoads3Dv3;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovementStateManager_Multiplayer : NetworkBehaviour
{

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float currentMoveSpeed = 3;
    [HideInInspector] public float walkSpeed = 3, walkBackSpeed = 2;
    [HideInInspector] public float runSpeed = 7, runBackSpeed = 6;
    [HideInInspector] public float crouchSpeed = 1.5f, crouchBackSpeed = 1;
    [HideInInspector] private int currentHealth = 100;
    [HideInInspector] private int maxHealth = 100;
    [HideInInspector] private float regenDelay = 8f; // Time to wait for health regeneration ** mudei a regen delay para 8 para que estivesse mais balanceado com o dano do inimigo
    [HideInInspector] private int regenAmount = 1;   // Health regeneration per second
    [HideInInspector] private float lastDamageTime;
    [HideInInspector] private int _deathHash;

    public TimePuzzle timePuzzle;
    public LeverPuzzle leverPuzzle;
    public LPuzzle lPuzzle;
    public CheckpointManager checkpointManager;

    [SerializeField] private float groundYOffset;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private GameObject Camera;

    [SerializeField] private AudioSource FSAudioSource;
    [SerializeField] private AudioSource DamageAudioSource;

    enum FSMaterial
    {
        Wood, Gravel, Stone, Empty
    }

    [Header("Footsteps")]
    [SerializeField] private List<AudioClip> woodFS;
    [SerializeField] private List<AudioClip> gravelFS;
    [SerializeField] private List<AudioClip> stoneFS;

    [HideInInspector]  public Animator anim;
    MovementBaseState_Multiplayer currentState;
    public IdleState_Multiplayer Idle = new IdleState_Multiplayer();
    public WalkState_Multiplayer Walk = new WalkState_Multiplayer();
    public CrouchState_Multiplayer Crouch = new CrouchState_Multiplayer();
    public RunState_Multiplayer Run = new RunState_Multiplayer();

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _deathHash = Animator.StringToHash("Death");
        SwitchState(Idle);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Camera.SetActive(true);
            RespawnServerRpc(new Vector3(0f, 1f, 0f));
        }
        base.OnNetworkSpawn();
    }

    [ServerRpc]
    private void RespawnServerRpc(Vector3 pos)
    {
        RespawnClientRpc(OwnerClientId, pos);
    }

    [ClientRpc]
    private void RespawnClientRpc(ulong clientId, Vector3 pos)
    {
        var nt = GetComponent<ClientNetworkTransform>();
        //NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        //playerPrefab.transform.position = new Vector3(227.74f, 51.83f, -3.42f);
        //nt.Teleport(new Vector3(227.74f, 51.83f, -3.42f), transform.rotation, transform.localScale);
        nt.transform.position = pos;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (!isDead)
        {
            GetDirectionAndMove();
        }
        //Gravity();
        if (currentHealth != 100)
        {
            HandleHealth();
        }
        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);
        currentState.UpdateState(this);
    }

    private void GetDirectionAndMove()
    {
        hzInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        transform.position += dir * currentMoveSpeed * Time.deltaTime;
    }

    private void Gravity()
    {
        Vector3 velocity = new Vector3();
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        transform.position += velocity * Time.deltaTime;
    }

    private bool IsGrounded()
    {
        float capsuleHeight = GetComponent<CapsuleCollider>().height;
        float capsuleRadius = GetComponent<CapsuleCollider>().radius;

        Vector3 raycastOrigin = transform.position + Vector3.up * capsuleHeight * 0.5f;

        if (Physics.Raycast(raycastOrigin, Vector3.down, capsuleHeight * 0.6f, groundMask))
        {
            return true;
        }

        return false;
    }

    public void SwitchState(MovementBaseState_Multiplayer state)
    {
        currentState = state;
        currentState.EnterState(this);

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

    private IEnumerator RespawnAfterDelay(float delay, string levelName)
    {
        yield return new WaitForSeconds(delay);

        // Find the appropriate checkpoint for the current level
        Transform respawnPoint = GetRespawnPointForLevel(levelName);

        // Reset player's health and position
        currentHealth = maxHealth;
        //transform.position = respawnPoint.position;
        RespawnServerRpc(respawnPoint.position);
        isDead = false;
        anim.SetBool(_deathHash, false);

    }

    public void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
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

    private void PlayFootstep()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();
        switch (surface)
        {
            case FSMaterial.Wood:
                clip = woodFS[Random.Range(0, woodFS.Count)];
                break;
            case FSMaterial.Gravel:
                clip = gravelFS[Random.Range(0, gravelFS.Count)];
                break;
            case FSMaterial.Stone:
                clip = stoneFS[Random.Range(0, stoneFS.Count)];
                break;
            case FSMaterial.Empty:
                break;
            default:
                break;
        }

        if (surface != FSMaterial.Empty)
        {
            FSAudioSource.clip = clip;

            FSAudioSource.volume = Random.Range(0.5f, 0.8f);
            FSAudioSource.pitch = Random.Range(0.8f, 1.2f);
            FSAudioSource.Play();
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
}
