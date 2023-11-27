using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    public Vector2 Look {get; private set;}
    private InputActionMap _currentMap;
    private InputAction _lookAction;
    private void Awake() 
    {
        HideCursor();
        _currentMap = PlayerInput.currentActionMap;
        _lookAction = _currentMap.FindAction("Look");
        _lookAction.performed += onLook;
        _lookAction.canceled += onLook;
    }
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void onLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }
    private void OnEnable() 
    {
        _currentMap.Enable();
    }
    private void OnDisable()
    {
        _currentMap.Disable();
    }
}
