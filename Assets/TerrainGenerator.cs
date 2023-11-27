using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private int brushRadius;
    [SerializeField] private float brushStrength;

    [Header("Grid Data")]
    [SerializeField] private int gridSize;
    [SerializeField] private float gridScale;
    [SerializeField] private float isoValue;
    
    private SquareGrid squareGrid;
    private float[,] grid;


    private void Start()
    {
        InputManager.OnClick += InputManager_OnClick;
        grid = new float[gridSize, gridSize];

        float thresholdValue = 0.1f;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                grid[x, y] = isoValue + thresholdValue;
            }
        }
        
        squareGrid = new SquareGrid(gridSize-1, gridScale, isoValue);

        GenerateMesh();
    }

    private void InputManager_OnClick(object sender, Vector3 worldPosition)
    {
        worldPosition.z = 0f;
        Debug.Log(worldPosition);

        Vector2Int gridPosition = GetGridPositionFromWorldPosition(worldPosition);

        for (int y = gridPosition.y - brushRadius;y <= gridPosition.y + brushRadius; y++)
        {
            for(int x = gridPosition.x - brushRadius; x <= gridPosition.x + brushRadius; x++)
            {
                Vector2Int currentGridPosition = new(x, y);

                if (!IsValidGridPosition(currentGridPosition))
                {
                    Debug.LogWarning("Invalid Grid Position!");
                    continue;
                }

                grid[currentGridPosition.x, currentGridPosition.y] -= brushStrength;
            }
        }

       GenerateMesh();
    }

    private List<Vector3> vertices = new();
    private List<int> triangles = new();
  [SerializeField]  private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;


    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        squareGrid.Update(grid);

        Mesh mesh = new()
        {
            vertices = squareGrid.GetVertices(),
            triangles = squareGrid.GetTriangles()
        };
        meshFilter.mesh = mesh;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying) { return; }

        Gizmos.color = Color.green;

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Vector2 worldPosition = GetWorldPositionFromGridPosition(x, y);

                Gizmos.DrawSphere(worldPosition, gridScale / 4);

                Handles.Label(worldPosition + Vector2.up * gridScale / 3, grid[x,y].ToString());
            }
        }
    }
#endif

    private Vector2 GetWorldPositionFromGridPosition(int x, int y)
    {
        Vector2 worldPosition = new Vector2(x, y) * gridScale;

        worldPosition.x -= (gridSize * gridScale) / 2 - gridScale / 2;
        worldPosition.y -= (gridSize * gridScale) / 2 - gridScale / 2;

        return worldPosition;
    }

    private Vector2Int GetGridPositionFromWorldPosition(Vector2 worldPosition)
    {
        Vector2Int gridPosition = new ();

        gridPosition.x = Mathf.FloorToInt(worldPosition.x/ gridScale + gridSize /2 - gridScale/2);
        gridPosition.y = Mathf.FloorToInt(worldPosition.y/ gridScale + gridSize /2 - gridScale/2);

        return gridPosition;
    }

    private bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < gridSize 
            && gridPosition.y >= 0 && gridPosition.y < gridSize;
    }
}
