using System;
using System.Collections;
using DG.Tweening;
using Kameosa.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Managers
    {
        public class SceneTransitionManager : Singleton<SceneTransitionManager> 
        {
            private const float FADE_IN_TRANSITION_DURATION = 1f;
            private const float FADE_OUT_TRANSITION_DURATION = 0.5f;

            #region Actions
            public event Action<string, string> OnSceneChange;
            #endregion

            #region Private Variables
            private string currentSceneName;
            private UnityEngine.GameObject transitionPanel;
            private Image transitionPanelImage;
            private Color transitionPanelImageDefaultColor = Color.black;
            #endregion

            #region Properties 
            public string CurrentSceneName
            {
                get
                {
                    return this.currentSceneName;
                }
            }

            public static float TransitionDuration
            {
                get
                {
                    return FADE_IN_TRANSITION_DURATION + FADE_OUT_TRANSITION_DURATION;
                }
            }
            #endregion

            #region MonoBehaviour Functions
            protected override void Awake()
            {
                base.Awake();

                Transform transitionPanelTransform = this.transform.Find("TransitionPanel");

                if (transitionPanelTransform != null)
                {
                    this.transitionPanel = transitionPanelTransform.gameObject;
                    this.transitionPanelImage = this.transitionPanel.GetComponent<Image>();
                    this.transitionPanelImageDefaultColor.a = 0f;
                }
            }

            private void Start()
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                //OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();

                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
            #endregion

            #region Public Functions
            public void LoadScene(string scene)
            {
                FadeIn(scene);
            }

            public void ReloadScene()
            {
                FadeIn(SceneManager.GetActiveScene().name);
            }
            #endregion

            #region Private Functions
            private void FadeIn(string scene)
            {
#if UNITY_EDITOR
                if (this.transitionPanel == null)
                {
                    SceneManager.LoadScene(scene);
                    return;
                }
#endif
                Sequence sequence = DOTween.Sequence();

                sequence.AppendCallback(() =>
                {
                    this.transitionPanel.SetActive(true);
                });
                sequence.Append(this.transitionPanelImage.DOFade(1f, FADE_IN_TRANSITION_DURATION));
                sequence.AppendCallback(() =>
                {
                    SceneManager.LoadScene(scene);
                    FadeOut();
                });
            }

            private void FadeOut()
            {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(this.transitionPanelImage.DOFade(0f, FADE_OUT_TRANSITION_DURATION));
                sequence.AppendCallback(() =>
                {
                    this.transitionPanel.SetActive(false);
                });
            }
            #endregion

            #region Listeners
            private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
            {
                if (scene.name != this.currentSceneName)
                {
                    if (OnSceneChange != null)
                    {
                        OnSceneChange(this.currentSceneName, scene.name);
                    }

                    this.currentSceneName = scene.name;
                }
            }
            #endregion
        }
    }
}