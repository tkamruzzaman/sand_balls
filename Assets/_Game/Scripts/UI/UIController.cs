using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [SerializeField] private Button m_RestartButton;
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_PauseCloseButton;

    [Space]
    [SerializeField] private RectTransform m_PasusePanelRect;
    [SerializeField] private RectTransform m_GameOverPanel;


    void Awake()
    {
        m_RestartButton.onClick.AddListener(()=>{ RestartButtonAction(); });
        m_PauseButton.onClick.AddListener(() => { PauseButtonAction(); });
        m_ResumeButton.onClick.AddListener(() => { ResumeButtonAction(); });
        m_PauseCloseButton.onClick.AddListener(() => { ResumeButtonAction(); });

        PausePanelStatus(false);
    }

    private void RestartButtonAction() { Service.GameService.Instance.Navigation.LoadScene(Service.Scenes.GamePlay, 0); }
    private void PauseButtonAction() { PausePanelStatus(true); }
    private void ResumeButtonAction() { PausePanelStatus(false); }

    private void PausePanelStatus(bool status)
    {
        m_PasusePanelRect.gameObject.SetActive(status);
    }
}
