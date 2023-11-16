using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController door;
    public bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            GameObject.FindGameObjectWithTag("Object").layer = LayerMask.NameToLayer("Outlined");
            door.CloseDoor();
        }
    }

    public bool IsTriggeredCheck() => isTriggered;
}