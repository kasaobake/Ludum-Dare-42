using System;
using System.Collections;
using Kameosa.Common;
using Kameosa.Components;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    private const float COUNT_DOWN_TO_GAME_START = 1f;

    #region Inspector Variables
    [SerializeField]
    private bool isShrinking = true;
    [SerializeField]
    private int timeBetweenShrink = 10;
    [SerializeField]
    private int timeTillNextShrink = 10;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private RoomController room;
    #endregion

    #region Actions
    public event Action OnGameStart;
    public event Action OnGamePause;
    public event Action OnGameResume;
    public event Action OnGameWon;
    public event Action OnGameLose;
    public event Action OnGameEnd;
    public event Action OnGameRestart;
    public event Action OnGameQuit;
    #endregion

    #region Private Variables
    private ScoreComponent scoreComponent;
    #endregion

    #region Properties
    public ScoreComponent ScoreComponent
    {
        get
        {
            return this.scoreComponent;
        }
    }

    public int Score
    {
        get
        {
            return this.scoreComponent.Score;
        }
    }

    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.player == null)
        {
            this.player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        if (this.room == null)
        {
            this.room = GameObject.Find("Room").GetComponent<RoomController>();
        }

        if (this.scoreComponent == null)
        {
            this.scoreComponent = GetComponent<ScoreComponent>();
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        SetupDebugPanel();
#endif
        this.player.OnDie += OnPlayerDie;

        StartCoroutine(CountDownToGameStart());
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        if (this.player != null)
        {
            this.player.OnDie -= OnPlayerDie;
        }
    }
    #endregion

    #region Gamestate Functions
    public void GameStart()
    {
#if UNITY_EDITOR
        Debug.Log("GameStart()");
#endif
        if (OnGameStart != null)
        {
            OnGameStart();
        }

        Time.timeScale = 1f;
        StartCoroutine(CountdownTimeTillNextShrink());
    }

    public void GamePause()
    {
#if UNITY_EDITOR
        Debug.Log("GamePause()");
#endif
        if (OnGamePause != null)
        {
            OnGamePause();
        }

        Time.timeScale = 0f;
    }

    public void GameResume()
    {
#if UNITY_EDITOR
        Debug.Log("GameResume()");
#endif
        if (OnGameResume != null)
        {
            OnGameResume();
        }

        Time.timeScale = 1f;
    }

    private void GameWon()
    {
#if UNITY_EDITOR
        Debug.Log("GameWon()");
#endif
        if (OnGameWon != null)
        {
            OnGameWon();
        }

        GameEnd();
    }

    private void GameLose()
    {
#if UNITY_EDITOR
        Debug.Log("GameLose()");
#endif
        if (OnGameLose != null)
        {
            OnGameLose();
        }

        GameEnd();
    }

    private void GameEnd()
    {
#if UNITY_EDITOR
        Debug.Log("GameEnd()");
#endif
        if (OnGameEnd != null)
        {
            OnGameEnd();
        }

        Time.timeScale = 0f;
    }

    public void GameRestart()
    {
#if UNITY_EDITOR
        Debug.Log("GameRestart()");
#endif
        if (OnGameRestart != null)
        {
            OnGameRestart();
        }

        SceneTransitionManager.Instance.ReloadScene();
        StartCoroutine(UnfreezeTimeScale());
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        Debug.Log("GameQuit()");
#endif
        if (OnGameQuit != null)
        {
            OnGameQuit();
        }

        SceneTransitionManager.Instance.LoadScene("MainMenu");
        StartCoroutine(UnfreezeTimeScale());
    }
    #endregion

    #region Private Functions
    private IEnumerator CountDownToGameStart()
    {
        yield return new WaitForSecondsRealtime(COUNT_DOWN_TO_GAME_START);

        GameStart();
    }

    private IEnumerator CountdownTimeTillNextShrink()
    {
        yield return new WaitForSecondsRealtime(1f);

        this.timeTillNextShrink--;

        if (this.isShrinking && this.timeTillNextShrink <= 0)
        {
            this.room.Shrink();
            this.timeTillNextShrink = this.timeBetweenShrink;
        }

        StartCoroutine(CountdownTimeTillNextShrink());
    }

    private IEnumerator UnfreezeTimeScale()
    {
        yield return new WaitForSecondsRealtime(SceneTransitionManager.TransitionDuration);
        Time.timeScale = 1f;
    }
    #endregion

    #region Listeners
    private void OnPlayerDie()
    {
        GameLose();
    }
    #endregion

    #region Debug Functions
#if UNITY_EDITOR
    private GameObject debugPanel;

    private void SetupDebugPanel()
    {
        GameObject canvas = Util.CreateCanvas("DebugCanvas");
        this.debugPanel = Util.CreatePanel("DebugPanel", canvas);
        this.debugPanel.GetComponent<RectTransform>().anchorMin = new UnityEngine.Vector2(0.1f, 0.1f);
        this.debugPanel.GetComponent<RectTransform>().anchorMax = new UnityEngine.Vector2(0.3f, 0.9f);
        this.debugPanel.SetActive(false);

        GameObject layoutContainer = Util.CreateVerticalLayoutContainer("Buttons", this.debugPanel);

        GameObject gameStartButton = Util.CreateButton("GameStartButton", layoutContainer);
        gameStartButton.GetComponent<Button>().onClick.AddListener(SetGameStart);

        GameObject gameEndButton = Util.CreateButton("GameEndButton", layoutContainer); 
        gameEndButton.GetComponent<Button>().onClick.AddListener(SetGameEnd);

        GameObject gameTogglePauseButton = Util.CreateButton("GameTogglePauseButton", layoutContainer);
        gameTogglePauseButton.GetComponent<Button>().onClick.AddListener(ToggleGamePause);

        GameObject gameRestartButton = Util.CreateButton("GameRestartButton", layoutContainer); 
        gameRestartButton.GetComponent<Button>().onClick.AddListener(SetGameRestart);

        GameObject toggleDebugPanelButton = Util.CreateButton("ToggleDebugPanelButton", canvas); 
        toggleDebugPanelButton.GetComponent<Button>().onClick.AddListener(ToggleDebugPanel);
        toggleDebugPanelButton.GetComponent<RectTransform>().offsetMin = UnityEngine.Vector2.zero;
        toggleDebugPanelButton.GetComponent<RectTransform>().offsetMax = UnityEngine.Vector2.zero;
        toggleDebugPanelButton.GetComponent<RectTransform>().anchorMin = new UnityEngine.Vector2(0.8f, 0.01f);
        toggleDebugPanelButton.GetComponent<RectTransform>().anchorMax = new UnityEngine.Vector2(0.99f, 0.1f);
    }

    public void ToggleDebugPanel()
    {
        if (this.debugPanel.activeInHierarchy)
        {
            this.debugPanel.SetActive(false);
        }
        else
        {
            this.debugPanel.SetActive(true);
        }
    }

    public void SetGameEnd()
    {
        GameEnd();
    }

    public void SetGameStart()
    {
        GameStart();
    }

    public void ToggleGamePause()
    {
        if (Time.timeScale == 0f)
        {
            GameResume();
        }
        else
        {
            GamePause();
        }
    }

    public void SetGameRestart()
    {
        GameRestart();
    }
#endif
    #endregion
}