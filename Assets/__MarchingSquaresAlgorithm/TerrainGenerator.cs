//#define TESTING

using System.Collections.Generic;
#if TESTING && UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private int brushRadius;
    [SerializeField] private float brushStrength;
    [SerializeField] private float brushFallback;

    [Header("Grid Data")]
    [SerializeField] private int gridSize;
    [SerializeField] private float gridScale;
    [SerializeField] private float isoValue;

    private SquareGrid squareGrid;
    private float[,] grid;

    private Mesh mesh;

    private List<Vector3> vertices = new();
    private List<int> triangles = new();
    [SerializeField] private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        Application.targetFrameRate = 60;

        mesh = new();

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

        squareGrid = new SquareGrid(gridSize - 1, gridScale, isoValue);

        GenerateMesh();
    }

    private void InputManager_OnClick(object sender, Vector3 worldPosition)
    {
        worldPosition.z = 0f;

        worldPosition = transform.InverseTransformPoint(worldPosition);

        Vector2Int gridPosition = GetGridPositionFromWorldPosition(worldPosition);

        bool shouldGenerateMesh = false;

        for (int y = gridPosition.y - brushRadius; y <= gridPosition.y + brushRadius; y++)
        {
            for (int x = gridPosition.x - brushRadius; x <= gridPosition.x + brushRadius; x++)
            {
                Vector2Int currentGridPosition = new(x, y);

                if (!IsValidGridPosition(currentGridPosition))
                {
                    //Debug.LogWarning("Invalid Grid Position!");
                    continue;
                }

                float distance = Vector2.Distance(currentGridPosition, gridPosition);
                float factor = brushStrength * Mathf.Exp(-distance * brushFallback / brushRadius);

                grid[currentGridPosition.x, currentGridPosition.y] -= factor;

                shouldGenerateMesh = true;
            }
        }
        if (shouldGenerateMesh)
        {
            GenerateMesh();
        }
    }

    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        squareGrid.Update(grid);

        mesh = new Mesh();

        vertices = squareGrid.GetVertices();
        triangles = squareGrid.GetTriangles();


        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        meshFilter.mesh = mesh;

        GenerateCollider();
    }

    private void GenerateCollider()
    {
        if (meshFilter.TryGetComponent(out MeshCollider meshCollider))
        {
            meshCollider.sharedMesh = mesh;
        }
        else
        {
            meshFilter.gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }
    }

    private Vector2 GetWorldPositionFromGridPosition(int x, int y)
    {
        Vector2 worldPosition = new Vector2(x, y) * gridScale;

        worldPosition.x -= (gridSize * gridScale) / 2 - gridScale / 2;
        worldPosition.y -= (gridSize * gridScale) / 2 - gridScale / 2;

        return worldPosition;
    }

    private Vector2Int GetGridPositionFromWorldPosition(Vector2 worldPosition)
        => new()
        {
            x = Mathf.FloorToInt(worldPosition.x / gridScale + gridSize / 2 - gridScale / 2),
            y = Mathf.FloorToInt(worldPosition.y / gridScale + gridSize / 2 - gridScale / 2)
        };

    private bool IsValidGridPosition(Vector2Int gridPosition)
        => gridPosition.x >= 0 && gridPosition.x < gridSize
        && gridPosition.y >= 0 && gridPosition.y < gridSize;

    private void OnDestroy()
    {
        InputManager.OnClick -= InputManager_OnClick;
    }

#if TESTING && UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying) { return; }

        Gizmos.color = Color.green;

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Vector2 worldPosition = GetWorldPositionFromGridPosition(x, y);
                worldPosition = transform.InverseTransformPoint(worldPosition);
                Gizmos.DrawSphere(worldPosition, gridScale / 4);
                Handles.Label(worldPosition + Vector2.up * gridScale / 3, grid[x, y].ToString());
            }
        }
    }
#endif

}
