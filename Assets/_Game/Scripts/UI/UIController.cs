using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button m_RestartButton;
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_PauseCloseButton;

    [Space]
    [SerializeField] private RectTransform m_GamePlayPanelRect;
    [SerializeField] private RectTransform m_PasusePanelRect;
    [SerializeField] private RectTransform m_GameOverPanelRect;

    private GameManager m_GameManager;

    private void Awake()
    {
        m_RestartButton.onClick.AddListener(() => { RestartButtonAction(); });
        m_PauseButton.onClick.AddListener(() => { PauseButtonAction(); });
        m_ResumeButton.onClick.AddListener(() => { ResumeButtonAction(); });
        m_PauseCloseButton.onClick.AddListener(() => { ResumeButtonAction(); });

        GamePlayPanelStatus(true);
        PausePanelStatus(false);
        GameOverPanelStatus(false);
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    private void RestartButtonAction()
    {
        m_GameManager.DoReplay();
    }
    private void PauseButtonAction()
    {
        m_GameManager.DoPause();
        PausePanelStatus(true);
    }
    private void ResumeButtonAction()
    {
        m_GameManager.DoResume();
        PausePanelStatus(false);
    }

    private void GamePlayPanelStatus(bool status) => m_GamePlayPanelRect.gameObject.SetActive(status);
    private void PausePanelStatus(bool status) => m_PasusePanelRect.gameObject.SetActive(status);
    private void GameOverPanelStatus(bool status) => m_GameOverPanelRect.gameObject.SetActive(status);

    public void ShowGameOverUI()
    {
        GamePlayPanelStatus(false);
        GameOverPanelStatus(true);
    }
}
