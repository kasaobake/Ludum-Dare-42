using System;
using Kameosa.Cartesian;
using Kameosa.Components;
using Kameosa.Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region Inspector Variables
    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private new Rigidbody2D rigidbody2D;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private DamageableComponent damageableComponent;
    #endregion

    #region Actions
    public event Action OnDie;
    public event Action OnTakeDamage;
    #endregion

    #region Properties
    public Coordinate StartCoordinate
    {
        set
        {
            this.transform.position = value;
        }
    }

    public float X
    {
        get
        {
            return this.transform.position.x;
        }
    }

    public float Y
    {
        get
        {
            return this.transform.position.y;
        }
    }

    public DamageableComponent DamageableComponent
    {
        get
        {
            return this.damageableComponent;
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

        if (this.rigidbody2D == null)
        {
            this.rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (this.animator == null)
        {
            this.animator = GetComponent<Animator>();
        }

        if (this.damageableComponent == null)
        {
            this.damageableComponent = GetComponent<DamageableComponent>();
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

        this.damageableComponent.OnTakeDamage += OnComponentTakeDamage;
        this.damageableComponent.OnDie += OnComponentDie;
    }

    private void Update()
    {
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

        if (this.damageableComponent != null)
        {
            this.damageableComponent.OnTakeDamage -= OnComponentTakeDamage;
            this.damageableComponent.OnDie -= OnComponentDie;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
#if UNITY_EDITOR
        Debug.Log("PlayerController OnCollisionEnter2D()");
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
#if UNITY_EDITOR
        Debug.Log("PlayerController OnTriggerEnter2D() collision.tag: " + collision.tag);
#endif
    }
    #endregion

    #region Private Functions
    #endregion

    #region Listeners
    private void OnGameStart()
    {
    }

    private void OnGamePause()
    {
    }

    private void OnGameResume()
    {
    }

    private void OnGameWon()
    {
    }

    private void OnGameLose()
    {
    }

    private void OnGameEnd()
    {
    }

    private void OnGameRestart()
    {
    }

    private void OnGameQuit()
    {
    }

    private void OnComponentTakeDamage(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
    {
        if (OnTakeDamage != null)
        {
            OnTakeDamage();
        }
    }

    private void OnComponentDie(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
    {
#if UNITY_EDITOR
        Debug.Log("PlayerController Die()");
#endif
        if (OnDie != null)
        {
            OnDie();
        }
    }
    #endregion
}