using System;
using System.Collections;
using System.Collections.Generic;
using Kameosa.Collections.IList;
using UnityEngine;

public class SpawnController : MonoBehaviour 
{
    [System.Serializable]
    public class Wave
    {
        [System.Serializable]
        public struct EnemySpawnFrequency
        {
            public GameObject enemyPrefab;
            [Range(1, 10)]
            public int oneInEveryBasicEnemyCount;
        }

        #region Inspector Variables
        [SerializeField]
        private bool isInfinite;
        [SerializeField]
        private int enemyCount;
        [SerializeField]
        private float timeBetweenSpawns;
        [SerializeField]
        private float timeBetweenWaves;
        [Space(10)]

        [Header("Enemy")]
        [SerializeField]
        private GameObject basicEnemyPrefab;
        [SerializeField]
        private EnemySpawnFrequency[] enemySpawnFrequencies;
        [Space(10)]
        #endregion

        #region Private Variables
        private List<int> enemyPrefabFrequencyIndices;
        #endregion

        #region Properties
        public int EnemyCount
        {
            get
            {
                return this.enemyCount;
            }

            set
            {
                this.enemyCount = value;
            }
        }

        public float TimeBetweenSpawns
        {
            get
            {
                return this.timeBetweenSpawns;
            }

            set
            {
                this.timeBetweenSpawns = value;
            }
        }

        public float TimeBetweenWaves
        {
            get
            {
                return this.timeBetweenWaves;
            }

            set
            {
                this.timeBetweenWaves = value;
            }
        }

        public bool IsInfinite
        {
            get
            {
                return this.isInfinite;
            }

            set
            {
                this.isInfinite = value;
            }
        }
        #endregion

        #region Public Functions
        public GameObject GetRandomEnemyPrefab()
        {
            if (this.enemyPrefabFrequencyIndices == null)
            {
                this.enemyPrefabFrequencyIndices = new List<int>();

                for (int i = 0; i < this.enemySpawnFrequencies.Length; i++)
                {
                    this.enemyPrefabFrequencyIndices.Add(i);

                    for (int j = 0; j < this.enemySpawnFrequencies[i].oneInEveryBasicEnemyCount; j++)
                    {
                        this.enemyPrefabFrequencyIndices.Add(-1);
                    }
                }
            }

            int enemyPrefabIndex = this.enemyPrefabFrequencyIndices.GetRandom();

            return enemyPrefabIndex == -1 ? this.basicEnemyPrefab : this.enemySpawnFrequencies[enemyPrefabIndex].enemyPrefab;
        }
        #endregion
    }

    #region Inspector Variables
    [SerializeField]
    private Wave[] waves;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Transform playerTransform;
    #endregion

    #region Private Variables
    private int currentWaveIndex;
    private int enemiesRemainingToSpawnCount;
    private int enemiesRemainingAliveCount;
    private float timeTillNextSpawn;
    private float timeTillNextWave;
    private float spawnDelay = 1f;
    private float tileFlashSpeed = 4f;

    private Color spawningColor = Color.red;

    private RoomController room;

    private float timeBetweenCampingChecks = 2;
    private float campThresholdDistance = 1.5f;
    private float nextCampCheckTime;
    private Vector3 campPositionOld;
    private bool isCamping;
    #endregion

    #region Properties
    public Wave CurrentWave 
    {
        get
        {
            if (this.currentWaveIndex >= this.waves.Length)
            {
                return null;
            }

            return this.waves[this.currentWaveIndex];
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

        if (this.room == null)
        {
            this.room = UnityEngine.GameObject.Find("Room").GetComponent<RoomController>();
        }

        if (this.playerTransform == null)
        {
            this.playerTransform = GameObject.Find("Player").transform;
        }

        if (this.player == null)
        {
            this.player = this.playerTransform.GetComponent<PlayerController>();
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

        EnemyController.OnDieStatic += OnEnemyDie;

        this.enemiesRemainingToSpawnCount = CurrentWave.EnemyCount;
        this.enemiesRemainingAliveCount = CurrentWave.EnemyCount;

        this.nextCampCheckTime = this.timeBetweenCampingChecks + Time.time;
        this.campPositionOld = this.playerTransform.position;
    }

    private void Update() 
    {
        if (CurrentWave == null)
        {
            return;
        }

        if (Time.time > this.nextCampCheckTime)
        {
            this.nextCampCheckTime = Time.time + this.timeBetweenCampingChecks;

            this.isCamping = (Vector3.Distance(this.playerTransform.position, this.campPositionOld) < this.campThresholdDistance);
            this.campPositionOld = this.playerTransform.position;
        }

        if ((this.enemiesRemainingToSpawnCount > 0 || CurrentWave.IsInfinite) && Time.time > this.timeTillNextSpawn && Time.time > this.timeTillNextWave)
        {
            this.enemiesRemainingToSpawnCount--;
            this.timeTillNextSpawn = Time.time + CurrentWave.TimeBetweenSpawns;

            StartCoroutine(Spawn());
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

        EnemyController.OnDieStatic -= OnEnemyDie;
    }
    #endregion

    #region Private Functions
    private IEnumerator Spawn()
    {
        TileController spawnTile = this.room.GetRandomUnoccupiedTile();

        if (this.isCamping)
        {
            spawnTile = this.room.GetTileFromPosition(this.playerTransform.position);
        }

        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        float spawnTimer = 0;

        while (spawnTimer < this.spawnDelay)
        {
            tileMat.color = Color.Lerp(this.room.TileColor, this.spawningColor, Mathf.PingPong(spawnTimer * this.tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Instantiate(CurrentWave.GetRandomEnemyPrefab(), spawnTile.transform.position + Vector3.up, Quaternion.identity).GetComponent<EnemyController>();
    }

    private void NextWave()
    {
        this.timeTillNextWave = Time.time + this.waves[this.currentWaveIndex].TimeBetweenWaves;
        this.currentWaveIndex++;

        if (this.currentWaveIndex < this.waves.Length)
        {
            this.enemiesRemainingToSpawnCount = this.waves[this.currentWaveIndex].EnemyCount;
            this.enemiesRemainingAliveCount = this.waves[this.currentWaveIndex].EnemyCount;
        }
    }
    #endregion

    #region Listeners
    private void OnEnemyDie(int points)
    {
        this.gameplay.ScoreComponent.AddScore(points);
        this.enemiesRemainingAliveCount--;

        if (!CurrentWave.IsInfinite && this.enemiesRemainingAliveCount == 0)
        {
            NextWave();
        }
    }

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
