using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{

    public GameObject player;
    public GameObject[] throwableObjects;
    public MovementStateManager movementStateManager;

    private Vector3[] initialObjectPositions;
    private Quaternion[] initialObjectRotations;
    [SerializeField] public bool test = false;

    private void Start()
    {

        StoreInitialObjectTransforms();
    }
    private void Update()
    {

        if(movementStateManager.currentHealth < 1)
        {
            OnPlayerDeath();
        }
    }

    // Called when the player dies
    public void OnPlayerDeath()
    {
        DestroyThrowableObjects();

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

    // Store the initial positions and rotation
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

    // Respawn throwable objects at their initial positions
    private void RespawnThrowableObjects()
    {
        for (int i = 0; i < throwableObjects.Length; i++)
        {
            if (throwableObjects[i] != null)
            {
                GameObject newThrowableObject = Instantiate(throwableObjects[i], initialObjectPositions[i], initialObjectRotations[i]);

                // Enable the PickUp script and AudioSource
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

                throwableObjects[i] = newThrowableObject;
            }
        }
    }
}
