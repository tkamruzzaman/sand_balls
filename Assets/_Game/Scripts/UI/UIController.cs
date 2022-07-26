using Service;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_GamePlayLevelText;
    [SerializeField] private TMP_Text m_ScoreText;
    [SerializeField] private RectTransform m_GamePlayButtons;
    [SerializeField] private RectTransform m_GameResultButtons;
    [Space]

    [SerializeField] private Button m_RestartButton;
    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_PauseCloseButton;

    [SerializeField] private Button m_ResultRestartButton;
    [SerializeField] private Button m_ResultNextButton;

    [Space]
    [SerializeField] private RectTransform m_GamePlayPanelRect;
    [SerializeField] private RectTransform m_PasusePanelRect;
    [SerializeField] private RectTransform m_GameOverPanelRect;

    [Space]
    [SerializeField] private GameObject[] stars;
    [SerializeField] private TMP_Text m_GameOverLevelText;
    [SerializeField] private Button m_GameOverNextButton;

    private GameManager m_GameManager;
    private LevelController m_LevelController;

    private void Awake()
    {
        m_RestartButton.onClick.AddListener(() => { RestartButtonAction(); });
        m_PauseButton.onClick.AddListener(() => { PauseButtonAction(); });
        m_ResumeButton.onClick.AddListener(() => { ResumeButtonAction(); });
        m_PauseCloseButton.onClick.AddListener(() => { ResumeButtonAction(); });

        m_ResultRestartButton.onClick.AddListener(() => { RestartButtonAction(); });
        m_ResultNextButton.onClick.AddListener(() => { Service.GameEvents.OnPressedResultNextButton?.Invoke(); /*m_GameManager.DoGameOver(true);*/ });
        m_GameOverNextButton.onClick.AddListener(() => { RestartButtonAction(); });

        GamePlayPanelStatus(true);
        PausePanelStatus(false);
        GameOverPanelStatus(false);

        m_GameManager = FindObjectOfType<GameManager>();
        m_LevelController = FindObjectOfType<LevelController>();

        m_GamePlayButtons.gameObject.SetActive(true);
        m_GameResultButtons.gameObject.SetActive(false);

        foreach (GameObject star in stars)
        {
            star.transform.localScale = Vector3.zero;
        }
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_LevelController = FindObjectOfType<LevelController>();
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
        StartCoroutine(IE_ShowStar());
    }

    public void ShowGameResultButtons()
    {
        m_GamePlayButtons.gameObject.SetActive(false);
        m_GameResultButtons.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Service.GameEvents.OnLevelSpawned += SetLevelText;
    }

    private void OnDisable()
    {
        Service.GameEvents.OnLevelSpawned -= SetLevelText;
    }

    private void SetLevelText()
    {
        m_GamePlayLevelText.text = "Level " + m_LevelController.GetCurrentLevelIndex();
        m_GameOverLevelText.text = "Level " + m_LevelController.GetCurrentLevelIndex();
    }

    private readonly WaitForSeconds waitForPointTwo = new(0.2f);

    private IEnumerator IE_ShowStar()
    {
        int star = m_LevelController.CalculateLevelStar();

        for (int i = 0; i < star; i++)
        {
            yield return StartCoroutine(ZoomTo(stars[i], new Vector3(1.5f, 1.5f, 1.5f), 0.2f));
            yield return StartCoroutine(ZoomTo(stars[i], Vector3.one, 0.2f));
            yield return waitForPointTwo;
        }

        GameService.Instance.SoundManager.PlaySound(GameService.Instance.SoundManager.celebrateAudioClip);
        GameService.Instance.VibrationManager.HapticSuccess();
    }

    public static IEnumerator ZoomTo(GameObject gameObject, Vector3 scale, float duration)
    {
        Vector3 startScale = gameObject.transform.localScale;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            gameObject.transform.localScale = Vector3.Lerp(startScale, scale, t);
            yield return null;
        }
    }
}
