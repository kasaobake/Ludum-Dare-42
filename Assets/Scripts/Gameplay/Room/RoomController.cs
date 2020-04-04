using System;
using System.Collections;
using System.Collections.Generic;
using Kameosa.Cartesian;
using UnityEngine;
using UnityEngine.AI;

public class OuterWalls
{
    #region Private Variables
    private Transform left;
    private Transform right;
    private Transform forward;
    private Transform back;
    #endregion

    #region Properties
    public Transform Left
    {
        get 
        {
            return this.left;
        }

        set 
        {
            this.left = value; 
        }
    }

    public Transform Right
    {
        get
        {
            return this.right;
        }

        set
        {
            this.right = value;
        }
    }

    public Transform Forward
    {
        get
        {
            return this.forward;
        }

        set
        {
            this.forward = value;
        }
    }

    public Transform Back
    {
        get
        {
            return this.back;
        }

        set
        {
            this.back = value;
        }
    }
    #endregion
}

[RequireComponent(typeof(RoomGeneratorComponent))]
[RequireComponent(typeof(NavMeshSurface))]
public class RoomController : MonoBehaviour
{
    private const int SHRINK = -1;
    private const int EXPAND = 1;
   
    #region Inspector Variables
    [SerializeField]
    private float shrinkDurationInSeconds = 1f;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    #endregion

    #region Private Variables
    private RoomGeneratorComponent roomGeneratorComponent;
    private TileController[,] tiles;
    private OuterWalls outerWalls;
    private Transform floorBaseTransform;
    private float tileSize;
    private Color tileColor;
    private Coordinate playerSpawnCoordinate;

    private int currentShrunkIndex = 0;
    private int targetShrunkIndex = 0;
    private int maxShrunkIndex = 0;
    private bool isAnimationSize = false;
    private NavMeshSurface navMeshSurface;
    #endregion

    #region Properties
    public TileController[,] Tiles
    {
        get
        {
            return this.tiles;
        }

        set
        {
            this.tiles = value;
        }
    }

    public OuterWalls OuterWalls
    {
        get
        {
            return this.outerWalls;
        }

        set
        {
            this.outerWalls = value;
        }
    }

    public Transform FloorBaseTransform
    {
        get
        {
            return this.floorBaseTransform;
        }

        set
        {
            this.floorBaseTransform = value;
        }
    }

    public float TileSize
    {
        get
        {
            return this.tileSize;
        }

        set
        {
            this.tileSize = value;
        }
    }

    public int MaxShrunkIndex
    {
        get
        {
            return this.maxShrunkIndex;
        }

        set
        {
            this.maxShrunkIndex = value;
        }
    }

    public Color TileColor
    {
        get
        {
            return this.tileColor;
        }

        set
        {
            this.tileColor = value;
        }
    }

