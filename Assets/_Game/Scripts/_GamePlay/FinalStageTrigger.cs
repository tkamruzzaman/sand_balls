using UnityEngine;
using Service;

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
        if (!other.TryGetComponent<Ball>(out Ball ball)) { return; }

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

        GameService.Instance.SoundManager.PlaySound(GameService.Instance.SoundManager.winAudioClip, 0.5f);
        GameService.Instance.VibrationManager.HapticMedium();
    }
}
