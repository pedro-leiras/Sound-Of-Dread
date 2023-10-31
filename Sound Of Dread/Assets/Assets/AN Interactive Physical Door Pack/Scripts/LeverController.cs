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
    public int lockStatus = 1; // 0: Down, 1: Up

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
            }
            else
            {
                //locked lever
            }

        }

    }

    private void CheckLever()
    {
        Debug.Log("lever");
        if (gameObject.tag == "lever")
        {

            int stateValue = animator.GetInteger("State");

            if (stateValue == 0)
            {
                animator.SetInteger("State", 2); // lever up

                foreach (DoorController door in doors)
                {

                    if (door.doorID == 1)
                        door.lockStatus = 0;
                }
            }
            //play audio lever up

            else if (stateValue == 1)
            {
                animator.SetInteger("State", 2); // lever up


                foreach (DoorController door in doors)
                {

                    if (door.doorID == 1)
                        door.lockStatus = 0;
                }
                //play audio lever down
            }
            else if (stateValue == 2)
            {
                animator.SetInteger("State", 1); // lever down

                foreach (DoorController door in doors)
                {

                    if (door.doorID == 1)
                        door.lockStatus = 1;
                }
                //play audio lever down
            }
        }

    }
}
