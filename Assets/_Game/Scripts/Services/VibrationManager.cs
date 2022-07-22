using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Service
{
    public class VibrationManager : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite vibrationOnSprite;
        public Sprite vibrationOffSprite;

        [Header("Colors")]
        public Color vibrationActiveColor;
        public Color vibrationDeactiveColor;

        public static bool isVibrationOn;
        private const int VIBRATION_ON_STATE = 0;
        private const int VIBRATION_OFF_STATE = 1;

        //keys
        private const string KEY_VIBRATION = "key_vibration";

        private void Awake()
        {
            isVibrationOn = (PlayerPrefs.GetInt(KEY_VIBRATION) == VIBRATION_ON_STATE);
        }

        public void HapticLight()
        {
            TriggerVibration(HapticTypes.LightImpact);
        }

        public void HapticMedium()
        {
            TriggerVibration(HapticTypes.MediumImpact);
        }

        public void HapticHeavy()
        {
            TriggerVibration(HapticTypes.HeavyImpact);
        }

        public void HapticFail()
        {
            TriggerVibration(HapticTypes.Failure);
        }

        public void HapticSuccess()
        {
            TriggerVibration(HapticTypes.Success);
        }

        private void TriggerVibration(HapticTypes hapticType)
        {
            if (isVibrationOn)
            {
#if UNITY_IOS
        if(MMVibrationManager.HapticsSupported())
#endif
                MMVibrationManager.Haptic(hapticType);
            }
        }

        /// <summary>
        /// Toggle the vibration
        /// </summary>
        public void ToggleVibration()
        {
            if (isVibrationOn)
            {
                isVibrationOn = false;
                PlayerPrefs.SetInt(KEY_VIBRATION, VIBRATION_OFF_STATE);
            }
            else
            {
                isVibrationOn = true;
                PlayerPrefs.SetInt(KEY_VIBRATION, VIBRATION_ON_STATE);
            }
            GameEvents.OnVibrationToggled?.Invoke();
        }
    }
}