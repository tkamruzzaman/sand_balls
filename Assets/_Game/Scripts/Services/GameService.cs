using System.Threading.Tasks;
using UnityEngine;

namespace Service
{
    public class GameService : MonoBehaviour
    {
        private static GameService m_Instance;

        public static GameService Instance { get => m_Instance; private set => m_Instance = value; }
        public Navigation Navigation { get; private set; }
        public SoundManager SoundManager { get; private set; }
        public VibrationManager VibrationManager { get; private set; }

        private async Task Awake()
        {
            await MakeServicePersistent();
            SetDebug();
            await GetReferences();
            GameEvents.OnGameServiceInitialized?.Invoke();
        }

        private void OnEnable() => GameEvents.OnGameServiceInitialized += LoadMainMenuScene;

        private void OnDisable() => GameEvents.OnGameServiceInitialized -= LoadMainMenuScene;

        private async Task MakeServicePersistent()
        {
            if (m_Instance == null)
            { m_Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
            await Task.Yield();
        }

        private void SetDebug()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }

        private async Task GetReferences()
        {
            Navigation = GetComponentInChildren<Navigation>();         
            SoundManager = GetComponentInChildren<SoundManager>();
            VibrationManager = GetComponentInChildren<VibrationManager>();
            await Task.Yield();
        }

        private void LoadMainMenuScene()
        {
#if UNITY_EDITOR
            if (!Navigation.IsInInitializeScene()) { return; }
#endif
            Navigation.LoadScene(Scenes.MainMenu, 0, true);
        }
    }
}