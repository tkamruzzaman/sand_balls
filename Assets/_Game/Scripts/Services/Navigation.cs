using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Service
{
    public enum Scenes
    {
        Initialize = 0,
        MainMenu = 1,
        GamePlay = 2,
    }

    public class Navigation : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image loadingPanel;

        [SerializeField] private float loadingTime = 0.2f;

        private bool isInLoading;

        private bool isClicked = false;

        private Queue<Action> fadeInOutTasks = new Queue<Action>();
        private bool IsFadeInOutBusy;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            canvas.gameObject.SetActive(false);
            loadingPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) { LoadScene(GetCurrentScene()); }
        }
#endif

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isClicked = false;
            FadeOut();
        }

        /// <summary>
        /// To get the current/active scene from SCENE enum
        /// </summary>
        /// <returns></returns>
        public Scenes GetCurrentScene() => (Scenes)SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// Returns true if current scene is initialize scene
        /// </summary>
        /// <returns></returns>
        public bool IsInInitializeScene() => GetCurrentScene() == Scenes.Initialize;

        /// <summary>
        /// Returns true if current scene is main menu
        /// </summary>
        /// <returns></returns>
        public bool IsInMainMenuScene() => GetCurrentScene() == Scenes.MainMenu;

        /// <summary>
        /// Returns true if current scene is game scene
        /// </summary>
        /// <returns></returns>
        public bool IsInGameScene() => GetCurrentScene() == Scenes.GamePlay;

        /// <summary>
        /// Load scene with parameter from SCENE enum and an optional scene load delay. To get active scene call GetCurrentScene()
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScene(Scenes scene, float loadDelay = 0.0f, bool isToIgnoreFade = false)
        {
            if (isClicked) { return; }

            isClicked = true;
            FadeIn((int)scene, loadDelay, isToIgnoreFade);
        }

        private void FadeIn(int sceneIndex, float delay = 0.0F, bool isToIgnoreFade = false)
        {
            GameEvents.OnSceneLoadStarted?.Invoke();
            isInLoading = true;

            canvas.gameObject.SetActive(true);
            loadingPanel.gameObject.SetActive(true);
            FadeEffect(isToIgnoreFade ? 0 : 1, isToIgnoreFade ? 0 : loadingTime, delay, Ease.OutExpo,
                (onComplete) =>
                {
                    SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                });
        }

        private void FadeOut()
        {
            FadeEffect(0, loadingTime, 0, Ease.OutQuad,
                (onComplete) =>
                {
                    isInLoading = false;
                    loadingPanel.gameObject.SetActive(false);
                    canvas.gameObject.SetActive(false);
                    GameEvents.OnSceneLoadFinished?.Invoke();
                });
        }

        /// <summary>
        /// Fades the in out mimicing a fake scene load effect
        /// </summary>
        /// <param name="Callback">Callback Fucntion that will execute in between the fade in out.</param>
        public void FadeInOut(Action Callback = null)
        {
            canvas.gameObject.SetActive(true);
            if (IsFadeInOutBusy)
            {
                Action EnqueuedCallback = () =>
                {
                    FadeInOut(Callback);
                };
                fadeInOutTasks.Enqueue(EnqueuedCallback);
                return;
            }
            IsFadeInOutBusy = true;
            GameEvents.OnFakeSceneLoadStarted?.Invoke();
            loadingPanel.gameObject.SetActive(true);
            FadeEffect(1, loadingTime, 0, Ease.OutExpo,
                (onComplete) =>
                {
                    Callback?.Invoke();

                    FadeEffect(0, loadingTime, 0, Ease.OutQuad,
                        (onComplete) =>
                        {
                            loadingPanel.gameObject.SetActive(false);
                            GameEvents.OnFakeSceneLoadFinished?.Invoke();
                            IsFadeInOutBusy = false;
                            canvas.gameObject.SetActive(false);
                            if (fadeInOutTasks.Count > 0)
                            {
                                Action DequeuedCallback = fadeInOutTasks.Dequeue();
                                DequeuedCallback();
                            }
                        });
                });
        }

        private void FadeEffect(float endValue, float loadingTime, float delay, Ease ease, Action<bool> onComplete)
        {
            //Debug.Log("+++++++++++++++++++ FadeEffect Started ++++++++++++++++++++");
            loadingPanel.DOFade(endValue, loadingTime).SetEase(ease).SetDelay(delay).SetUpdate(true).OnComplete(() =>
            {
                //Debug.Log("------------------- FadeEffect End ----------------------");
                onComplete(true);
            });
        }
    }
}