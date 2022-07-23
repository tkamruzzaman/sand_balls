using UnityEngine;

public class PlaneDeformer : MonoBehaviour
{
    //public references
    public float radiusOfDeformation = 0.7f;
    public float powerOfDeformation = 2.0f;
    public GameObject cylinderPrefab;


    // private references
    private MeshFilter meshFilter;
    private Mesh mesh;
    private MeshCollider col;
    private Vector3[] verts;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
        verts = mesh.vertices;
    }

    public void DeformThePlane(Vector3 positionToDeform)
    {
        Vector3 hitpoint = positionToDeform;
        positionToDeform = transform.InverseTransformPoint(positionToDeform);
        bool somethingDeformed = false;

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - positionToDeform).sqrMagnitude;

            if (dist < radiusOfDeformation)
            {
                verts[i] -= Vector3.up * powerOfDeformation;
                somethingDeformed = true;
            }

        }
        if (somethingDeformed)
        {
            mesh.vertices = verts;
            col.sharedMesh = mesh;
            //Instantiate(cylinderPrefab, new Vector3(hitpoint.x, hitpoint.y, hitpoint.z + 0.11f), Quaternion.Euler(-90f, 0f, 0f));
        }
    }

    public void Puthole(Vector3 positionToDeform, float radius)
    {
        Vector3 hitpoint = positionToDeform;
        positionToDeform = transform.InverseTransformPoint(positionToDeform);
        bool somethingDeformed = false;

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - positionToDeform).sqrMagnitude;

            if (dist < radius)
            {
                verts[i] -= Vector3.up * powerOfDeformation;
                somethingDeformed = true;
            }

        }
        if (somethingDeformed)
        {
            mesh.vertices = verts;
            col.sharedMesh = mesh;
            //Instantiate(cylinderPrefab, new Vector3(hitpoint.x, hitpoint.y, hitpoint.z + 0.11f), Quaternion.Euler(-90f, 0f, 0f));
        }
    }
}
