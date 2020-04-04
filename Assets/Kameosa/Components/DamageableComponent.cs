using System;
using System.Collections;
using System.Collections.Generic;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Components
    {
        public class DamageableComponent : MonoBehaviour
        {
            [System.Serializable]
            public class ZoneColor
            {
                #region Inspector Variables
                [SerializeField]
                [Range(0, 1)]
                private float threshold;
                [SerializeField]
                private Color color = Color.green;
                #endregion

                #region Properties
                public float Threshold
                {
                    get
                    {
                        return this.threshold;
                    }

                    set
                    {
                        this.threshold = value;
                    }
                }

                public Color Color
                {
                    get
                    {
                        return this.color;
                    }

                    set
                    {
                        this.color = value;
                    }
                }
                #endregion

                #region Public Functions
                public ZoneColor(float threshold, Color color)
                {
                    this.threshold = threshold;
                    this.color = color;
                }
                #endregion
            }

            #region Inspector Variables
            [SerializeField]
            private int maxHealth;
            [SerializeField]
            private bool isGodMode;
            [Space(10)]

            [Header("Zone Color")]
            [SerializeField]
            private ZoneColor goodZone = new ZoneColor(0.0f, Color.green);
            [SerializeField]
            private ZoneColor fairZone = new ZoneColor(0.7f, Color.yellow);
            [SerializeField]
            private ZoneColor seriousZone = new ZoneColor(0.4f, new Color(1f, 0.64f, 0f));
            [SerializeField]
            private ZoneColor criticalZone = new ZoneColor(0.2f, Color.red);
            [Space(10)]

            [Header("Audio")]
            [SerializeField]
            private string takeDamageAudioName;
            [SerializeField]
            private AudioClip takeDamageAudioClip;
            [SerializeField]
            private string dieAudioName;
            [SerializeField]
            private AudioClip dieAudioClip;
            [Space(10)]

            [Header("Reference")]
            [SerializeField]
            private GameplayController gameplay;
            [SerializeField]
            private Image healthBar;
            #endregion

            #region Private Variables
            [SerializeField]
            private int health;
            #endregion

            #region Actions
            public event Action<int, UnityEngine.Vector3, UnityEngine.Vector3> OnTakeDamage;
            public event Action<int, UnityEngine.Vector3, UnityEngine.Vector3> OnDie;
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {
                if (this.gameplay == null)
                {
                    this.gameplay = GameObject.Find("Gameplay").GetComponent<GameplayController>();
                }

                //if (this.healthBar == null)
                //{
                //    this.healthBar = this.transform.Find("Canvas/HealthBarBackground/HealthBarFill").GetComponent<Image>();
                //}
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

                this.health = this.maxHealth;
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
            }
            #endregion

            #region Public Functions
            public void TakeDamage(int damage)
            {
                TakeDamage(damage, UnityEngine.Vector3.zero, UnityEngine.Vector3.zero);
            }

            public void TakeDamage(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
            {
                if (this.isGodMode || !this.enabled)
                {
                    return;
                }

                this.health -= damage;

                UpdateHealthBar();

                if (this.health <= 0)
                {
                    Die(damage, position, direction);
                }
                else
                {
                    if (this.takeDamageAudioName != "")
                    {
                        AudioManager.Instance.PlaySound(this.takeDamageAudioName, this.transform.position);
                    }
                    else if (this.takeDamageAudioClip != null)
                    {
                        AudioManager.Instance.PlaySound(this.takeDamageAudioClip, this.transform.position);
                    }

                    if (OnTakeDamage != null)
                    {
                        OnTakeDamage(damage, position, direction);
                    }
                }
            }
            #endregion

            #region Private Functions
            public void Die(int damage, UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
            {
                if (this.dieAudioName != "")
                {
                    AudioManager.Instance.PlaySound(this.dieAudioName, this.transform.position);
                }
                else if (this.dieAudioClip != null)
                {
                    AudioManager.Instance.PlaySound(this.dieAudioClip, this.transform.position);
                }

                if (OnDie != null)
                {
                    OnDie(damage, position, direction);
                }
            }

            private void UpdateHealthBar()
            {
                if (this.healthBar != null)
                {
                    float healthPercent = (float) this.health / this.maxHealth;
                    this.healthBar.fillAmount = healthPercent;

                    if (healthPercent <= this.criticalZone.Threshold)
                    {
                        this.healthBar.color = this.criticalZone.Color;
                    }
                    else if (healthPercent <= this.seriousZone.Threshold)
                    {
                        this.healthBar.color = this.seriousZone.Color;
                    }
                    else if (healthPercent <= this.fairZone.Threshold)
                    {
                        this.healthBar.color = this.fairZone.Color;
                    }
                    else
                    {
                        this.healthBar.color = this.goodZone.Color;
                    }
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
            #endregion
        }
    }
}
