using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kameosa.Managers;
using UnityEngine;

public enum WeaponFireMode
{
    Auto,
    Burst,
    Single
}

public class WeaponController : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private WeaponFireMode fireMode;
    [SerializeField]
    private float delayInMsBetweenShots = 400;
    [SerializeField]
    private float muzzleVelocity = 8f; 
    [SerializeField]
    private int burstCount = 8; 
    [Space(10)]

    [Header("Recoil")]
    [SerializeField]
    private Vector2 kickMinMax = new Vector2(.05f, .2f);
    [SerializeField]
    private Vector2 recoilAngleMinMax = new Vector2(3, 5);
    [SerializeField]
    private float recoilMoveSettleTime = .1f;
    [SerializeField]
    private float recoilRotationSettleTime = .1f;
    [Space(10)]

    [Header("Reload")]
    [SerializeField]
    private float reloadTime = 0.2f;
    [SerializeField]
    [Range(20f, 70f)]
    private float maxReloadAngle = 30f;
    [Space(10)]

    [Header("Audio")]
    [SerializeField]
    private AudioClip shootAudioClip;
    [SerializeField]
    private AudioClip reloadAudioClip;

    [Header("Prefabs")]
    [SerializeField]
    private Transform shellPrefab;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private Transform[] muzzleTransforms;
    [SerializeField]
    private Transform shellEjectionPosition;
    [SerializeField]
    private ProjectileController projectileController;
    [SerializeField]
    private MuzzleFlashController muzzleFlash;
    [SerializeField]
    private CrosshairController crosshair;
    #endregion

    #region Private Variables
    private Transform projectilesTransform;
    private float timeTillNextShot = 0f;
    private bool isTriggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    private float recoilAngle;
    private Vector3 recoilSmoothDampVelocity;
    private float recoilRotationSmoothDampVelocity;
    private bool isReloading = false;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.gameplay == null)
        {
            this.gameplay = GameObject.Find("Gameplay").GetComponent<GameplayController>();
        }

        if (this.projectilesTransform == null)
        {
            GameObject projectilesGameObject = UnityEngine.GameObject.Find("Projectiles");

            if (projectilesGameObject == null)
            {
                projectilesGameObject = new GameObject("Projectiles");
            }
            this.projectilesTransform = projectilesGameObject.GetComponent<Component>().transform;
        }

        if (this.muzzleFlash == null)
        {
            this.muzzleFlash = this.transform.Find("Muzzle/MuzzleFlash").GetComponent<MuzzleFlashController>();
        }

        if (this.crosshair == null)
        {
            this.crosshair = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
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

        this.enabled = false;
        this.shotsRemainingInBurst = this.burstCount;
    }

    private void Update()
    {
        Aim(this.crosshair.Position);
    }

    private void LateUpdate()
    {
        RecoilUpdate();
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
    public void Aim(Vector3 position)
    {
        if ((new Vector2(position.x, position.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
        {
            this.transform.LookAt(position);
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        this.isTriggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        this.isTriggerReleasedSinceLastShot = true;
        this.shotsRemainingInBurst = this.burstCount;
    }
    #endregion

    #region Private Functions
    private void Shoot() 
    {
        if (!this.isReloading && Time.time > this.timeTillNextShot)
        {
            if (this.fireMode == WeaponFireMode.Burst)
            {
                if (this.shotsRemainingInBurst == 0)
                {
                    // I choose to let user reload when burst runs out.
                    Reload();
                    return;
                }

                this.shotsRemainingInBurst--;
            }
            else if (this.fireMode == WeaponFireMode.Single)
            {
                if (!this.isTriggerReleasedSinceLastShot)
                {
                    Reload();
                    return;
                }
            }

            foreach(Transform muzzleTransform in this.muzzleTransforms)
            {
                this.timeTillNextShot = Time.time + (this.delayInMsBetweenShots / 1000);

                ProjectileController newProjectile = Instantiate(this.projectileController, muzzleTransform.position, muzzleTransform.rotation) as ProjectileController;
                newProjectile.Velocity = this.muzzleVelocity;
                newProjectile.transform.SetParent(this.projectilesTransform);
            }

            EjectShell();
            this.muzzleFlash.Activate();
            Recoil();

            AudioManager.Instance.PlaySound(this.shootAudioClip, this.transform.position);
        }
    }

    private void EjectShell()
    {
        Instantiate(this.shellPrefab, this.shellEjectionPosition.position, this.shellEjectionPosition.rotation);
    }

    private void Recoil()
    {
        this.transform.localPosition -= Vector3.forward * Random.Range(this.kickMinMax.x, this.kickMinMax.y);
        this.recoilAngle += Random.Range(this.recoilAngleMinMax.x, this.recoilAngleMinMax.y);
        this.recoilAngle = Mathf.Clamp(this.recoilAngle, 0, 30);
    }

    private void RecoilUpdate()
    {
        this.transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref this.recoilSmoothDampVelocity, this.recoilMoveSettleTime);
        this.recoilAngle = Mathf.SmoothDamp(this.recoilAngle, 0, ref this.recoilRotationSmoothDampVelocity, this.recoilRotationSettleTime);
        this.transform.localEulerAngles = this.transform.localEulerAngles + Vector3.left * this.recoilAngle;
    }

    private void Reload()
    {
        StartCoroutine(AnimateReload());
        AudioManager.Instance.PlaySound(this.reloadAudioClip, this.transform.position);
    }

    private IEnumerator AnimateReload()
    {
        this.isReloading = true;

        yield return new WaitForSeconds(0.2f);

        float reloadSpeed = 1f / this.reloadTime;
        float percent = 0f;
        Vector3 initialRotation = this.transform.localEulerAngles;

        while (percent < 1f)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, this.maxReloadAngle, interpolation);
            this.transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;

            yield return null;
        }

        this.isReloading = false;
        OnTriggerRelease();
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
