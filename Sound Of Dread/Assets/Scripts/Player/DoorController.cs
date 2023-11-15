using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int doorID;
    public BoxCollider coll;
    public Transform player, fpsCam;

    public float pickUpRange;
    private Animator animator;
    public int lockStatus = 1; // 0: Unlocked, 1: Locked

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            if (lockStatus == 0) { 
                CheckDoor();
            }
            else
            {
                //audio locked door
            }
            
        }

    }

    private void CheckDoor()
    {
        if (gameObject.tag == "door")
        {
            
            int stateValue = animator.GetInteger("State");

            if (stateValue == 0) 
            {
                animator.SetInteger("State", 2); // open door default -> open
                //play audio opening
            }
            else if (stateValue == 1) 
            {
                animator.SetInteger("State", 2); // open door closed-> open
                //play audio opening
            }
            else if (stateValue == 2) 
            {
                animator.SetInteger("State", 1); // close door
                //play audio closing door
            }
        }
    }

    public void OpenDoor()
    {
        animator.SetInteger("State", 2);
    }

    public void CloseDoor()
    {
        animator.SetInteger("State", 1);
    }

}