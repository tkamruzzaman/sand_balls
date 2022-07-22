using UnityEngine;

/// <summary>
/// Add this component to any object and it'll set the target frame rate and vsync count. Note that vsync count must be 0 for the target FPS to work.
/// </summary>
public class TargetFPS : MonoBehaviour
{
    [Range(15, 60)]
    /// the target FPS you want the game to run at
    [SerializeField] private int m_TargetFPS = 30;

    /// whether vsync should be enabled or not (on a 60Hz screen, 1 : 60fps, 2 : 30fps, 0 : don't wait for vsync)
    private int m_VSyncCount = 0;

    /// <summary>
    /// On start we change our target fps and vsync settings
    /// </summary>
    protected virtual void Start()
    {
        UpdateSettings();
    }

    /// <summary>
    /// When a value gets changed in the editor, we update our settings
    /// </summary>
    protected virtual void OnValidate()
    {
        UpdateSettings();
    }

    /// <summary>
    /// Updates the target frame rate value and vsync count setting
    /// </summary>
    protected virtual void UpdateSettings()
    {
        QualitySettings.vSyncCount = m_VSyncCount;
        Application.targetFrameRate = m_TargetFPS;
    }
}
