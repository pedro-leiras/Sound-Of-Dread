using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;


    public Vector2 Move {get; private set;}
    public Vector2 Look {get; private set;}
    public bool Run {get; private set;}
    public bool Jump {get; private set;}
    public bool Crouch {get; private set;}

    [Header("Input Manager Parameters")]
    private InputActionMap _currentMap;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _runAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;

    public PlayerController playerController;





    private void Awake() {
        HideCursor();
        _currentMap = PlayerInput.currentActionMap;
        _moveAction = _currentMap.FindAction("Move");
        _lookAction = _currentMap.FindAction("Look");
        _runAction  = _currentMap.FindAction("Run");
        _crouchAction = _currentMap.FindAction("Crouch");
        playerController = FindObjectOfType<PlayerController>();

        _moveAction.performed += onMove;
        _lookAction.performed += onLook;
        _runAction.performed += onRun;
        _crouchAction.started += onCrouch;

        _moveAction.canceled += onMove;
        _lookAction.canceled += onLook;
        _runAction.canceled += onRun;
        _crouchAction.canceled += onCrouch;
    }
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void onMove(InputAction.CallbackContext context)
    {
        if(playerController.isDead == false) {
            Move = context.ReadValue<Vector2>();
        }
    }
    private void onLook(InputAction.CallbackContext context)
    {


            Look = context.ReadValue<Vector2>();

    }
    private void onRun(InputAction.CallbackContext context)
    {
            if (playerController.isDead == false)
            {
                Run = context.ReadValueAsButton();
            }
    }
    private void onCrouch(InputAction.CallbackContext context)
    {
                if (playerController.isDead == false)
                {
                    Crouch = context.ReadValueAsButton();
                }
    }   

    private void OnEnable() {
        _currentMap.Enable();
    }

    private void OnDisable() {
        _currentMap.Disable();
    }

}
