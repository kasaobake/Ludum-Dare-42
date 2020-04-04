using System.Collections;
using System.Collections.Generic;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneController : MonoBehaviour
{
    #region MonoBehaviour Functions
    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            LoadGameplayScene();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            LoadSettingsScene();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            LoadLeaderboardScene();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Quit();
        }
    }
    #endregion

    #region Public Functions
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        SceneTransitionManager.Instance.LoadScene(scene);
    }

    public void LoadGameplayScene()
    {
        LoadScene("Gameplay");
    }

    public void LoadSettingsScene()
    {
        LoadScene("Settings");
    }

    public void LoadLeaderboardScene()
    {
    }
    #endregion
}