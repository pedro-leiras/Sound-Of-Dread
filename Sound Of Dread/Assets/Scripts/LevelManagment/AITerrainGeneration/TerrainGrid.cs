using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : MonoBehaviour{
    public int gridSizeX = 250;
    public int gridSizeZ = 250;
    public float nodeRadius = 1f;

    [SerializeField] Terrain terrain;
    [SerializeField] VegetationGenerator vegetationGenerator;

    public Node[,] grid;

    void Start(){
        CreateGrid();
        SpawnVegetation();
    }

    private void CreateGrid(){
        if (terrain == null){
            return;
        }

        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        int gridSizeX = Mathf.RoundToInt(terrainSize.x / (nodeRadius * 2));
        int gridSizeZ = Mathf.RoundToInt(terrainSize.z / (nodeRadius * 2));

        grid = new Node[gridSizeX, gridSizeZ];

        for (int x = 0; x < gridSizeX; x++){
            for (int z = 0; z < gridSizeZ; z++){
                Vector3 worldPoint = terrainPosition + new Vector3(x * nodeRadius * 2 + nodeRadius, 0, z * nodeRadius * 2 + nodeRadius);

                float elevation = terrain.SampleHeight(worldPoint);

                worldPoint.y = elevation;

                bool walkable = true;

                grid[x, z] = new Node(walkable, worldPoint, x, z);
            }
        }
    }

    private void OnDrawGizmos(){
        if (grid != null){
            foreach (Node node in grid){
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius * 2 - 0.1f));
            }
        }
    }

    private void SpawnVegetation(){
        if (vegetationGenerator != null){
            Vector3[,] gridPositions = new Vector3[grid.GetLength(0), grid.GetLength(1)];

            for (int x = 0; x < grid.GetLength(0); x++){
                for (int z = 0; z < grid.GetLength(1); z++){
                    gridPositions[x, z] = grid[x, z].worldPosition;
                }
            }

            vegetationGenerator.GenerateVegetation(gridPositions);
        }            
    }

    public class Node{
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridZ;

        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridZ){
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridZ = _gridZ;
        }
    }
}
