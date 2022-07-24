using UnityEngine;

public class PlaneDeformer : MonoBehaviour
{
    private GameManager m_GameManager;

    private MeshFilter m_MeshFilter;
    private Mesh m_Mesh;
    private MeshCollider m_MeshCollider;
    private Vector3[] m_Verts;

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshCollider = GetComponent<MeshCollider>();
        m_Mesh = m_MeshFilter.mesh;
        m_Verts = m_Mesh.vertices;

        m_GameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    public void DeformMesh(Vector3 positionToDeform)
    {
        Vector3 hitpoint = positionToDeform;
        positionToDeform = transform.InverseTransformPoint(positionToDeform);
        bool somethingDeformed = false;

        for (int i = 0; i < m_Verts.Length; i++)
        {
            float dist = (m_Verts[i] - positionToDeform).sqrMagnitude;

            if (dist < m_GameManager.radiusOfDeformation)
            {
                m_Verts[i] -= Vector3.up * m_GameManager.powerOfDeformation;
                somethingDeformed = true;
            }

        }
        if (somethingDeformed)
        {
            m_Mesh.vertices = m_Verts;
            m_MeshCollider.sharedMesh = m_Mesh;

            if (m_GameManager.isToAddCircle)
            {
                Instantiate(m_GameManager.circlePrefab, new Vector3(hitpoint.x, hitpoint.y, hitpoint.z + 0.11f), Quaternion.Euler(-90f, 0f, 0f));
            }
        }
    }

    public void CreateHole(Vector3 positionToDeform, float radius)
    {
        Vector3 hitpoint = positionToDeform;
        positionToDeform = transform.InverseTransformPoint(positionToDeform);
        bool somethingDeformed = false;

        for (int i = 0; i < m_Verts.Length; i++)
        {
            float dist = (m_Verts[i] - positionToDeform).sqrMagnitude;

            if (dist < radius)
            {
                m_Verts[i] -= Vector3.up * m_GameManager.powerOfDeformation;
                somethingDeformed = true;
            }

        }
        if (somethingDeformed)
        {
            m_Mesh.vertices = m_Verts;
            m_MeshCollider.sharedMesh = m_Mesh;
            if (m_GameManager.isToAddCircle)
            {
                Instantiate(m_GameManager.circlePrefab, new Vector3(hitpoint.x, hitpoint.y, hitpoint.z + 0.11f), Quaternion.Euler(-90f, 0f, 0f));
            }
        }
    }
}
