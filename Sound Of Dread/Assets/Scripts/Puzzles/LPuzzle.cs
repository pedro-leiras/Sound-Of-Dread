using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPuzzle : MonoBehaviour
{
    public int letters;
    public DoorController[] doors;
    public bool Level3Finish = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Level3Finish == false)
        {
            if (letters == 5)
            {
                Level3Finish = true;
                foreach (DoorController door in doors)
                {
                    if (door.doorID == 420)
                    {
                        door.lockStatus = 0;
                        door.OpenChurchDoor();
                    }
                }
            }
        }
        
    }
}
