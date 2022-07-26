using Service;
using UnityEngine;
using UnityEngine.UI;

public class UIVibrationToggle : MonoBehaviour
{
    [SerializeField] private Button vibrationButton;
    [SerializeField] private Image vibrationImage;

    private void Awake()
    {
        vibrationButton.onClick.AddListener(() => { VibrationButtonAction(); });
    }

    private void OnEnable() => GameEvents.OnVibrationToggled += OnVibrationToggle;

    private void OnDisable() => GameEvents.OnVibrationToggled -= OnVibrationToggle;

    private void Start() => OnVibrationToggle();

    private void OnVibrationToggle()
    {
        if (vibrationImage == null) { return; }

        if (VibrationManager.isVibrationOn)
        {
            vibrationImage.sprite = GameService.Instance.VibrationManager.vibrationOnSprite;
            vibrationImage.color = GameService.Instance.VibrationManager.vibrationActiveColor;
        }
        else
        {
            vibrationImage.sprite = GameService.Instance.VibrationManager.vibrationOffSprite;
            vibrationImage.color = GameService.Instance.VibrationManager.vibrationDeactiveColor;
        }
    }

    private void VibrationButtonAction()
    {
        GameService.Instance.VibrationManager.ToggleVibration();

        GameService.Instance.VibrationManager.HapticHeavy();
    }
}
