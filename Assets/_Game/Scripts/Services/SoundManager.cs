using DG.Tweening;
using UnityEngine;

namespace Service
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite soundOnSprite;
        public Sprite soundOffSprite;
        [Header("Colors")]
        public Color soundActiveColor;
        public Color soundDeactiveColor;

        [Space]
        [Header("UI Audio")]
        public AudioSource button;
        public AudioSource win;
        public AudioSource lost;
        public AudioSource celebrate;
        public AudioSource point;
        public AudioSource coinEffect;

        [Space]
        [Header("Game Audio")]
        public AudioClip hitClip;
        public AudioClip hitByArrowClip;
        public AudioClip arrowClip;
        public AudioClip glassBreakClip;

        public static bool isSoundOn;

        private const int SOUND_ON_STATE = 0;
        private const int SOUND_OFF_STATE = 1;

        private const string KEY_SOUND = "key_sound";

        private void Awake()
        {
            isSoundOn = (PlayerPrefs.GetInt(KEY_SOUND) == SOUND_ON_STATE);
        }

        public void PlaySound(AudioSource audio)
        {
            if (isSoundOn && audio != null)
            {
                audio.pitch = 1;
                audio.Play();
            }
        }

        public void PlaySound(AudioSource audio, AudioClip clip)
        {
            if (isSoundOn && audio != null)
            {
                audio.clip = clip;
                audio.pitch = 1;
                audio.Play();
            }
        }

        public void PlayButtonSound()
        {
            PlaySound(button);
        }

        public void ChangePitch(AudioSource source, float pitch, float duration)
        {
            DOTween.To(() => source.pitch, x => source.pitch = x, pitch, duration);
        }


        /// <summary>
        /// Toggle the sound
        /// </summary>
        /// <param name="isSoundOn"></param>
        public void ToggleSound()
        {
            if (isSoundOn)
            {
                isSoundOn = false;
                PlayerPrefs.SetInt(KEY_SOUND, SOUND_OFF_STATE);
            }
            else
            {
                isSoundOn = true;
                PlayerPrefs.SetInt(KEY_SOUND, SOUND_ON_STATE);
            }
            GameEvents.OnSoundToggled?.Invoke();
        }
    }
}