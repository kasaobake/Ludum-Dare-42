using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveJoystickComponent : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private float maxVelocity = 5f;
    [SerializeField]
    private bool isAlwaysFacingCursor = false;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private CrosshairController crosshair;
    #endregion

    #region Private Variables
    private Vector3 velocity;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.gameplay == null)
        {
            GameObject gameplayGameObject = GameObject.Find("Gameplay");

            if (gameplayGameObject != null)
            {
                this.gameplay = gameplayGameObject.GetComponent<GameplayController>();
            }
        }

        if (this.rigidbody == null)
        {
            this.rigidbody = GetComponent<Rigidbody>();
        }

        if (this.crosshair == null)
        {
            GameObject crosshairGameObject = GameObject.Find("Crosshair");

            if (crosshairGameObject != null)
            {
                this.crosshair = crosshairGameObject.GetComponent<CrosshairController>();
            }
        }
    }

    private void Start() 
    {
        if (this.gameplay != null)
        {
            this.gameplay.OnGameStart += OnGameStart;
            this.gameplay.OnGamePause += OnGamePause;
            this.gameplay.OnGameResume += OnGameResume;
            this.gameplay.OnGameWon += OnGameWon;
            this.gameplay.OnGameLose += OnGameLose;
            this.gameplay.OnGameEnd += OnGameEnd;
            this.gameplay.OnGameRestart += OnGameRestart;
            this.gameplay.OnGameQuit += OnGameQuit;
        }

        this.enabled = false;
    }

    private void Update() 
    {
        Vector3 input = new Vector3(Input.GetAxisRaw(Kameosa.Constants.Input.HORIZONTAL), 0f, Input.GetAxisRaw(Kameosa.Constants.Input.VERTICAL));
        this.velocity = input.normalized * this.maxVelocity;

        if (this.isAlwaysFacingCursor)
        {
            Vector3 heightCorrectedPoint = new Vector3(this.crosshair.Position.x, this.transform.position.y, this.crosshair.Position.z);
            this.transform.LookAt(heightCorrectedPoint);
        }
    }

    private void FixedUpdate()
    {
        Move();
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

    #region Private Functions
    private void Move()
    {
        this.rigidbody.MovePosition(this.rigidbody.position + (this.velocity * Time.fixedDeltaTime));
        //this.rigidbody.AddForce(this.velocity);
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
