using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState_Multiplayer : MovementBaseState_Multiplayer
{
    public override void EnterState(MovementStateManager_Multiplayer movement)
    {
        movement.anim.SetBool("Running", true);
    }

    public override void UpdateState(MovementStateManager_Multiplayer movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.runBackSpeed;
        else movement.currentMoveSpeed = movement.runSpeed;
    }

    void ExitState(MovementStateManager_Multiplayer movement, MovementBaseState_Multiplayer state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
