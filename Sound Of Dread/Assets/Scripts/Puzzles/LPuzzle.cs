using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPuzzle : MonoBehaviour
{
    public int letters;
    public DoorController[] doors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (letters == 5)
        {
            foreach (DoorController door in doors)
            {
                if (door.doorID == 20)
                {
                    door.lockStatus = 0;
                    door.OpenChurchDoor();
                }
            }
        }
    }
}
