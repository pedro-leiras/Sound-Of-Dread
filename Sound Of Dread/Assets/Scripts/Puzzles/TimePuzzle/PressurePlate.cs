using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressurePlate : MonoBehaviour
{
    public int plateID; // Assign a unique ID to each pressure plate in the inspector
    public TimePuzzle timePuzzle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timePuzzle.PressurePlateActivated(plateID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}