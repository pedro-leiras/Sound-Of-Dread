using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CheckpointData
{
    public string levelName;
    public Transform spawnPoint;
}

public class CheckpointManager : MonoBehaviour
{
    public List<CheckpointData> checkpoints = new List<CheckpointData>();

    public static CheckpointManager instance; // Singleton reference

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}