using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SobelController : MonoBehaviour
{
    public Transform FloorAndWallsParent;

    public void EnableSobel()
    {
        foreach (Transform t in FloorAndWallsParent)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Outlined");
            foreach (Transform t2 in t)
            {
                t2.gameObject.layer = LayerMask.NameToLayer("Outlined");
            }
            
        }
    }

    public void EnableSobel(GameObject gameObject)
    {
        gameObject.layer = LayerMask.NameToLayer("Outlined");
    }

    public void DisableSobel()
    {
        foreach (Transform t in FloorAndWallsParent)
        {
            t.gameObject.layer = LayerMask.NameToLayer("No Outlined");
            foreach (Transform t2 in t.GetComponentsInChildren<Transform>())
            {
                t2.gameObject.layer = LayerMask.NameToLayer("No Outlined");
            }
        }
    }

    public void DisableSobel(GameObject gameObject)
    {
        gameObject.layer = LayerMask.NameToLayer("No Outlined");
    }
}
