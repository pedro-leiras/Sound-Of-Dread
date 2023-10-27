using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [Header("Changable Parameters")]
    [SerializeField] private float AnimBlendSpeed = 8.9f;
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 50f;
    [SerializeField] private float MouseSensitivity = 21.9f;


    private Camera playerCamera;
    [Header("Import Parameters")]
    private Rigidbody _playerRigidbody;
    private InputManager _inputManager;

    [Header("Animator Parameters")]
    private Animator _animator;
    private bool _hasAnimator;
    private int _xVelHash;
    private int _yVelHash;
    private int _groundHash;
    private int _fallingHash;
    private int _zVelHash;
    private int _crouchHash;
    private float _xRotation;

    [Header("Speed Parameters")]
    private const float _walkSpeed = 2f;
    private const float _runSpeed = 6f;
    private Vector2 _currentVelocity;

    [Header("Stamina Parameters")]
    public float maxStamina = 100.0f;
    public float currentStamina = 100.0f;
    public float staminaDepletionRate = 20.0f;
    public float staminaRegenRate = 10.0f;
    float scaleFactor = 3.0f;

    [Header("Health Parameters")]
    private int maxHealth = 100;
    public int currentHealth;
    public bool isDead; // se o player estiver morto = true o contrario = false
    private float regenDelay = 8f; // Time to wait for health regeneration ** mudei a regen delay para 8 para que estivesse mais balanceado com o dano do inimigo
    private int regenAmount = 1;   // Health regeneration per second
    private float lastDamageTime;




    private void Start()
    {
        
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();


        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _crouchHash = Animator.StringToHash("Crouch");

        currentHealth = maxHealth;
        playerCamera = GetComponentInChildren<Camera>();

        isDead = false; // inicializar o player como vivo
    }

    private void FixedUpdate()
    {
        Move();
        HandleCrouch();
              
        if (Time.time - lastDamageTime >= regenDelay)
        {
            HandleHealth();
        }
    }
    private void LateUpdate()
    {
        CamMovements();
    }
    private void Move()
    {
        if (!_hasAnimator) return;
        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if (currentStamina == 0)
        {
            targetSpeed = _walkSpeed;
        }
        if (targetSpeed == _runSpeed)
        {
            float staminaDepletion = staminaDepletionRate * Time.deltaTime * scaleFactor;
            currentStamina = Mathf.Max(0, currentStamina - staminaDepletion);
        }
        else
        {
            float staminaRegen = staminaRegenRate * Time.deltaTime * scaleFactor;
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegen);
        }
        if (_inputManager.Crouch) targetSpeed = 1.5f;
        if (_inputManager.Move == Vector2.zero) targetSpeed = 0;






        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
        var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

        _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);



        _animator.SetFloat(_xVelHash, _currentVelocity.x);
        _animator.SetFloat(_yVelHash, _currentVelocity.y);
    }



    private void CamMovements()
    {
        if (!_hasAnimator) return;

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        Camera.position = CameraRoot.position;
        Camera.rotation = CameraRoot.rotation;


        _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void HandleCrouch()
    {
        _animator.SetBool(_crouchHash, _inputManager.Crouch);
    }

    public void Attack(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            lastDamageTime = Time.time;
        }
        else if (currentHealth <= 0)
        {
            //Player morreu
            }
    }

    public void HandleHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

}
