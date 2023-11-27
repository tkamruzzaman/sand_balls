using System.Collections.Generic;
using UnityEngine;

public struct Square
{
    private Vector2 position;

    private Vector2 topRight;
    private Vector2 bottomRight;
    private Vector2 bottomLeft;
    private Vector2 topLeft;

    private Vector2 rightCenter;
    private Vector2 bottomCenter;
    private Vector2 leftCenter;
    private Vector2 topCenter;

    private List<Vector3> vertices;
    private List<int> triangles;

    public Square(Vector2 position, float gridScale) : this()
    {
        this.position = position;

        topRight = position+ gridScale * Vector2.one / 2;
        bottomRight = topRight + Vector2.down * gridScale;
        bottomLeft = bottomRight + Vector2.left * gridScale;
        topLeft = bottomLeft + Vector2.up * gridScale;

        rightCenter = (topRight + bottomRight) / 2;
        bottomCenter = (bottomRight + bottomLeft) / 2;
        leftCenter = (bottomLeft + topLeft) / 2;
        topCenter = (topLeft + topRight) / 2;

        vertices = new List<Vector3>();
        triangles = new List<int>();
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

    public void Triangulate(float isoValue, float[] values)
    {
        vertices.Clear();
        triangles.Clear();

        int configaration = GetConfigaration(isoValue, values);

        Interpolate(isoValue, values);

        Triangulate(configaration);
    }

    private void Triangulate(int configaration)
    {
        int[] oneTriangle = new int[] { 0, 1, 2 };
        int[] twoTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
        int[] threeTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        int[] fourTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 5, 3, 4, 5 };

        switch (configaration)
        {
            case 0:
                break;
            case 1:
                vertices.AddRange(new Vector3[] { topRight, rightCenter, topCenter });
                triangles.AddRange(oneTriangle);
                break;
            case 2:
                vertices.AddRange(new Vector3[] { rightCenter, bottomRight, bottomCenter });
                triangles.AddRange(oneTriangle);
                break;
            case 3:
                vertices.AddRange(new Vector3[] { topCenter, topRight, bottomRight, bottomCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 4:
                vertices.AddRange(new Vector3[] { bottomCenter, bottomLeft, leftCenter });
                triangles.AddRange(oneTriangle);
                break;
            case 5:
                vertices.AddRange(new Vector3[] { topCenter, topRight, rightCenter, bottomCenter, bottomLeft, leftCenter });
                triangles.AddRange(fourTriangles);
                break;
            case 6:
                vertices.AddRange(new Vector3[] { rightCenter, bottomRight, bottomLeft, leftCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 7:
                vertices.AddRange(new Vector3[] { topCenter, topRight, bottomRight, bottomLeft, leftCenter });
                triangles.AddRange(threeTriangles);
                break;
            case 8:
                vertices.AddRange(new Vector3[] { topLeft, topCenter, leftCenter });
                triangles.AddRange(oneTriangle);
                break;
            case 9:
                vertices.AddRange(new Vector3[] { topLeft, topRight, rightCenter, leftCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 10:
                vertices.AddRange(new Vector3[] { topLeft, topCenter, rightCenter, bottomRight, bottomCenter, leftCenter });
                triangles.AddRange(fourTriangles);
                break;
            case 11:
                vertices.AddRange(new Vector3[] { leftCenter, topLeft, topRight, bottomRight, bottomCenter });
                triangles.AddRange(threeTriangles);
                break;
            case 12:
                vertices.AddRange(new Vector3[] { bottomCenter, bottomLeft, topLeft, topCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 13:
                vertices.AddRange(new Vector3[] { topRight, rightCenter, bottomCenter, bottomLeft, topLeft });
                triangles.AddRange(threeTriangles);
                break;
            case 14:
                vertices.AddRange(new Vector3[] { topCenter, rightCenter, bottomRight, bottomLeft, topLeft });
                triangles.AddRange(threeTriangles);
                break;
            case 15:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, topLeft });
                triangles.AddRange(twoTriangles);
                break;
            default:
                break;
        }
    }

    private int GetConfigaration(float isoValue, float[] values)
    {
        int configaration = 0;
        if (values[0] > isoValue) { configaration |= (1 << 0); }               //configaration += 1;
        if (values[1] > isoValue) { configaration |= (1 << 1); }            //configaration += 2;
        if (values[2] > isoValue) { configaration |= (1 << 2); }             //configaration += 4;
        if (values[3] > isoValue) { configaration |= (1 << 3); }                //configaration += 8;
        return configaration;
    }


    public readonly Vector3[] GetVertices() => vertices.ToArray();

    public readonly int[] GetTriangles() => triangles.ToArray();

}
