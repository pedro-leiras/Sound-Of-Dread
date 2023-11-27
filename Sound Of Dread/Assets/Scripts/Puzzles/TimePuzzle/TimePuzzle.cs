using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePuzzle : MonoBehaviour
{
    private bool firstPlateActivated = false;
    private bool secondPlateActivated = false;
    private float timer = 60f;
    public bool Level2Finish = false;
    public DoorController[] doors;
    public WaveController waveController;
    private float elapsed = 0f;
    public AudioSource plateTicking;
    public AudioClip plateSound;

    private void Update()
    {
        if (!Level2Finish)
        {
            if (firstPlateActivated)
            {
                elapsed += Time.deltaTime;
                
                if (elapsed >= 1f)
                {
                    elapsed = elapsed % 1f;
                    waveController.SpawnFootSteepsWaveEffect();
                }

                timer -= Time.deltaTime;
                Debug.Log(timer);
                if (timer <= 0)
                {
                    Debug.Log("Time is up");
                    ResetPuzzle2();
                }   
            }
        }
    }

    public void PressurePlateActivated(int plateID)
    {
        if (plateID == 0)
        {
            firstPlateActivated = true;
            plateTicking.clip = plateSound;
            plateTicking.Play();
        }
        else if (plateID == 1)
        {
            secondPlateActivated = true;
            if (firstPlateActivated && secondPlateActivated)
            {
                Debug.Log("You win");
                //plateTicking.Stop();
                foreach (DoorController door in doors)
                {
                    if(door.doorID == 5)
                    {
                        door.lockStatus = 0;
                    }
                }
                Level2Finish = true;
            }
        }
    }

    private void ResetPuzzle2()
    {
        firstPlateActivated = false;
        secondPlateActivated = false;
        timer = 60f;
        elapsed = 0f;
    }
}
