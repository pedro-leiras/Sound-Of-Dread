using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState_Multiplayer : MovementBaseState_Multiplayer
{
    private bool isCrouching = false;
    private bool crouchKeyPressed = false;

    public override void EnterState(MovementStateManager_Multiplayer movement)
    {
        movement.anim.SetBool("Crouching", true);
        isCrouching = true;
    }

    public override void UpdateState(MovementStateManager_Multiplayer movement)
    {
        crouchKeyPressed = Input.GetKey(KeyCode.LeftControl);

        if (!crouchKeyPressed)
        {
            if (isCrouching)
            {
                ExitState(movement, movement.Idle);
            }
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);

        if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
        else ExitState(movement, movement.Walk);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.crouchBackSpeed;
        else movement.currentMoveSpeed = movement.crouchSpeed;
    }

    void ExitState(MovementStateManager_Multiplayer movement, MovementBaseState_Multiplayer state)
    {
        if (crouchKeyPressed && !isCrouching)
        {
            movement.anim.SetBool("Crouching", true);
            isCrouching = true;
        }
        else if (!crouchKeyPressed && isCrouching)
        {
            movement.anim.SetBool("Crouching", false);
            isCrouching = false;
            movement.SwitchState(state);
        }
    }
}