using UnityEngine;

public class InputController : MonoBehaviour
{
    [Range(30, 70)] public float planeDistance;

    private Camera m_mainCamera;

    private bool m_CanReadInput;
    private LevelController m_LevelController;
    private Level m_CurrentLevel;

    private void Awake() => m_CanReadInput = false;

    private void OnEnable() => Service.GameEvents.OnLevelSpawned += ReadyForInput;

    private void OnDisable() => Service.GameEvents.OnLevelSpawned -= ReadyForInput;

    private void ReadyForInput() => m_CanReadInput = true;

    private void Start()
    {
        m_mainCamera = FindObjectOfType<Camera>();
        m_LevelController = FindObjectOfType<LevelController>();
        m_CurrentLevel = m_LevelController.GetCurrentLevel();
    }

    private void FixedUpdate()
    {
        if (m_CanReadInput && Input.GetMouseButton(0))
        {
            Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                m_CurrentLevel.DoformMesh(hit, planeDistance);
            }
        }
    }
}
