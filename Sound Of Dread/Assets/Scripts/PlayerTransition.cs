using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    [SerializeField] private GameObject playerInside;
    [SerializeField] public RuntimeAnimatorController anim;
    [SerializeField] public Material MaterialSkin;
    [SerializeField] public Material MaterialClothes;
    [SerializeField] public string layer;
    [SerializeField] public GameObject TriggerToEnable;

    private void OnTriggerEnter(Collider other)
    {
        Transform trans = playerInside.transform;
        ChangeLayerRecursive(trans, layer);
        Transform geo = trans.Find("Geo");
        Transform skin = geo.Find("Body_LOD1");
        Transform clothes = geo.Find("Clothes_LOD1");
        Transform eyes = geo.Find("Eyes");
        playerInside.GetComponent<Animator>().runtimeAnimatorController = anim as RuntimeAnimatorController;
        skin.gameObject.GetComponent<Renderer>().material = MaterialSkin;
        eyes.gameObject.GetComponent<Renderer>().material = MaterialSkin;
        clothes.gameObject.GetComponent<Renderer>().material = MaterialClothes;

        TriggerToEnable.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ChangeLayerRecursive(Transform currentTransform, string layerName)
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
}