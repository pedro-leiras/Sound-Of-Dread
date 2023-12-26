using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SobelController_Multiplayer : NetworkBehaviour
{
    public Transform ObjectsToBeDetected; 
    public float initialRadius = 0f; // Set the initial detection radius in the Unity editor
    public float radiusIncreaseRate = 5f; // Set the rate at which the radius increases in the Unity editor
    public LayerMask targetLayer;

    private float currentRadius;
    private bool canDraw = false;
    private Vector3 startPosition = Vector3.zero;

    private void Start()
    {
        ObjectsToBeDetected = GameObject.Find("ObjectsToBeDetected").transform;
    }

    void Update()
    {
        // Increase the detection radius over time
        if (canDraw) { 
            currentRadius += radiusIncreaseRate * Time.deltaTime;

            // Detect objects within the current radius
            Collider[] colliders = Physics.OverlapSphere(startPosition, currentRadius, targetLayer);

            // Process the detected objects
            foreach (Collider collider in colliders)
            {
                if(collider.gameObject.tag != "Player") { 
                    collider.gameObject.layer = LayerMask.NameToLayer("Outlined");
                    for (int i = 0; i < collider.transform.childCount; i++)
                    {
                        // Access each child's Transform component
                        Transform childTransform = collider.transform.GetChild(i);
                        // Change the layer of the child
                        ChangeLayerRecursive(childTransform, "Outlined");
                    }
                }
            }
        }
    }

    /*void OnDrawGizmos()
    {
        //Desenha a esfera de sobreposição (Overlap Sphere) no Editor do Unity
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPosition, currentRadius);
    }*/

    public void EnableSobel()
    {
        for (int i = 0; i < ObjectsToBeDetected.childCount; i++)
        {
            // Access each child's Transform component
            Transform childTransform = ObjectsToBeDetected.GetChild(i);

            // Change the layer of the child
            ChangeLayerRecursive(childTransform, "Outlined");
        }
    }

    public void EnableSobel(Vector3 initialPosition)
    {
        canDraw = true;
        startPosition = initialPosition;
    }

    public void ChangeLayerRecursive(Transform currentTransform, string layerName)
    {
        // Change the layer of the current object
        currentTransform.gameObject.layer = LayerMask.NameToLayer(layerName);

        // Loop through each child of the current object
        for (int i = 0; i < currentTransform.childCount; i++)
        {
            // Access each child's Transform component
            Transform childTransform = currentTransform.GetChild(i);

            // Recursively change the layer of the child's descendants
            ChangeLayerRecursive(childTransform, layerName);
        }
    }

    public void DisableSobel()
    {
        for (int i = 0; i < ObjectsToBeDetected.childCount; i++)
        {
            // Access each child's Transform component
            Transform childTransform = ObjectsToBeDetected.GetChild(i);

            // Change the layer of the child
            ChangeLayerRecursive(childTransform, "No Outlined");
        }

        canDraw = false;
        currentRadius = initialRadius;
    }
}
