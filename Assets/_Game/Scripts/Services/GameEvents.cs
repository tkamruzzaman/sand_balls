using System;

namespace Service
{
    internal class GameEvents
    {
        //GameService
        public static Action OnGameServiceInitialized = delegate { };

        //Navigation
        public static Action OnSceneLoadStarted = delegate { };
        public static Action OnSceneLoadFinished = delegate { };
        public static Action OnFakeSceneLoadStarted = delegate { };
        public static Action OnFakeSceneLoadFinished = delegate { };

        //Sound
        public static Action OnSoundToggled = delegate { };

        //Vibration
        internal static Action OnVibrationToggled = delegate { };

        //GameController
        public static Action OnGameStart = delegate { };
        public static Action<bool> OnGameEnd = delegate { };
    }
}