using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class AimStateManager : MonoBehaviour
{

    float xAxis, yAxis;
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    private PlayerControlls _inputManager;

    private float _xRotation;
    private float lastXRotation;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 50f;
    [SerializeField] private float MouseSensitivity = 21.9f;
    private Rigidbody _playerRigidbody;
    public MovementStateManager movement;
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = GetComponent<PlayerControlls>();
        _playerRigidbody = GetComponent<Rigidbody>();
        MovementStateManager movement = FindObjectOfType<MovementStateManager>();
    }

    // Update is called once per frame


    private void LateUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;
            Camera.rotation = CameraRoot.rotation;




            if (!movement.isDead)
            {
                _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
                _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);
                lastXRotation = _xRotation;
                Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
            }
            else
            {
                Camera.localRotation = Quaternion.Euler(lastXRotation, 0, 0);
            }
        }
        
    }
}
