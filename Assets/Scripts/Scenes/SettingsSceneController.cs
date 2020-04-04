using System.Collections;
using System.Collections.Generic;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSceneController : MonoBehaviour
{
    #region Inspector Variables
    [Header("References")]
    [SerializeField]
    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToggle;
    #endregion

    #region Properties
    public float MasterVolume
    {
        get
        {
            return AudioManager.Instance.MasterVolume;
        }

        set
        {
            AudioManager.Instance.MasterVolume = value;
        }
    }

    public float MusicVolume
    {
        get
        {
            return AudioManager.Instance.MusicVolume;
        }

        set
        {
            AudioManager.Instance.MusicVolume = value;
        }
    }

    public float EffectVolume
    {
        get
        {
            return AudioManager.Instance.EffectVolume;
        }

        set
        {
            AudioManager.Instance.EffectVolume = value;
        }
    }

    public int ResolutionIndex
    {
        get
        {
            return ResolutionManager.Instance.ResolutionIndex;
        }

        set
        {
            ResolutionManager.Instance.ResolutionIndex = value;
        }
    }

    public bool IsFullScreen
    {
        get
        {
            return ResolutionManager.Instance.IsFullScreen;
        }

        set
        {
            SetResolutionTogglesInteractable(!value);
            ResolutionManager.Instance.IsFullScreen = value;
        }
    }
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
    }

    private void Start()
    {
        this.volumeSliders[0].value = AudioManager.Instance.MasterVolume;
        this.volumeSliders[1].value = AudioManager.Instance.MusicVolume;
        this.volumeSliders[2].value = AudioManager.Instance.EffectVolume;

        for (int i = 0; i < this.resolutionToggles.Length; i++)
        {
            this.resolutionToggles[i].isOn = i == ResolutionManager.Instance.ResolutionIndex;
        }

        this.fullscreenToggle.isOn = ResolutionManager.Instance.IsFullScreen;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            LoadMainMenu();
        }
    }
    #endregion

    #region Public Functions
    public virtual void LoadScene(string scene)
    {
        SceneTransitionManager.Instance.LoadScene(scene);
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }
    #endregion

    #region Private Functions
    private void SetResolutionTogglesInteractable(bool isInteractable)
    {
        for (int i = 0; i < this.resolutionToggles.Length; i++)
        {
            this.resolutionToggles[i].interactable = isInteractable;
        }
    }
    #endregion
}
