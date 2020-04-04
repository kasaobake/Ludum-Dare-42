using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kameosa.Services;
using UnityEngine.SceneManagement;
using Kameosa.Managers;
using Kameosa.Common;

public class GameManager : Singleton<GameManager>
{
    #region Private Variables
    private GameData gameData;
    #endregion

    #region Properties
    public GameData GameData
    {
        get
        {
            return this.gameData;
        }

        set
        {
            this.gameData = value;
        }
    }
    #endregion

    #region MonoBehaviour Functions
    protected override void Awake()
    {
        base.Awake();

        this.gameData = (GameData)FileService.Load<GameData>();

        if (this.gameData == null)
        {
            InitializeGameData();
        }
    }

    private void Start()
    {
        SceneTransitionManager.Instance.OnSceneChange += OnSceneChange;
        OnSceneChange("", SceneManager.GetActiveScene().name);

        StartCoroutine(AutoSaveGameData());
    }

    private void Update()
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.OnSceneChange -= OnSceneChange;
        }
    }
    #endregion

    #region Public Functions
    public void SaveGameData()
    {
        FileService.Save(this.gameData);
    }
    #endregion

    #region Private Functions
    private void InitializeGameData()
    {
        this.gameData = new GameData();
    }

    private IEnumerator AutoSaveGameData()
    {
        yield return new WaitForSecondsRealtime(this.gameData.AutoSaveInterval);

        if (this.gameData.IsAutoSave)
        {
            SaveGameData();
            StartCoroutine(AutoSaveGameData());
        }
    }
    #endregion

    #region Listeners
    private void OnSceneChange(string oldSceneName, string newSceneName)
    {
        if (newSceneName == "MainMenu")
        {
            if (oldSceneName != "Settings")
            {
                AudioManager.Instance.PlayMusic("MenuTheme");
            }
        }
        else if (newSceneName == "Gameplay")
        {
            AudioManager.Instance.PlayMusic("MainTheme");
        }
    }
    #endregion
}