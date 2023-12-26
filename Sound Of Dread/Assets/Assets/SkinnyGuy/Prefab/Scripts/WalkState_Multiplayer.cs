using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState_Multiplayer : MovementBaseState_Multiplayer
{
    public override void EnterState(MovementStateManager_Multiplayer movement)
    {
        movement.anim.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager_Multiplayer movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if (Input.GetKeyDown(KeyCode.LeftControl)) ExitState(movement, movement.Crouch);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.walkBackSpeed;
        else movement.currentMoveSpeed = movement.walkSpeed;
    }

    void ExitState(MovementStateManager_Multiplayer movement, MovementBaseState_Multiplayer state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
