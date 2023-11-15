using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPuzzle : MonoBehaviour
{
    // Assuming you have five cubes with unique IDs
    public int cubeId;
    public string notePrefix = "letterDisplay";
    [SerializeField] private bool firstLetter = false;
    [SerializeField] private bool secondLetter = false;
    [SerializeField] private bool thirdLetter = false;
    [SerializeField] private bool fourthLetter = false;
    [SerializeField] private bool fifthLetter = false;
    private bool letterPuzzleComplete = false;
    public DoorController[] doors;

    public void Update()
    {
        if (letterPuzzleComplete == false)
        {
            if (firstLetter && secondLetter && thirdLetter && fourthLetter && fifthLetter)
            {
                letterPuzzleComplete = true;
                foreach (DoorController door in doors)
                {
                    if (door.doorID == 20)
                    {
                        door.lockStatus = 0;
                        door.OpenDoor();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "letterS" && cubeId == 0)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "S");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                firstLetter = true;
            }
        }
        else if (other.gameObject.name == "letterO" && cubeId == 1)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "O");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                secondLetter = true;
            }
        }
        else if (other.gameObject.name == "letterU" && cubeId == 2)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "U");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                thirdLetter = true;
            }
        }
        else if (other.gameObject.name == "letterN" && cubeId == 3)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "N");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                fourthLetter = true;
            }
        }
        else if (other.gameObject.name == "letterD" && cubeId == 4)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "D");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                fifthLetter = true;
            }
        }
    }
}