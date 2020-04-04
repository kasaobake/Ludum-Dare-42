using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeaponComponent : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private WeaponController startingWeapon;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private WeaponController equippedWeapon;
    [SerializeField]
    public Transform hand;
    #endregion

    #region Properties
    public float WeaponHeight
    {
        get
        {
            return this.hand.position.y;
        }
    }
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.gameplay == null)
        {
            this.gameplay = GameObject.Find("Gameplay").GetComponent<GameplayController>();
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

        if (this.startingWeapon != null)
        {
            Equip(this.startingWeapon);
        }

        this.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(Kameosa.Constants.Input.MOUSE_LEFT))
        {
            this.equippedWeapon.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(Kameosa.Constants.Input.MOUSE_LEFT))
        {
            this.equippedWeapon.OnTriggerRelease();
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
    public void Equip(WeaponController weapon) 
    {
        if (this.equippedWeapon != null)
        {
            Destroy(this.equippedWeapon.gameObject);
        }

        this.equippedWeapon = Instantiate(weapon, this.hand.position, this.hand.rotation);
        this.equippedWeapon.transform.SetParent(this.hand);
    }
    #endregion

    #region Listeners
    private void OnGameStart()
    {
        this.enabled = true;
    }

    private void OnGamePause()
    {
        this.enabled = false;
    }

    private void OnGameResume()
    {
        this.enabled = true;
    }

    private void OnGameWon()
    {
    }

    private void OnGameLose()
    {
    }

    private void OnGameEnd()
    {
        this.enabled = false;
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
