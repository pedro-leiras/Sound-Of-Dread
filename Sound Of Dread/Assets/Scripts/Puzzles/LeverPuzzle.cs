using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{

    public DoorController[] doors;
    public LeverController[] levers;
    public bool Level1Finish = false;
    //Start is called before the first frame update
    void Start()
    {

    }

    //Update is called once per frame
    void Update()
    {
        if (!Level1Finish)
        {
            bool allLeversInState2 = true;

            //Check if all levers are opened
            foreach (LeverController lever in levers)
            {
                if (lever.leverState != 2)
                {
                    allLeversInState2 = false;
                    break;
                }
            }

            if (allLeversInState2)
            {
                foreach (DoorController door in doors)
                {
                    if (door.doorID == 1)
                    {
                        door.lockStatus = 0;
                    }
                }
                Level1Finish = true;
            }
        }
    }
}
