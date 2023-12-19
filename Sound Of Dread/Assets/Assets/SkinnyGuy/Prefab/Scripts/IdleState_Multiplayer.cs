using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Multiplayer : MovementBaseState_Multiplayer
{
    public override void EnterState(MovementStateManager_Multiplayer movement)
    {

    }

    public override void UpdateState(MovementStateManager_Multiplayer movement)
    {
        if(movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) movement.SwitchState(movement.Crouch);
    }
}
