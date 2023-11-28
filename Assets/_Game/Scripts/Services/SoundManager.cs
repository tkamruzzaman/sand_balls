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
        public AudioClip buttonAudioClip;
        public AudioClip winAudioClip;
        public AudioClip celebrateAudioClip;
    
        [Space]
        [Header("Game Audio")]
        public AudioClip ballAudioClip;
        public AudioClip sandAudioClip;
        public AudioClip coinAudioClip;

        public static bool isSoundOn;

        private const int SOUND_ON_STATE = 0;
        private const int SOUND_OFF_STATE = 1;

        private const string KEY_SOUND = "key_sound";

        [Space]
        [SerializeField] private AudioSource m_MusicAudioSource;
        [SerializeField] private AudioSource m_SFXAudioSource;

        private void Awake()
        {
            isSoundOn = (PlayerPrefs.GetInt(KEY_SOUND) == SOUND_ON_STATE);

        }
        void Start()
        {
            CheckBackgroundMusic();
        }

        void CheckBackgroundMusic()
        {
            if (isSoundOn)
            {
                m_MusicAudioSource.Stop();
                m_MusicAudioSource.Play();
            }
            else
            {
                m_MusicAudioSource.Stop();
            }
        }

        public void PlaySound(AudioClip audioClip, float volume = 0.5f)
        {
            if (isSoundOn && audioClip != null)
            {
                m_SFXAudioSource.PlayOneShot(audioClip, volume);
            }
        }

        public void PlaySound(AudioSource audioSource)
        {
            if (isSoundOn && audioSource != null)
            {
                audioSource.Play();
            }
        }

        public void PlayButtonSound()
        {
            PlaySound(buttonAudioClip);
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
            CheckBackgroundMusic();
            GameEvents.OnSoundToggled?.Invoke();
        }
    }
}