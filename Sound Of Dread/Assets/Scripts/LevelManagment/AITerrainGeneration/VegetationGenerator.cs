using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    [SerializeField] public GameObject grassPrefab;
    public int numberOfGrassInstances = 10;

    public void GenerateVegetation(Vector3[,] gridPositions){
        if (grassPrefab == null){
            return;
        }

        foreach (Vector3 position in gridPositions){
            for (int i = 0; i < numberOfGrassInstances; i++){
                Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                Vector3 spawnPosition = position + randomOffset;

                Instantiate(grassPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
