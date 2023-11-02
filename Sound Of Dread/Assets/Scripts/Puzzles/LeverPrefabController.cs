using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPrefabController : MonoBehaviour
{
    public string originalLayerName = "Default"; 
    public string outlinedLayerName = "Outlined"; 

    private void Start()
    {
        //Começa default
        SetPrefabLayer(originalLayerName);
    }

    public void ChangeLayersToOutlinedAndRevert()
    {
        //Layer para outlined
        SetPrefabLayer(outlinedLayerName);

       
        StartCoroutine(RevertLayerAfterDelay());
    }

    private void SetPrefabLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);

        // Muda para todos os objetos child
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    private IEnumerator RevertLayerAfterDelay()
    {
        //espera 2 segundos
        yield return new WaitForSeconds(2f);

        //Layer para default
        SetPrefabLayer(originalLayerName);
    }
}
