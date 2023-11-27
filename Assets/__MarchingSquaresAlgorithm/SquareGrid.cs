using System.Collections.Generic;
using UnityEngine;

public struct SquareGrid
{
    public Square[,] squares;

    private readonly List<Vector3> vertices;
    private readonly List<int> triangles;

    private readonly float isoValue;

    public SquareGrid(int size, float gridScale, float isoValue)
    {
        squares = new Square[size, size];
        vertices = new List<Vector3>();
        triangles = new List<int>();

        this.isoValue = isoValue;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 squarePosition = new Vector2(x, y) * gridScale;

                squarePosition.x -= (size * gridScale) / 2 - gridScale / 2;
                squarePosition.y -= (size * gridScale) / 2 - gridScale / 2;

                squares[x, y] = new Square(squarePosition, gridScale);
            }
        }
    }

    public readonly void Update(float[,] grid)
    {
        vertices.Clear();
        triangles.Clear();

        int triangleStartIndex = 0;

        for (int y = 0; y < squares.GetLength(1); y++)
        {
            for (int x = 0; x < squares.GetLength(0); x++)
            {
                Square currentSquare = squares[x, y];

                float[] values = new float[4];

                values[0] = grid[x + 1, y + 1];
                values[1] = grid[x + 1, y + 0];
                values[2] = grid[x + 0, y + 0];
                values[3] = grid[x + 0, y + 1];

                currentSquare.Triangulate(isoValue, values);

                vertices.AddRange(currentSquare.GetVertices());

                triangles.AddRange(currentSquare.UpdateAndGetTriangles(triangleStartIndex));
                
                triangleStartIndex += currentSquare.GetVertices().Count;
            }
        }
    }

    public readonly List<Vector3> GetVertices() => vertices;

    public readonly List<int> GetTriangles() => triangles;

}
