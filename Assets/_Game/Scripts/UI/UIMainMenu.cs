using Service;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("Main Menu Panel")]
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_SettingsButton;

    [Header("Settings Panel")]
    [SerializeField] private RectTransform m_SettingsRect;
    //[SerializeField] private Button m_AudioButton;
    //[SerializeField] private Button m_VibrationButton;
    [SerializeField] private Button m_SettingsCloseButton;

    private void Awake()
    {
        m_PlayButton.onClick.AddListener(() => { PlayButtonAction(); });
        m_SettingsButton.onClick.AddListener(() => { SettingsButtonAction(); });

        //m_AudioButton.onClick.AddListener(() => { AudioButtonAction(); });
        //m_VibrationButton.onClick.AddListener(() => { VibrationButtonAction(); });
        m_SettingsCloseButton.onClick.AddListener(() => { SettingsCloseButtonAction(); });

        SettingsPanelStatus(false);
    }

    private void PlayButtonAction()
    {
        GameService.Instance.Navigation.LoadScene(Scenes.GamePlay);
    }

    private void SettingsButtonAction()
    {
        SettingsPanelStatus(true);
    }

    //private void AudioButtonAction()
    //{
    //    GameService.Instance.SoundManager.ToggleSound();
    //}

    //private void VibrationButtonAction()
    //{
    //    GameService.Instance.VibrationManager.ToggleVibration();
    //}

    private void SettingsCloseButtonAction()
    {
        SettingsPanelStatus(false);
    }

    private void SettingsPanelStatus(bool status)
    {
        m_SettingsRect.gameObject.SetActive(status);
    }

}
