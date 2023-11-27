using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int gridSize;
    [SerializeField] private float gridScale;
    private float[,] grid;

    private void Start()
    {
        InputManager.OnClick += InputManager_OnClick;
        grid = new float[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                grid[x, y] = Random.Range(0f, 2f);
            }
        }
    }

    private void InputManager_OnClick(object sender, Vector3 worldPosition)
    {
        worldPosition.z = 0f;
        Debug.Log(worldPosition);

        Vector2Int gridPosition = GetGridPositionFromWorldPosition(worldPosition);

        if(!IsValidGridPosition(gridPosition)) { Debug.LogWarning("Invalid Grid Position!"); return; }

        grid[gridPosition.x, gridPosition.y] = 0;
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
