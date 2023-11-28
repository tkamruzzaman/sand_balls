using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Level m_CurrentLevel;

    public string currentLevelName;

    private int maxLevelCount = 5;

    private static int lastLevelIndex;

    public bool IsLevelSucceed { get; set; }
    public bool IsPlayingRandomLevel { get; set; }

    private int m_FinalStagedBallCount;

    public int FinalStagedBallCount { get => m_FinalStagedBallCount; set => m_FinalStagedBallCount = value; }

    [Header("Debug Level")]
    public int debugLevel = -1;
    public bool isToDebugLevel;
    public GameObject debugLevelPrefab;
    public TMPro.TMP_Text levelDebugText;

    private const string KEY_LEVEL = "key_level";

    [SerializeField] private GameObject[] levels;


    private void Start()
    {
        maxLevelCount = levels.Length;
    }

    public void LoadLevel()
    {
        int levelIndex = GetCurrentLevelIndex();

        //Debug.Log("levelIndex: " + levelIndex);
        //Debug.Log("maxLevelCount: " + maxLevelCount);

        IsPlayingRandomLevel = false;

        if (levelIndex >= maxLevelCount)
        {
            if (GameManager.isToRetryLevel)
            {
                GameManager.isToRetryLevel = false;
                levelIndex = lastLevelIndex;
            }
            else
            {
                IsPlayingRandomLevel = true;
                levelIndex = Random.Range(0, maxLevelCount);
            }
        }

#if UNITY_EDITOR
        if (debugLevel > -1) { levelIndex = debugLevel; }
#endif
        GameObject prefab = levels[levelIndex];

#if UNITY_EDITOR
        if (isToDebugLevel) { prefab = debugLevelPrefab; }
#endif
        //levelDebugText.text = "debug: origin_level_" + (levelIndex);
        GameObject level = Instantiate(prefab);
        level.name = "Level_" + levelIndex;
        level.transform.position = new Vector3(0, 0, 0);

        SetCurrentLevel(level.GetComponent<Level>());
        currentLevelName = "level_" + (levelIndex + 1).ToString();
        lastLevelIndex = levelIndex;
        level.SetActive(true);
    }

    #region Level Work
    public Level GetCurrentLevel() => m_CurrentLevel;
    public void SetCurrentLevel(Level level) => m_CurrentLevel = level;

    public int GetCurrentLevelIndex() => PlayerPrefs.GetInt(KEY_LEVEL, 1);
    private void SetCurrentLevelIndex(int value) => PlayerPrefs.SetInt(KEY_LEVEL, value);

    public void CheckToUnlockLevel(bool winStatus, bool isSkipedLevelWithAd = false)
    {
        //Debug.Log("............winStatus.......... " + winStatus);
        //Debug.Log("............isSkipedLevelWithAd.......... " + isSkipedLevelWithAd);
        if (!winStatus && !isSkipedLevelWithAd) { return; }
        UnlockLevel();
    }

    public void UnlockLevel()
    {
        int currentLevelIndex = GetCurrentLevelIndex();

        currentLevelIndex += 1;

        SetCurrentLevelIndex(currentLevelIndex);
    }
    #endregion

    public int CalculateLevelStar()
    {
        int star = 0;
        float successRate = 0;
        try
        {
            successRate = FinalStagedBallCount / (float)GetCurrentLevel().TotalBalls;
        }
        catch { }

        if (successRate >= 0.67f) { star = 3; }
        else if (successRate >= 0.34f && successRate < 0.67f) { star = 2; }
        else if (successRate < 0.34f) { star = 1; }

        return star;
    }

    public int CalculateLevelFinalProgress()
    {
        int before = GetCurrentLevel().TotalBalls;

        int after = FinalStagedBallCount;

        int remaining = before - after;

        int percentage = 0;
        try
        {
            percentage = remaining * 100 / before;
        }
        catch { }

        return percentage;
    }

    public string GetCurrentLevelName() => "level_" + (GetCurrentLevelIndex() + 1).ToString();

    public void UpdateLevelProgress()
    {
        //float fillAmount = Mathf.Lerp(0, 1.0f,
        //    (m_EnemyController.StartEnemyCount - m_EnemyController.ActiveEnemyCount)
        //    / (float)m_EnemyController.StartEnemyCount);
        //Controller.self.uiController.gamePlayUI.UpdateProgressBar(fillAmount);
    }

    public bool IsPassedResultThreshold()
    {
        int total = GetCurrentLevel().TotalBalls;
        int passingThreshold = 10;
        try
        {
             passingThreshold = total / 3;
        }
        catch { }

        if(FinalStagedBallCount > passingThreshold)
        {
            return true;
        }
        return false;
    }
}
