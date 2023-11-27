using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Test : MonoBehaviour
{
    [Range(0.1f, 0.25f)]
    [SerializeField] float cornerRadius;

    Vector2 topRight;
    Vector2 bottomRight;
    Vector2 bottomLeft;
    Vector2 topLeft;

    Vector2 rightCenter;
    Vector2 bottomCenter;
    Vector2 leftCenter;
    Vector2 topCenter;

    [Space]
    //[Range(0f, 20f)] 
    [SerializeField] private float topRightValue;
    //[Range(0f, 20f)] 
    [SerializeField] private float bottomRightValue;
    //[Range(0f, 20f)] 
    [SerializeField] private float bottomLeftValue;
    //[Range(0f, 20f)] 
    [SerializeField] private float topLeftValue;
    [Space]
    [SerializeField] private float isoValue;

    private List<Vector3> vertices = new();
    private List<int> triangles = new();

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    [Header("Grid Data")]
    [SerializeField] private int gridSize;
    [SerializeField] private float gridScale;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        
        topRight = Vector2.one / 2;
        bottomRight = topRight + Vector2.down;
        bottomLeft = bottomRight + Vector2.left;
        topLeft = bottomLeft + Vector2.up;

        rightCenter = (topRight + bottomRight) / 2;
        bottomCenter = (bottomRight + bottomLeft) / 2;
        leftCenter = (bottomLeft + topLeft) / 2;
        topCenter = (topLeft + topRight) / 2;
        

        Shader shader = Shader.Find("Diffuse");
        Material material = new(shader);
        meshRenderer.material = material;
    }

    private void Update()
    {
        //Debug.Log($"Configaration: {Convert.ToString(GetConfigaration(), toBase: 2).PadLeft(4, '0'),4}");
        
        CreateMesh();
    }

    /*
    private void Interpolate() 
    {
        float topLerp = Mathf.InverseLerp(topLeftValue, topRightValue, isoValue);
        topCenter = topLeft + (topRight - topLeft) * topLerp;


        float rightLerp = Mathf.InverseLerp(topRightValue, bottomRightValue, isoValue);
        rightCenter = topRight + (bottomRight - topRight ) * rightLerp;


        float bottomLerp = Mathf.InverseLerp(bottomLeftValue, bottomRightValue, isoValue);
        bottomCenter = bottomLeft + (bottomRight - bottomLeft) * bottomLerp;


        float leftLerp = Mathf.InverseLerp(topLeftValue, bottomLeftValue, isoValue);
        leftCenter = topLeft + (bottomLeft - topLeft) * leftLerp;
    }

    private void Triangulate(int configaration)
    {
        int[] oneTriangle       = new int[] { 0, 1, 2 };
        int[] twoTriangles      = new int[] { 0, 1, 2, 0, 2, 3 };
        int[] threeTriangles    = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        int[] fourTriangles     = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 5, 3, 4, 5 };

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
                vertices.AddRange(new Vector3[] {topCenter, topRight, rightCenter, bottomCenter, bottomLeft, leftCenter });
                triangles.AddRange(fourTriangles);
                break;
            case 6:
                vertices.AddRange(new Vector3[] {rightCenter, bottomRight, bottomLeft, leftCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 7:
                vertices.AddRange(new Vector3[] { topCenter, topRight, bottomRight, bottomLeft,leftCenter });
                triangles.AddRange(threeTriangles);
                break;
            case 8:
                vertices.AddRange(new Vector3[] { topLeft, topCenter, leftCenter});
                triangles.AddRange(oneTriangle);
                break;
            case 9:
                vertices.AddRange(new Vector3[] {topLeft, topRight, rightCenter, leftCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 10:
                vertices.AddRange(new Vector3[] {topLeft, topCenter, rightCenter,bottomRight, bottomCenter, leftCenter });
                triangles.AddRange(fourTriangles);
                break;
            case 11:
                vertices.AddRange(new Vector3[] { leftCenter, topLeft, topRight, bottomRight, bottomCenter});
                triangles.AddRange(threeTriangles);
                break;
            case 12:
                vertices.AddRange(new Vector3[] {bottomCenter, bottomLeft,topLeft,topCenter });
                triangles.AddRange(twoTriangles);
                break;
            case 13:
                vertices.AddRange(new Vector3[] {topRight, rightCenter,bottomCenter,bottomLeft,topLeft });
                triangles.AddRange(threeTriangles);
                break;
            case 14:
                vertices.AddRange(new Vector3[] { topCenter, rightCenter, bottomRight, bottomLeft, topLeft });
                triangles.AddRange(threeTriangles);
                break;
            case 15:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, topLeft});
                triangles.AddRange(twoTriangles);
                break;
            default:
                break;
        }
    }

    private int GetConfigaration()
    {
        int configaration = 0;
        if (topRightValue > isoValue) { configaration |= (1 << 0); }               //configaration += 1;
        if (bottomRightValue > isoValue) { configaration |= (1 << 1); }            //configaration += 2;
        if (bottomLeftValue > isoValue) { configaration |= (1 << 2); }             //configaration += 4;
        if (topLeftValue > isoValue) { configaration |= (1 << 3); }                //configaration += 8;
        return configaration;
    }
    */

    private void CreateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        Square square = new Square(Vector3.zero, gridScale);

        square.Triangulate(isoValue, new float[] 
        {
            topRightValue,
            bottomRightValue,
            bottomLeftValue,
            topLeftValue
        });

        Mesh mesh = new()
        {
            vertices = square.GetVertices().ToArray(),
            triangles = square.GetTriangles().ToArray()
        };
        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(topRight, cornerRadius);
        Gizmos.DrawSphere(bottomRight, cornerRadius);
        Gizmos.DrawSphere(bottomLeft, cornerRadius);
        Gizmos.DrawSphere(topLeft, cornerRadius);

        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(rightCenter, cornerRadius / 1.75f);
        Gizmos.DrawSphere(bottomCenter, cornerRadius / 1.75f);
        Gizmos.DrawSphere(leftCenter, cornerRadius / 1.75f);
        Gizmos.DrawSphere(topCenter, cornerRadius / 1.75f);
    }
}
