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





    private void Awake() {
        HideCursor();
        _currentMap = PlayerInput.currentActionMap;
        _moveAction = _currentMap.FindAction("Move");
        _lookAction = _currentMap.FindAction("Look");
        _runAction  = _currentMap.FindAction("Run");
        _crouchAction = _currentMap.FindAction("Crouch");

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
        Move = context.ReadValue<Vector2>();
    }
    private void onLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }
    private void onRun(InputAction.CallbackContext context)
    {
        Run = context.ReadValueAsButton();
    }

    private void onJump(InputAction.CallbackContext context)
    {
        Jump = context.ReadValueAsButton();
    }
    private void onCrouch(InputAction.CallbackContext context)
    {
        /*foreach (DoorController door in doors)
        {
            
            if (door.doorID == 1)
                door.lockStatus = 0;
            }
        }*/
        Crouch = context.ReadValueAsButton();
    }

    private void OnEnable() {
        _currentMap.Enable();
    }

    private void OnDisable() {
        _currentMap.Disable();
    }
}
