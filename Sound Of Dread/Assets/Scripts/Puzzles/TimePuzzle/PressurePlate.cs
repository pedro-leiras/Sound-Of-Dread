using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressurePlate : MonoBehaviour
{
    public int plateID; // Assign a unique ID to each pressure plate in the inspector
    public TimePuzzle timePuzzle;

    private AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
            timePuzzle.PressurePlateActivated(plateID);

        }
    }
}