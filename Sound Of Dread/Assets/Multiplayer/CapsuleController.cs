using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CapsuleController : NetworkBehaviour
{
    [HideInInspector] public bool isDead = false;
    private float hzInput, vInput;
    private float currentMoveSpeed = 3;
    [SerializeField] private GameObject Camera;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Camera.SetActive(true);
        }
        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (!isDead)
        {
            GetDirectionAndMove();
        }
    }

    private void GetDirectionAndMove()
    {
        Vector3 dir = new Vector3 (0, 0, 0);

        hzInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;
        float speed = 3f;
        transform.position += dir * speed * Time.deltaTime;
    }
}
