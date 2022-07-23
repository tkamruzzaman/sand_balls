using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private GameManager m_GameManager;
    [SerializeField] private float m_Speed = 1.5f;
    private float m_LowestBallY;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_LowestBallY = transform.position.y;
    }

    private void Update()
    {
        float lowestActiveBall = m_LowestBallY;
        for (int i = 0; i < m_GameManager.activeBalls.Count; i++)
        {
            float y = m_GameManager.activeBalls[i].transform.position.y;
            if (y < lowestActiveBall)
            {
                lowestActiveBall = y;
            }
        }

        if (lowestActiveBall != m_LowestBallY)
        {
            m_LowestBallY = lowestActiveBall;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, m_LowestBallY, transform.position.z), Time.deltaTime * m_Speed);
            // this.transform.position = new Vector3(this.transform.position.x, lowestBallY, this.transform.position.z);
        }
    }
}
