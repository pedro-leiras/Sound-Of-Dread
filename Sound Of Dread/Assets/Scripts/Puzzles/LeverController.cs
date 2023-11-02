using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public int LeverID;
    public BoxCollider coll;
    public Transform player, fpsCam;

    public float pickUpRange;
    private Animator animator;
    public int lockStatus = 0; // 0: Can be used, 1: Can't be used
    public int leverState = 0; // 0: Deafult, 1: Closed, 2: Opened

    public DoorController[] doors;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            if (lockStatus == 0)
            {
                CheckLever();
                transform.parent.GetComponent<LeverPrefabController>().ChangeLayersToOutlinedAndRevert();
            }
            else
            {
                //locked lever
            }

        }

    }

    private void CheckLever()
    {
        if (gameObject.tag == "lever")
        {

            int stateValue = animator.GetInteger("State");

            if (stateValue == 0)
            {
                animator.SetInteger("State", 2); // lever up
                leverState = 2;
            }
            //play audio lever up

            else if (stateValue == 1)
            {
                animator.SetInteger("State", 2); // lever up
                leverState = 2;
                //play audio lever down
            }
            else if (stateValue == 2)
            {
                animator.SetInteger("State", 1); // lever down
                leverState = 1;
                //play audio lever down
            }
        }


    }

    
}
