using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPuzzle : MonoBehaviour
{
    // Assuming you have five cubes with unique IDs
    public int cubeId;
    public string notePrefix = "letterDisplay";
    public DoorController[] doors;

    public LPuzzle lPuzzle;


    private void Awake()
    {
        lPuzzle = FindObjectOfType<LPuzzle>();
    }

    public void Update()
    {
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
                lPuzzle.letters += 1;
            }
        }
        else if (other.gameObject.name == "letterO" && cubeId == 1)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "O");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                lPuzzle.letters += 1;
            }
        }
        else if (other.gameObject.name == "letterU" && cubeId == 2)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "U");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                lPuzzle.letters += 1;
            }
        }
        else if (other.gameObject.name == "letterN" && cubeId == 3)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "N");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                lPuzzle.letters += 1;
            }
        }
        else if (other.gameObject.name == "letterD" && cubeId == 4)
        {
            Destroy(other.gameObject);
            Transform letter = transform.Find(notePrefix + "D");
            if (letter != null)
            {
                letter.gameObject.SetActive(true);
                lPuzzle.letters += 1;
            }
        }
    }
}