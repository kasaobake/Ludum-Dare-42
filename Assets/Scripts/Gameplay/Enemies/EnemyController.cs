using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kameosa.Components;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(FollowTargetComponent))]
public class EnemyController : MonoBehaviour, IDamageable 
{
    public enum State {
        Idle,
        Chasing,
        Attacking
    };

    #region Inspector Variables
    [SerializeField]
    private int points = 1;
    [SerializeField]
    private GameObject deathEffect;
    [Space(10)]

    [Header("Attacking")]
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float attackDistanceThreshold = 0.4f;
    [SerializeField]
    private float timeBetweenAttacks = 1f;
    [SerializeField]
    private float attackSpeed = 3f;
    [SerializeField]
    private Color attackColor = Color.red;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private DamageableComponent damageableComponent;
    #endregion

    #region Actions
    public static Action<int> OnDieStatic;
    #endregion

    #region Private Variables
    private float attackDistanceThresholdSquared;
    private float timeTillNextAttack;
    private bool isDead;
    private State currentState;

    private float collisionRadius;
    private float playerCollisionRadius;
    private Color chasingColor;

    private FollowTargetComponent followTargetComponent;
    private Material material;
    private Coroutine attackCoroutine;
    #endregion

    #region Properties
    public bool IsDead
    {
        get
        {
            return this.isDead;
        }

        set
        {
            this.isDead = value;
        }
    }

    public State CurrentState
    {
        get
        {
            return this.currentState;
        }

        set
        {
            this.currentState = value;
        }
    }

    public float AttackDistanceThreshold
    {
        get
        {
            return this.attackDistanceThreshold;
        }

        set
        {
            this.attackDistanceThreshold = value;
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

        this.followTargetComponent = GetComponent<FollowTargetComponent>();
        this.material = GetComponent<Renderer>().material;

        if (this.playerTransform == null)
        {
            this.playerTransform = GameObject.Find("Player").transform;
        }

        if (this.damageableComponent == null)
        {
            this.damageableComponent = GetComponent<DamageableComponent>();
        }

        this.collisionRadius = GetComponent<CapsuleCollider>().radius;
        this.playerCollisionRadius = this.playerTransform.GetComponent<CapsuleCollider>().radius;
        this.attackDistanceThresholdSquared = Mathf.Pow(this.attackDistanceThreshold + this.collisionRadius + this.playerCollisionRadius, 2);
        this.followTargetComponent.PersonalSpace = this.collisionRadius + this.playerCollisionRadius;
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

        this.chasingColor = this.material.color;
        this.currentState = State.Chasing;
    }

    private void Update() 
    {
        if (Time.time > this.timeTillNextAttack)
        {
            float squareDistanceToTarget = (this.followTargetComponent.Target.transform.position - this.transform.position).sqrMagnitude;

            if (squareDistanceToTarget < this.attackDistanceThresholdSquared)
            {
                this.followTargetComponent.IsFollowTarget = false;
                this.timeTillNextAttack = Time.time + this.timeBetweenAttacks;

                this.attackCoroutine = StartCoroutine(Attack());
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

        if (this.damageableComponent != null)
        {
            this.damageableComponent.OnTakeDamage -= OnComponentTakeDamage;
            this.damageableComponent.OnDie -= OnComponentDie;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.DamageableComponent.TakeDamage(this.damage);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

    //    if (playerController != null)
    //    {
    //        playerController.DamageableComponent.TakeDamage(this.damage);
    //    }
    //}
    #endregion

    #region Public Functions
    #endregion

    #region Private Functions
    private IEnumerator Attack()
    {
        this.currentState = State.Attacking;
        this.followTargetComponent.IsFollowTarget = false;

        AudioManager.Instance.PlaySound("EnemyAttack", this.transform.position);

        Vector3 chasingPosition = transform.position;
        Vector3 dirToTarget = (this.playerTransform.position - this.transform.position).normalized;
        Vector3 attackPosition = this.playerTransform.position - dirToTarget * (this.collisionRadius);

        float percent = 0;

        this.material.color = this.attackColor;

        while (percent <= 1)
        {
            percent += Time.deltaTime * this.attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(chasingPosition, attackPosition, interpolation);

            yield return null;
        }

        this.material.color = this.chasingColor;
        this.currentState = State.Chasing;
        this.followTargetComponent.IsFollowTarget = true;
    } 

    private void PlayDeathEffect(Vector3 position, Vector3 direction)
    {
        float effectDuration = this.deathEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier;
        Destroy(Instantiate(this.deathEffect, position, Quaternion.FromToRotation(Vector3.forward, direction)) as GameObject, effectDuration);
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

        if (this.attackCoroutine != null)
        {
            StopCoroutine(this.attackCoroutine);
        }
    }

    private void OnGameRestart()
    {
    }

    private void OnGameQuit()
    {
        this.enabled = false;

        if (this.attackCoroutine != null)
        {
            StopCoroutine(this.attackCoroutine);
        }
    }

    private void OnComponentTakeDamage(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
    {
    }

    private void OnComponentDie(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
    {
        PlayDeathEffect(position, direction);

        if (OnDieStatic != null)
        {
            OnDieStatic(this.points);
        }

        this.isDead = true;

        Destroy(this.gameObject);
    }
    #endregion
}
