using Service;
using UnityEngine;
using UnityEngine.UI;

public class UISoundToggle : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Image soundImage;

    private void Awake()
    {
        soundButton.onClick.AddListener(() => { SoundButtonAction(); });
    }

    private void OnEnable() => GameEvents.OnSoundToggled += OnSoundToggled;

    private void OnDisable() => GameEvents.OnSoundToggled -= OnSoundToggled;

    private void Start() => OnSoundToggled();

    private void OnSoundToggled()
    {
        if (soundImage == null) { return; }

        if (SoundManager.isSoundOn)
        {
            soundImage.sprite = GameService.Instance.SoundManager.soundOnSprite;
            soundImage.color = GameService.Instance.SoundManager.soundActiveColor;
        }
        else
        {
            soundImage.sprite = GameService.Instance.SoundManager.soundOffSprite;
            soundImage.color = GameService.Instance.SoundManager.soundDeactiveColor;
        }
    }

    private void SoundButtonAction()
    {
        GameService.Instance.SoundManager.ToggleSound();

        GameService.Instance.SoundManager.PlayButtonSound();
    }
}