    public Coordinate PlayerSpawnCoordinate
    {
        get
        {
            return this.playerSpawnCoordinate;
        }

        set
        {
            this.playerSpawnCoordinate = value;
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

        this.roomGeneratorComponent = GetComponent<RoomGeneratorComponent>();
        this.navMeshSurface = GetComponent<NavMeshSurface>();
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

        this.roomGeneratorComponent.Generate();
        UpdateNavMesh();
        this.enabled = false;
    }

	private void Update()
	{
        HandleShrink();
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
	public TileController GetRandomTile()
    {
        int x = UnityEngine.Random.Range(this.currentShrunkIndex, this.tiles.GetLength(0) - this.currentShrunkIndex - 1);
        int y = UnityEngine.Random.Range(this.currentShrunkIndex, this.tiles.GetLength(1) - this.currentShrunkIndex - 1);

        return this.tiles[x, y];
    }

    public TileController GetRandomUnoccupiedTile()
    {
        int tryCount = 10;
        TileController result;

        do
        {
            tryCount--;
            result = GetRandomTile();
        } while (result.IsOccupied && tryCount > 0);

        return result.IsOccupied ?  this.tiles[this.playerSpawnCoordinate.X, this.playerSpawnCoordinate.Y] : result;
    }

    public void Shrink()
    {
        this.targetShrunkIndex = Mathf.Min(this.targetShrunkIndex + 1, this.maxShrunkIndex);
    }

    public void Expand()
    {
        this.targetShrunkIndex = Mathf.Max(this.targetShrunkIndex - 1, 0) ;
    }

    public TileController GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / this.tileSize + (this.tiles.GetLength(1) - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / this.tileSize + (this.tiles.GetLength(0) - 1) / 2f);
        x = Mathf.Clamp(x, 0, this.tiles.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, this.tiles.GetLength(1) - 1);

        return this.tiles[x, y];
    }
    #endregion

    #region Private Functions
    private void HandleShrink()
    {
        if (!this.isAnimationSize)
        {
            if (this.currentShrunkIndex < this.targetShrunkIndex)
            {
                this.currentShrunkIndex++;
                StartCoroutine(AnimateSize(SHRINK));
            }
            else if (this.currentShrunkIndex > this.targetShrunkIndex)
            {
                this.currentShrunkIndex--;
                StartCoroutine(AnimateSize(EXPAND));
            }
        }
    }

    private void UpdateNavMesh()
    {
        this.navMeshSurface.BuildNavMesh();
    }

    private IEnumerator AnimateSize(int direction)
    {
        this.isAnimationSize = true;
        float percent = 0f;

        Vector3 leftRightOuterWallOriginalSize = Vector3.zero;
        Vector3 forwardBackOuterWallOriginalSize = Vector3.zero;

        Vector3 leftOuterWallOriginalPosition = Vector3.zero;
        Vector3 rightOuterWallOriginalPosition = Vector3.zero;
        Vector3 forwardOuterWallOriginalPosition = Vector3.zero;
        Vector3 backOuterWallOriginalPosition = Vector3.zero;
 
        if (this.outerWalls != null)
        {
            leftRightOuterWallOriginalSize = this.outerWalls.Left.localScale;
            forwardBackOuterWallOriginalSize = this.outerWalls.Forward.localScale;

            leftOuterWallOriginalPosition = this.outerWalls.Left.position;
            rightOuterWallOriginalPosition = this.outerWalls.Right.position;
            forwardOuterWallOriginalPosition = this.outerWalls.Forward.position;
            backOuterWallOriginalPosition = this.outerWalls.Back.position;
        }

        Vector3 floorBaseOriginalSize = this.floorBaseTransform.localScale;
       
        if (direction != SHRINK)
        {
            bool isActive = true;
            int index = this.currentShrunkIndex;

            for (int i = index; i < this.tiles.GetLength(0) - index; i++)
            {
                this.tiles[i, index].gameObject.SetActive(isActive);
                this.tiles[i, this.tiles.GetLength(1) - index - 1].gameObject.SetActive(isActive);
            }

            for (int i = index; i < this.tiles.GetLength(1) - index; i++)
            {
                this.tiles[index, i].gameObject.SetActive(isActive);
                this.tiles[this.tiles.GetLength(0) - index - 1, i].gameObject.SetActive(isActive);
            }
        }

        while (percent < 1f)
        {
            percent += (Time.deltaTime / this.shrinkDurationInSeconds);

            if (this.outerWalls != null)
            {
                this.outerWalls.Left.localScale = leftRightOuterWallOriginalSize + (Vector3.forward * 2 * percent * direction);
                this.outerWalls.Right.localScale = leftRightOuterWallOriginalSize + (Vector3.forward * 2 * percent * direction);
                this.outerWalls.Forward.localScale = forwardBackOuterWallOriginalSize + (Vector3.right * 2 * percent * direction);
                this.outerWalls.Back.localScale = forwardBackOuterWallOriginalSize + (Vector3.right * 2 * percent * direction);

                this.outerWalls.Left.position = leftOuterWallOriginalPosition + (Vector3.left * percent * direction);
                this.outerWalls.Right.position = rightOuterWallOriginalPosition + (Vector3.right * percent * direction);
                this.outerWalls.Forward.position = forwardOuterWallOriginalPosition + (Vector3.forward * percent * direction);
                this.outerWalls.Back.position = backOuterWallOriginalPosition + (Vector3.back * percent * direction);
            }

            this.floorBaseTransform.localScale = floorBaseOriginalSize + ((Vector3.up + Vector3.right) * percent * 2 * direction);

            yield return null;
        }

        if (direction == SHRINK)
        {
            bool isActive = false;
            int index = this.currentShrunkIndex + direction;

            for (int i = index; i < this.tiles.GetLength(0) - index; i++)
            {
                this.tiles[i, index].gameObject.SetActive(isActive);
                this.tiles[i, this.tiles.GetLength(1) - index - 1].gameObject.SetActive(isActive);
            }

            for (int i = index; i < this.tiles.GetLength(1) - index; i++)
            {
                this.tiles[index, i].gameObject.SetActive(isActive);
                this.tiles[this.tiles.GetLength(0) - index - 1, i].gameObject.SetActive(isActive);
            }
        }

        UpdateNavMesh();
        this.isAnimationSize = false;
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