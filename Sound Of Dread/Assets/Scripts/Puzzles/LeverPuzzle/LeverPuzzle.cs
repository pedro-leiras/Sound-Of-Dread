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
                if(lever.LeverID == 1 && lever.leverState == 2)
                {
                    foreach(DoorController door in doors)
                    {
                        if(door.doorID == 1)
                        {
                            door.lockStatus = 0;
                        }
                    }
                }
                if (lever.LeverID == 2 && lever.leverState == 2)
                {
                    foreach (DoorController door in doors)
                    {
                        if (door.doorID == 2)
                        {
                            door.lockStatus = 0;
                        }
                        if (door.doorID == 6)
                        {
                            door.lockStatus = 0;
                        }
                    }
                }
                if (lever.LeverID == 3 && lever.leverState == 2)
                {
                    foreach (DoorController door in doors)
                    {
                        if (door.doorID == 3)
                        {
                            door.lockStatus = 0;
                        }
                    }
                }
                if (lever.LeverID == 4 && lever.leverState == 2)
                {
                    foreach (DoorController door in doors)
                    {
                        if (door.doorID == 4)
                        {
                            door.lockStatus = 0;
                        }
                    }
                }

                if (lever.leverState != 2)
                {
                    allLeversInState2 = false;
                    break;
                }
            }

            if (allLeversInState2)
            {
                Level1Finish = true;
            }
        }
    }
}
