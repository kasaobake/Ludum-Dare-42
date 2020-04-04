using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class FollowTargetComponent : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private float velocity;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private PlayerController player;
    #endregion

    #region Private Variables
    private NavMeshAgent navMeshAgent;
    private bool isFollowTarget = true;
    private float refreshPathRate = 0.2f;
    private float personalSpace = 0f;
    #endregion

    #region Properties
    public Transform Target
    {
        get
        {
            return this.target;
        }

        set
        {
            this.target = value;
        }
    }

    public bool IsFollowTarget
    {
        get
        {
            return this.isFollowTarget;
        }

        set
        {
            this.isFollowTarget = value;
        }
    }

    public float PersonalSpace
    {
        get
        {
            return this.personalSpace;
        }

        set
        {
            this.personalSpace = value;
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

        if (this.target == null)
        {
            this.target = GameObject.Find("Player").transform;
        }

        if (this.enemyController == null)
        {
            this.enemyController = GetComponent<EnemyController>();
        }

        if (this.player == null)
        {
            this.player = this.target.GetComponent<PlayerController>();
        }

        if (this.navMeshAgent == null)
        {
            this.navMeshAgent = GetComponent<NavMeshAgent>();
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

        this.player.OnDie += OnTargetDie;

        this.navMeshAgent.speed = this.velocity;

        StartCoroutine(UpdatePath());
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

        if (this.player != null)
        {
            this.player.OnDie -= OnTargetDie;
        }
    }
    #endregion

    #region Private Functions
    private IEnumerator UpdatePath()
    {
        while (this.target != null)
        {
            if (this.isFollowTarget && this.enemyController.CurrentState == EnemyController.State.Chasing)
            {
                if (!this.enemyController.IsDead)
                {
                    Vector3 directionToTarget = (this.target.transform.position - transform.position).normalized;
                    Vector3 targetPosition = this.target.transform.position - directionToTarget * (this.personalSpace + (this.enemyController.AttackDistanceThreshold / 2));
                    this.navMeshAgent.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(this.refreshPathRate);
        }
    }
    #endregion

    #region Listeners
    private void OnGameStart()
    {
        this.isFollowTarget = true;
    }

    private void OnGamePause()
    {
        this.isFollowTarget = false;
    }

    private void OnGameResume()
    {
        this.isFollowTarget = true;
    }

    private void OnGameWon()
    {
    }

    private void OnGameLose()
    {
    }

    private void OnGameEnd()
    {
        this.isFollowTarget = false;
    }

    private void OnGameRestart()
    {
    }

    private void OnGameQuit()
    {
        this.isFollowTarget = false;
    }

    private void OnTargetDie()
    {
        this.isFollowTarget = false;
        this.enemyController.CurrentState = EnemyController.State.Idle;
    }
    #endregion
}
