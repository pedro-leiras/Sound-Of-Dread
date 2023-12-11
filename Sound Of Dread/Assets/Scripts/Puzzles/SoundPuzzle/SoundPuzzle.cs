using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPuzzle : MonoBehaviour
{

    public int ButtonID;
    public BoxCollider coll;
    public Camera fpsCam;
    public float interactionDistance = 5f;
    public int lockStatus = 0;
    private bool isPlaying = false;
    public int[] correctSequence = { 1,1,3,3,2,4,2,1,1,1,3,3,3,1};
    private List<int> playerInputSequence = new List<int>();
    public bool Level4Finish = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (lockStatus == 0)
            {
                if(Level4Finish == false)
                {
                    TryInteractWithButton();
                }
                
            }
            else
            {
                // Locked button logic here.
            }
        }
    }

    private void TryInteractWithButton()
    {
        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            SoundPuzzle button = hit.collider.GetComponent<SoundPuzzle>();
            if (button != null && !isPlaying)
            {
                AudioSource cubeAudioSource = button.GetComponent<AudioSource>();
                
                
                if (cubeAudioSource != null)
                {
                    cubeAudioSource.Play();
                }
                if (button.ButtonID != 0)
                {


                    if (button.ButtonID == correctSequence[playerInputSequence.Count])
                    {
                        playerInputSequence.Add(button.ButtonID);

                        transform.parent.GetComponent<NoteManager>().ShowNotes(playerInputSequence.Count);

                        if (playerInputSequence.Count == correctSequence.Length)
                        {
                            Debug.Log("You win!");
                            Level4Finish = true;
                            playerInputSequence.Clear();
                        }
                    }
                    else
                    {
                        Debug.Log("You failed!");
                        playerInputSequence.Clear();
                    }
                }

            }

        }
        Debug.Log("Player Input Sequence: " + string.Join(", ", playerInputSequence));
    }
}


