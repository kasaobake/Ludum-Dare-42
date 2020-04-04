using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    #region Inspector Variables
    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private UnityEngine.GameObject pausePanel;
    [SerializeField]
    private UnityEngine.GameObject gameEndText;
    [SerializeField]
    private TitleController scoreTitle;
    #endregion

    #region Private Variables
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
	{
        if (this.gameplay == null)
        {
            this.gameplay = GameObject.Find("Gameplay").GetComponent<GameplayController>();
        }

        if (this.pauseButton == null)
        {
            UnityEngine.GameObject temp = UnityEngine.GameObject.Find("PauseButton");

            if (temp != null)
            {
                this.pauseButton = temp.GetComponent<Button>();
                this.pauseButton.onClick.AddListener(GamePause);
            }
        }

        if (this.resumeButton == null)
        {
            this.resumeButton = UnityEngine.GameObject.Find("ResumeButton").GetComponent<Button>();
            this.resumeButton.onClick.AddListener(GameResume);
        }

        if (this.quitButton == null)
        {
            this.quitButton = UnityEngine.GameObject.Find("QuitButton").GetComponent<Button>();
            this.quitButton.onClick.AddListener(GameQuit);
        }

        if (this.pausePanel == null)
        {
            this.pausePanel = UnityEngine.GameObject.Find("PausePanel");
        }
	}

    private void Start()
    {
        this.gameplay.OnGameStart += OnGameStart;
        this.gameplay.OnGamePause += OnGamePause;
        this.gameplay.OnGameResume += OnGameResume;
        this.gameplay.OnGameWon += OnGameWon;
        this.gameplay.OnGameLose += OnGameLose;
        this.gameplay.OnGameEnd += OnGameEnd;
        this.gameplay.OnGameRestart += OnGameRestart;
        this.gameplay.OnGameQuit += OnGameQuit;

        SetPausePanelActive(false);
        SetGameEndPanelActive(false);

        this.enabled = false;
    }

    private void Update()
    {
        this.scoreTitle.Text = this.gameplay.Score.ToString("D6");

        if (this.pausePanel.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.M))
            {
                GameQuit();
            }
            else if (Input.GetKey(KeyCode.R))
            {
                if (this.resumeButton.gameObject.activeInHierarchy)
                {
                    GameResume();
                }
                else if (this.restartButton.gameObject.activeInHierarchy)
                {
                    GameRestart();
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.P))
            {
                GamePause();
            }
        }
    }

    private void OnDestroy()
    {
        if (this.gameplay != null)
        {
            this.gameplay.OnGameStart -= OnGameStart;
            this.gameplay.OnGamePause -= OnGamePause;
            this.gameplay.OnGameResume -= OnGameResume;
            this.gameplay.OnGameWon -= OnGameWon;
            this.gameplay.OnGameLose -= OnGameLose;
            this.gameplay.OnGameEnd -= OnGameEnd;
            this.gameplay.OnGameRestart -= OnGameRestart;
            this.gameplay.OnGameQuit -= OnGameQuit;
        }
    }
    #endregion

    #region Public Functions
    public void GameStart()
    {
        this.gameplay.GameStart();
    }

    public void GamePause()
    {
        this.gameplay.GamePause();
    }

    public void GameResume()
    {
        this.gameplay.GameResume();
    }

    public void GameRestart()
    {
        this.gameplay.GameRestart();
    }

    public void GameQuit()
    {
        this.gameplay.GameQuit();
    }
    #endregion

    #region Private Functions
    private void SetPauseButtonActive(bool isActive)
    {
        if (this.pauseButton)
        {
            this.pauseButton.gameObject.SetActive(isActive);
        }
    }

    private void SetPausePanelActive(bool isActive)
    {
        if (this.pausePanel)
        {
            this.pausePanel.SetActive(isActive);
        }
    }

    private void SetGameEndPanelActive(bool isActive)
    {
        SetPausePanelActive(isActive);
        this.gameEndText.SetActive(isActive);
        this.restartButton.gameObject.SetActive(isActive);
        this.resumeButton.gameObject.SetActive(!isActive);
    }
    #endregion

    #region Listeners
    private void OnGameStart()
    {
        this.enabled = true;
    }

    private void OnGamePause()
    {
        SetPauseButtonActive(false);
        SetPausePanelActive(true);
    }

    private void OnGameResume()
    {
        SetPauseButtonActive(true);
        SetPausePanelActive(false);
    }

    private void OnGameWon()
    {
    }

    private void OnGameLose()
    {
    }

    private void OnGameEnd()
    {
        SetPauseButtonActive(false);
        SetGameEndPanelActive(true);
    }

    private void OnGameRestart()
    {
    }

    private void OnGameQuit()
    {
        this.enabled = false;
    }
    #endregion
}