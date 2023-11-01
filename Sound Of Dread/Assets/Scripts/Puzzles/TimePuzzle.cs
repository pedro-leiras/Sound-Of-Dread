using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePuzzle : MonoBehaviour
{
    private bool firstPlateActivated = false;
    private bool secondPlateActivated = false;
    private float timer = 30f;
    public bool Level2Finish = false;

    private void Update()
    {
        if (!Level2Finish)
        {
            if (firstPlateActivated)
            { 

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Debug.Log("Time is up");
                    //Failed attempt
                    ResetPuzzle2();
                }   
            }
        }
    }

    public void PressurePlateActivated(int plateID)
    {
        if (plateID == 1)
        {
            firstPlateActivated = true;
        }
        else if (plateID == 2)
        {
            secondPlateActivated = true;
            if (firstPlateActivated && secondPlateActivated)
            {
                Debug.Log("You win");
                Level2Finish = true;
            }
        }
    }

    private void ResetPuzzle2()
    {
        firstPlateActivated = false;
        secondPlateActivated = false;
        timer = 30f;
    }
}
