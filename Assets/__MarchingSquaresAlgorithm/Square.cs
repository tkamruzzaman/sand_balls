using System.Collections.Generic;
using UnityEngine;

public struct Square
{
    private Vector2 topRight;
    private Vector2 bottomRight;
    private Vector2 bottomLeft;
    private Vector2 topLeft;

    private Vector2 rightCenter;
    private Vector2 bottomCenter;
    private Vector2 leftCenter;
    private Vector2 topCenter;

    private readonly List<Vector3> vertices;
    private readonly List<int> triangles;

    private readonly Vector3[] case_1;
    private readonly Vector3[] case_2;
    private readonly Vector3[] case_3;
    private readonly Vector3[] case_4;
    private readonly Vector3[] case_5;
    private readonly Vector3[] case_6;
    private readonly Vector3[] case_7;
    private readonly Vector3[] case_8;
    private readonly Vector3[] case_9;
    private readonly Vector3[] case_10;
    private readonly Vector3[] case_11;
    private readonly Vector3[] case_12;
    private readonly Vector3[] case_13;
    private readonly Vector3[] case_14;
    private readonly Vector3[] case_15;

    private readonly int[] oneTriangle;
    private readonly int[] twoTriangles;
    private readonly int[] threeTriangles;
    private readonly int[] fourTriangles;

    public Square(Vector2 position, float gridScale) : this()
    {
        topRight = position + gridScale * Vector2.one / 2;
        bottomRight = topRight + Vector2.down * gridScale;
        bottomLeft = bottomRight + Vector2.left * gridScale;
        topLeft = bottomLeft + Vector2.up * gridScale;

        rightCenter = (topRight + bottomRight) / 2;
        bottomCenter = (bottomRight + bottomLeft) / 2;
        leftCenter = (bottomLeft + topLeft) / 2;
        topCenter = (topLeft + topRight) / 2;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        case_1 = new Vector3[] { topRight, rightCenter, topCenter };
        case_2 = new Vector3[] { rightCenter, bottomRight, bottomCenter };
        case_3 = new Vector3[] { topCenter, topRight, bottomRight, bottomCenter };
        case_4 = new Vector3[] { bottomCenter, bottomLeft, leftCenter };
        case_5 = new Vector3[] { topCenter, topRight, rightCenter, bottomCenter, bottomLeft, leftCenter };
        case_6 = new Vector3[] { rightCenter, bottomRight, bottomLeft, leftCenter };
        case_7 = new Vector3[] { topCenter, topRight, bottomRight, bottomLeft, leftCenter };
        case_8 = new Vector3[] { topLeft, topCenter, leftCenter };
        case_9 = new Vector3[] { topLeft, topRight, rightCenter, leftCenter };
        case_10 = new Vector3[] { topLeft, topCenter, rightCenter, bottomRight, bottomCenter, leftCenter };
        case_11 = new Vector3[] { leftCenter, topLeft, topRight, bottomRight, bottomCenter };
        case_12 = new Vector3[] { bottomCenter, bottomLeft, topLeft, topCenter };
        case_13 = new Vector3[] { topRight, rightCenter, bottomCenter, bottomLeft, topLeft };
        case_14 = new Vector3[] { topCenter, rightCenter, bottomRight, bottomLeft, topLeft };
        case_15 = new Vector3[] { topRight, bottomRight, bottomLeft, topLeft };

        oneTriangle = new int[] { 0, 1, 2 };
        twoTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
        threeTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        fourTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 5, 3, 4, 5 };
    }

    public void Triangulate(float isoValue, float[] values)
    {
        vertices.Clear();
        triangles.Clear();

        int configaration = GetConfigaration(isoValue, values);

        Interpolate(isoValue, values);

        Triangulate(configaration);
    }

    private readonly int GetConfigaration(float isoValue, float[] values)
    {
        int configaration = 0;
        if (values[0] > isoValue) { configaration |= (1 << 0); }            //configaration += 1;
        if (values[1] > isoValue) { configaration |= (1 << 1); }            //configaration += 2;
        if (values[2] > isoValue) { configaration |= (1 << 2); }            //configaration += 4;
        if (values[3] > isoValue) { configaration |= (1 << 3); }            //configaration += 8;
        return configaration;
    }

    private void Interpolate(float isoValue, float[] values)
    {
        float topLerp = Mathf.InverseLerp(values[3], values[0], isoValue);
        topCenter = topLeft + (topRight - topLeft) * topLerp;

        float rightLerp = Mathf.InverseLerp(values[0], values[1], isoValue);
        rightCenter = topRight + (bottomRight - topRight) * rightLerp;

        float bottomLerp = Mathf.InverseLerp(values[2], values[1], isoValue);
        bottomCenter = bottomLeft + (bottomRight - bottomLeft) * bottomLerp;

        float leftLerp = Mathf.InverseLerp(values[3], values[2], isoValue);
        leftCenter = topLeft + (bottomLeft - topLeft) * leftLerp;
    }

    private readonly void Triangulate(int configaration)
    {
        switch (configaration)
        {
            default:
            case 0: break;
            case 1: vertices.AddRange(case_1); triangles.AddRange(oneTriangle); break;
            case 2: vertices.AddRange(case_2); triangles.AddRange(oneTriangle); break;
            case 3: vertices.AddRange(case_3); triangles.AddRange(twoTriangles); break;
            case 4: vertices.AddRange(case_4); triangles.AddRange(oneTriangle); break;
            case 5: vertices.AddRange(case_5); triangles.AddRange(fourTriangles); break;
            case 6: vertices.AddRange(case_6); triangles.AddRange(twoTriangles); break;
            case 7: vertices.AddRange(case_7); triangles.AddRange(threeTriangles); break;
            case 8: vertices.AddRange(case_8); triangles.AddRange(oneTriangle); break;
            case 9: vertices.AddRange(case_9); triangles.AddRange(twoTriangles); break;
            case 10: vertices.AddRange(case_10); triangles.AddRange(fourTriangles); break;
            case 11: vertices.AddRange(case_11); triangles.AddRange(threeTriangles); break;
            case 12: vertices.AddRange(case_12); triangles.AddRange(twoTriangles); break;
            case 13: vertices.AddRange(case_13); triangles.AddRange(threeTriangles); break;
            case 14: vertices.AddRange(case_14); triangles.AddRange(threeTriangles); break;
            case 15: vertices.AddRange(case_15); triangles.AddRange(twoTriangles); break;
        }
    }

    public readonly IList<Vector3> GetVertices() => vertices.AsReadOnly();

    public readonly IList<int> GetTriangles() => triangles.AsReadOnly();

    public readonly IList<int> UpdateAndGetTriangles(int triangleStartIndex)
    {
        for (int i = 0; i < triangles.Count; i++)
        {
            triangles[i] += triangleStartIndex;
        }

       return triangles.AsReadOnly();
    }

}
