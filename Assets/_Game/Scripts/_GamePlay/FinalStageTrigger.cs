using UnityEngine;

public class FinalStageTrigger : MonoBehaviour
{
    private GameManager m_GameManager;
    private LevelController m_LevelController;
    private UIController m_UIController;

    [SerializeField] private ParticleSystem m_ConfettiPS;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_LevelController = FindObjectOfType<LevelController>();
        m_UIController = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != PhysicsLayers.Ball) { return; }
        if (!other.CompareTag("Active")) { return; }

        Ball ball = other.GetComponent<Ball>();
        if (ball == null) { return; }

        ball.isEnteredFinalStage = true;

        m_LevelController.FinalStagedBallCount++;

        if (m_LevelController.IsPassedResultThreshold()) 
        {
            GameManager.isPassedThreshold = true;
            m_UIController.ShowGameResultButtons();
        }

        if (!m_ConfettiPS.isPlaying)
        {
            m_ConfettiPS.Play();
        }
    }
}
