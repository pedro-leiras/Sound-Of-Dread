using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    [SerializeField] public GameObject grassPrefab;
    public int numberOfGrassInstances = 10;
    private List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();
    [HideInInspector] Mesh grassMesh;
    [HideInInspector] Material grassMaterial;
    [HideInInspector] public Vector3 spawnPosition;

    public List<Matrix4x4> BuildNewBatch(){
        return new List<Matrix4x4>();
    }

    public void AddObj(List<Matrix4x4> matrices, Vector3 spawnPosition, int i){
        matrices.Add(Matrix4x4.TRS(spawnPosition, Quaternion.identity, Vector3.one));
        // Debug.Log("Object added " + spawnPosition.x + "," + spawnPosition.y + "," + spawnPosition.z);
    }
    
    public void GeneratePosition(Vector3[,] gridPositions){
        grassMesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
        grassMaterial = grassPrefab.GetComponent<MeshRenderer>().sharedMaterial;
       
        if (grassPrefab.GetComponent<MeshFilter>() == null || grassPrefab.GetComponent<MeshRenderer>() == null){
            // Debug.LogError("MeshFilter or MeshRenderer component not found on grassPrefab.");
            return;
        }

        int indexNum = 0;
        List<Matrix4x4> matrices = new List<Matrix4x4>();

        foreach (Vector3 position in gridPositions){
            for(int i = 0; i < numberOfGrassInstances; i++){
                Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                spawnPosition = position + randomOffset;

                AddObj(matrices, spawnPosition, i);
                indexNum++;

                if(indexNum >= 1000){
                    batches.Add(matrices);
                    matrices = BuildNewBatch();
                    indexNum = 0;
                }
            }
        }
    }

    public void GenerateVegetation(){
        foreach(var batch in batches){
            Graphics.DrawMeshInstanced(grassMesh, 0, grassMaterial, batch);
            // Debug.Log("Drawing instances");
        }
    }
}
