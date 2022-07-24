using Service;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static bool isInGamePlay;
    internal static bool isToRetryLevel;
    internal static bool isInGameOver;

    [SerializeField] private LevelController m_LevelController;

    public List<Ball> activeBalls;

    [Range(0.25f, 2.0f)] public float radiusOfDeformation = 0.7f;
    [Range(1.5f, 3.0f)] public float powerOfDeformation = 2.0f;
    public GameObject circlePrefab;
    public bool isToAddCircle;

    public int finalStagedBallCount;

    private void Awake()
    {
        DoReset();
    }

    private void DoReset()
    {
        Time.timeScale = 1;

        isInGamePlay = false;
        isInGameOver = false;
        isToRetryLevel = false;

        DoPlay();
    }

    public void DoPlay()
    {
        if (isInGamePlay) { return; }

        isInGamePlay = true;

        m_LevelController.LoadLevel();

        GameEvents.OnGameStart?.Invoke();
    }

    public void DoGameOver(bool status)
    {
        //Debug.Log("DoGameOver 1  " + status);
        if (isInGameOver) { return; }

        isToRetryLevel = !status;

        //Debug.Log("DoGameOver 2  " + status);

        isInGamePlay = false;
        isInGameOver = true;

        m_LevelController.IsLevelSucceed = status;

        //uiController.ShowGameOverUI();

        m_LevelController.CheckToUnlockLevel(status);

        GameEvents.OnGameEnd?.Invoke(status);
    }

    public void DoPause()
    {
        Time.timeScale = 0;
    }

    public void DoResume()
    {
        Time.timeScale = 1;
    }

    public void DoReplay()
    {
        GameService.Instance.Navigation.LoadScene(Scenes.GamePlay);
    }

    public void MainMenu()
    {
        GameService.Instance.Navigation.LoadScene(Scenes.MainMenu);
    }

}
