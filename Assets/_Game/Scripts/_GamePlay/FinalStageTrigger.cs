using UnityEngine;

public class FinalStageTrigger : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private ParticleSystem m_ConfettiPS;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != PhysicsLayers.Ball) { return; }
        if (!other.CompareTag("Active")) { return; }

        Ball ball = other.GetComponent<Ball>();
        if (ball == null) { return; }

        ball.isEnteredFinalStage = true;

        m_GameManager.finalStagedBallCount++;

        if (!m_ConfettiPS.isPlaying)
        {
            m_ConfettiPS.Play();
        }
    }
}
