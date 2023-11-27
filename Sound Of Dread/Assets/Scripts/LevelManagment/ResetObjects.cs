using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    // Reference to the player and throwable objects
    public GameObject player;
    public GameObject[] throwableObjects;

    // Arrays to store the initial positions and rotations of throwable objects
    private Vector3[] initialObjectPositions;
    private Quaternion[] initialObjectRotations;
    [SerializeField] public bool test = false;

    // Called at the start of the game
    private void Start()
    {
        // Store the initial positions and rotations of throwable objects
        StoreInitialObjectTransforms();
    }
    private void Update()
    {
        if(test == true)
        {
            OnPlayerDeath();
            test = false;
        }
    }

    // Called when the player dies
    public void OnPlayerDeath()
    {
        // Destroy all throwable objects
        DestroyThrowableObjects();

        // Other logic for player respawn goes here

        // Respawn throwable objects at their initial positions and rotations
        RespawnThrowableObjects();
    }

    // Destroy all throwable objects in the scene
    private void DestroyThrowableObjects()
    {
        foreach (GameObject throwableObject in throwableObjects)
        {
            if (throwableObject != null)
            {
                Destroy(throwableObject);
            }
        }
    }

    // Store the initial positions and rotations of throwable objects
    private void StoreInitialObjectTransforms()
    {
        initialObjectPositions = new Vector3[throwableObjects.Length];
        initialObjectRotations = new Quaternion[throwableObjects.Length];

        for (int i = 0; i < throwableObjects.Length; i++)
        {
            if (throwableObjects[i] != null)
            {
                initialObjectPositions[i] = throwableObjects[i].transform.position;
                initialObjectRotations[i] = throwableObjects[i].transform.rotation;
            }
        }
    }

    // Respawn throwable objects at their initial positions and rotations
    private void RespawnThrowableObjects()
    {
        for (int i = 0; i < throwableObjects.Length; i++)
        {
            if (throwableObjects[i] != null)
            {
                // Instantiate a new throwable object at its initial position and rotation
                GameObject newThrowableObject = Instantiate(throwableObjects[i], initialObjectPositions[i], initialObjectRotations[i]);

                // Enable the PickUp script and AudioSource if they are present
                PickUpController pickUpScript = newThrowableObject.GetComponent<PickUpController>();
                if (pickUpScript != null)
                {
                    pickUpScript.enabled = true;
                }

                AudioSource audioSource = newThrowableObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.enabled = true;
                }

                // Assign the new throwable object to the array
                throwableObjects[i] = newThrowableObject;
            }
        }
    }
}
