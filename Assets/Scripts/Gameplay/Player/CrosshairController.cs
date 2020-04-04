using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private float rotationVelocity;
     [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private Color dotHighlightColour;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private Transform ring;
    [SerializeField]
    private SpriteRenderer dotRenderer;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private EquipWeaponComponent equipWeaponComponent;
    #endregion

    #region Private Variables
    private Color originalDotColour;
    private Vector3 position;
    private bool isVisible;
    #endregion

    #region Properties
    public Vector3 Position
    {
        get
        {
            return this.position;
        }

        set
        {
            this.position = value;
        }
    }

    public bool IsVisible
    {
        get
        {
            return this.isVisible;
        }

        set
        {
            this.isVisible = value;
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

        if (this.equipWeaponComponent == null)
        {
            this.equipWeaponComponent = GetComponent<EquipWeaponComponent>();
        }

        if (this.player == null)
        {
            this.player = GameObject.Find("Player").GetComponent<PlayerController>();
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

        this.player.OnDie += OnPlayerDie;

        Cursor.visible = false;
        this.originalDotColour = this.dotRenderer.color;
        this.enabled = false;
    }

    private void Update()
    {
        this.ring.Rotate(Vector3.forward * this.rotationVelocity * Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 height = this.equipWeaponComponent == null ? this.equipWeaponComponent.WeaponHeight * Vector3.up : Vector3.zero;
        Plane groundPlane = new Plane(Vector3.up, height);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            this.position = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, this.position, Color.red);
            this.transform.position = this.position;
            DetectTargets(ray);
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

        if (this.player != null)
        {
            this.player.OnDie -= OnPlayerDie;
        }
    }
    #endregion

    #region Private Functions
    private void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, this.targetMask))
        {
            this.dotRenderer.color = this.dotHighlightColour;
        }
        else
        {
            this.dotRenderer.color = this.originalDotColour;
        }
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

    private void OnPlayerDie()
    {
        this.gameObject.SetActive(false);
        Cursor.visible = true;
    }
    #endregion
}
