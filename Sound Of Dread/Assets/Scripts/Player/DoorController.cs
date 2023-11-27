using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int doorID;
    public BoxCollider coll;
    public Transform player;
    public Camera fpsCam;
    public float interactionDistance = 3f;
    private Animator animator;
    public int lockStatus = 1; // 0: Unlocked, 1: Locked
    public AudioClip openDoor;
    public AudioClip closeDoor;
    public AudioClip lockedDoor;
    private AudioSource audioSource;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.gameObject.tag == "door")
                {
                    DoorController doorController = hit.collider.gameObject.GetComponent<DoorController>();
                    if (lockStatus == 0)
                    {
                        CheckDoor(doorController);
                    }
                    else
                    {
                        //audio locked door
                        audioSource.clip = lockedDoor;
                        audioSource.Play();
                    }
                }
            }
        }
    }

    private void CheckDoor(DoorController doorController)
    {
        if (doorID == doorController.doorID)
        {
            int stateValue = animator.GetInteger("State");

            if (stateValue == 0)
            {
                animator.SetInteger("State", 2); // open door default -> open
            }
            else if (stateValue == 1)
            {
                animator.SetInteger("State", 2); // open door closed-> open
            }
            else if (stateValue == 2)
            {
                animator.SetInteger("State", 1); // close door
            }
        }

    }

    public void OpenDoor()
    {
        animator.SetInteger("State", 2);
        StartCoroutine(CloseDoorAfterDelay(3f));
    }

    public void CloseDoor()
    {
        animator.SetInteger("State", 1);
    }

    private IEnumerator CloseDoorAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        CloseDoor();

    }

    public void OpenChurchDoor()
    {
        animator.SetInteger("State", 2);
        lockStatus = 1;
    }

    public void PlayOpenDoorAudio()
    {
        audioSource.clip = openDoor;
        audioSource.Play();
    }

    public void PlayCloseDoorAudio()
    {
        audioSource.clip = closeDoor;
        audioSource.Play();
    }
}