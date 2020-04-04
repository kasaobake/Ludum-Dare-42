using System;
using System.Collections;
using System.Collections.Generic;
using Kameosa.Cartesian;
using UnityEngine;

[RequireComponent(typeof(RoomController))]
public class RoomGeneratorComponent : MonoBehaviour 
{
    [System.Serializable]
    public class RoomParameters
    { 
        #region Inspector Variables
        [SerializeField]
        private Coordinate roomSize = new Coordinate(10, 10);
        [SerializeField]
        private Color tileColor = Color.white;
        [SerializeField]
        private int seed;
        [Space(10)]

        [Header("Obstacle")]
        [SerializeField]
        [Range(0, 1)]
        private float obstaclePercentage = 0.2f;
        [SerializeField]
        [Range(0, 10)]
        private float minObstacleHeight = 1;
        [SerializeField]
        [Range(0, 10)]
        private float maxObstacleHeight = 10;
        [SerializeField]
        private Color foregroundColour;
        [SerializeField]
        private Color backgroundColour;
        #endregion

        #region Properties
        public Coordinate RoomSize
        {
            get
            {
                return this.roomSize;
            }

            set
            {
                this.roomSize = value;
            }
        }

        public float ObstaclePercentage
        {
            get
            {
                return this.obstaclePercentage;
            }

            set
            {
                this.obstaclePercentage = value;
            }
        }

        public float MinObstacleHeight
        {
            get
            {
                return this.minObstacleHeight;
            }

            set
            {
                this.minObstacleHeight = value;
            }
        }

        public float MaxObstacleHeight
        {
            get
            {
                return this.maxObstacleHeight;
            }

            set
            {
                this.maxObstacleHeight = value;
            }
        }

        public int Seed
        {
            get
            {
                return this.seed;
            }

            set
            {
                this.seed = value;
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

        public Color ForegroundColour
        {
            get
            {
                return this.foregroundColour;
            }

            set
            {
                this.foregroundColour = value;
            }
        }

        public Color BackgroundColour
        {
            get
            {
                return this.backgroundColour;
            }

            set
            {
                this.backgroundColour = value;
            }
        }
        #endregion
    }

    #region Inspector Variables
    [Header("Generation Parameters")]
    [SerializeField]
    private float tileSize = 1f;
    [SerializeField]
    private float outerWallHeight = 1f;
    [SerializeField]
    private bool isShowOuterWall = true;
    [SerializeField]
    private bool isGenerateOuterWall = true;
    [SerializeField]
    [Range(0, 1)]
    private float borderPercentage;
    [SerializeField]
    private RoomParameters roomParameters;
    [Space(10)]

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floorTilePrefab;
    [SerializeField]
    private GameObject obstacleTilePrefab;
    [SerializeField]
    private GameObject outerWallPrefab;
    [SerializeField]
    private GameObject floorBasePrefab;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private RoomController room;
    [Space(10)]

    //[SerializeField]
    //private Coordinate maxRoomSize = new Coordinate(20, 20);
    //[SerializeField]
    //private Transform navmeshPlane;
    #endregion

    #region Private Variables
    private System.Random random;
    private string containerName = "Container";
    private Transform container;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.room == null)
        {
            this.room = GetComponent<RoomController>();
        }
    }
    #endregion

    #region Public Functions
    public void Generate()
    {
        Generate(this.roomParameters);
    }

    public void Generate(RoomParameters roomParameters)
    {
        this.roomParameters = roomParameters;
        this.random = new System.Random(this.roomParameters.Seed);
        this.room.PlayerSpawnCoordinate = new Coordinate((int)(this.roomParameters.RoomSize.X / 2f), (int)(this.roomParameters.RoomSize.Y / 2f));
        this.room.MaxShrunkIndex = Mathf.Min(this.roomParameters.RoomSize.X, this.roomParameters.RoomSize.Y) / 2;
        this.room.TileSize = this.tileSize;
        this.room.TileColor = this.roomParameters.TileColor;

        Reset();
        GenerateFloorTiles();
        GenerateObstacles();

        if (this.isGenerateOuterWall)
        {
            GenerateOuterWall();
        }

        GenerateFloorBase();
    }
    #endregion

    #region Private Functions
    private void Reset()
    {
        if (this.transform.Find(this.containerName))
        {
            DestroyImmediate(this.transform.Find(this.containerName).gameObject);
        }

        this.container = new GameObject(this.containerName).transform;
        this.container.SetParent(this.transform);
    }

    private void GenerateFloorTiles()
    {
        this.room.Tiles = new TileController[this.roomParameters.RoomSize.X, this.roomParameters.RoomSize.Y];

        Transform tilesContainer = new GameObject("Tiles").transform;
        tilesContainer.SetParent(this.container);

        for (int x = 0; x < this.roomParameters.RoomSize.X; x++)
        {
            for (int y = 0; y < this.roomParameters.RoomSize.Y; y++)
            {
                Vector3 tilePosition = CoordinateToGlobalPosition(x, y);
                Transform newTile = Instantiate(this.floorTilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)).transform;
                newTile.SetParent(tilesContainer);
                //newTile.SetParent(this.container);
                newTile.GetComponent<Renderer>().sharedMaterial.color = this.roomParameters.TileColor;
                newTile.localScale = Vector3.one * (1f - this.borderPercentage) * this.tileSize;
                this.room.Tiles[x, y] = newTile.gameObject.AddComponent<TileController>();
                this.room.Tiles[x, y].Coordinate = new Coordinate(x, y);
            }
        }
    }

    private void GenerateObstacles()
    {
        int obstacleCount = (int)(this.roomParameters.ObstaclePercentage * (this.roomParameters.RoomSize.X * this.roomParameters.RoomSize.Y));
        int currentObstacleCount = 0;
        bool[,] obstacleMap = new bool[this.roomParameters.RoomSize.X, this.roomParameters.RoomSize.Y];

        Transform obstaclesContainer = new GameObject("Obstacles").transform;
        obstaclesContainer.SetParent(this.container);

        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = this.room.GetRandomTile().Coordinate;

            if (obstacleMap[randomCoordinate.X, randomCoordinate.Y])
            {
                continue;
            }

            obstacleMap[randomCoordinate.X, randomCoordinate.Y] = true;
            this.room.Tiles[randomCoordinate.X, randomCoordinate.Y].IsOccupied = true;
            currentObstacleCount++;

            if (randomCoordinate != this.room.PlayerSpawnCoordinate && IsMapFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(this.roomParameters.MinObstacleHeight, this.roomParameters.MaxObstacleHeight, (float) this.random.NextDouble());
                Vector3 obstaclePosition = CoordinateToGlobalPosition(randomCoordinate.X, randomCoordinate.Y);
                Transform newObstacle = Instantiate(this.obstacleTilePrefab, obstaclePosition + (Vector3.up * obstacleHeight / 2f), Quaternion.identity).transform;
                newObstacle.localScale = new Vector3((1f - this.borderPercentage) * this.tileSize, obstacleHeight, (1 - this.borderPercentage) * this.tileSize);
                //newObstacle.SetParent(this.container);
                newObstacle.SetParent(obstaclesContainer);

                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colourPercent = randomCoordinate.Y / (float)this.roomParameters.RoomSize.Y;
                obstacleMaterial.color = Color.Lerp(this.roomParameters.ForegroundColour, this.roomParameters.BackgroundColour, colourPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                obstacleMap[randomCoordinate.X, randomCoordinate.Y] = false;
                currentObstacleCount--;
            }
        }
    }

    private void GenerateOuterWall()
    {
        float padding = this.tileSize * 2f;

        Transform outerWallsContainer = new GameObject("OuterWalls").transform;
        outerWallsContainer.SetParent(this.container);

        Vector3 leftOuterWallPosition = Vector3.left * (this.roomParameters.RoomSize.X / 2f) * this.tileSize;
        leftOuterWallPosition.x -= this.tileSize / 2f;
        leftOuterWallPosition.y = 0.5f;
        Transform leftOuterWallTransform = Instantiate(this.outerWallPrefab, leftOuterWallPosition, Quaternion.identity).transform;
        //leftOuterWallTransform.SetParent(this.container);
        leftOuterWallTransform.SetParent(outerWallsContainer);
        leftOuterWallTransform.GetComponent<MeshRenderer>().enabled = this.isShowOuterWall;
        leftOuterWallTransform.localScale = new Vector3(1f, this.outerWallHeight, this.roomParameters.RoomSize.Y + padding) * this.tileSize;

        Vector3 rightOuterWallPosition = Vector3.right * (this.roomParameters.RoomSize.X / 2f) * this.tileSize;
        rightOuterWallPosition.x += this.tileSize / 2f;
        rightOuterWallPosition.y = 0.5f;
        Transform rightOuterWallTransform = Instantiate(this.outerWallPrefab, rightOuterWallPosition, Quaternion.identity).transform;
        //rightOuterWallTransform.SetParent(this.container);
        rightOuterWallTransform.SetParent(outerWallsContainer);
        rightOuterWallTransform.GetComponent<MeshRenderer>().enabled = this.isShowOuterWall;
        rightOuterWallTransform.localScale = new Vector3(1f, this.outerWallHeight, this.roomParameters.RoomSize.Y+ padding) * this.tileSize;

        Vector3 forwardOuterWallPosition = Vector3.forward * (this.roomParameters.RoomSize.Y / 2f) * this.tileSize;
        forwardOuterWallPosition.z += this.tileSize / 2f;
        forwardOuterWallPosition.y = 0.5f;
        Transform forwardOuterWallTransform = Instantiate(this.outerWallPrefab, forwardOuterWallPosition, Quaternion.identity).transform;
        //forwardOuterWallTransform.SetParent(this.container);
        forwardOuterWallTransform.SetParent(outerWallsContainer);
        forwardOuterWallTransform.GetComponent<MeshRenderer>().enabled = this.isShowOuterWall;
        forwardOuterWallTransform.localScale = new Vector3(this.roomParameters.RoomSize.X, this.outerWallHeight, 1f) * this.tileSize;

        Vector3 backOuterWallPosition = Vector3.back * (this.roomParameters.RoomSize.Y / 2f) * this.tileSize;
        backOuterWallPosition.z -= this.tileSize / 2f;
        backOuterWallPosition.y = 0.5f;
        Transform backOuterWallTransform = Instantiate(this.outerWallPrefab, backOuterWallPosition, Quaternion.identity).transform;
        //backOuterWallTransform.SetParent(this.container);
        backOuterWallTransform.SetParent(outerWallsContainer);
        backOuterWallTransform.GetComponent<MeshRenderer>().enabled = this.isShowOuterWall;
        backOuterWallTransform.localScale = new Vector3(this.roomParameters.RoomSize.X, this.outerWallHeight, 1f) * this.tileSize;

        OuterWalls outerWalls = new OuterWalls
        {
            Left = leftOuterWallTransform,
            Right = rightOuterWallTransform,
            Forward = forwardOuterWallTransform,
            Back = backOuterWallTransform
        };

        this.room.OuterWalls = outerWalls;
    }

    private void GenerateFloorBase()
    {
        Vector3 floorBasePosition = new Vector3(0f, -0.1f, 0f);
        Vector3 floorBaseSize = new Vector3(this.roomParameters.RoomSize.X * this.tileSize, this.roomParameters.RoomSize.Y * this.tileSize, 1f);
        Transform floorBaseTransform = Instantiate(this.floorBasePrefab, floorBasePosition, Quaternion.Euler(Vector3.right * 90)).transform;
        floorBaseTransform.localScale = floorBaseSize;
        //floorBaseTransform.GetComponent<BoxCollider>().size = Vector3.one;
        floorBaseTransform.SetParent(this.container);
        this.room.FloorBaseTransform = floorBaseTransform;
    }

    private Vector3 CoordinateToGlobalPosition(int x, int y)
    {
        return new Vector3((-this.roomParameters.RoomSize.X / 2f) + 0.5f + x, 0f, (-this.roomParameters.RoomSize.Y / 2f) + 0.5f + y) * this.tileSize;
    }

    private bool IsMapFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] isVisited = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        queue.Enqueue(this.room.PlayerSpawnCoordinate);
        isVisited[this.room.PlayerSpawnCoordinate.X, this.room.PlayerSpawnCoordinate.Y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coordinate tile = queue.Dequeue();

            foreach (Coordinate neighbour in tile.Neighbours)
            {
                if (neighbour.X >= 0 && neighbour.X < obstacleMap.GetLength(0) && neighbour.Y >= 0 && neighbour.Y < obstacleMap.GetLength(1))
                {
                    if (!isVisited[neighbour.X, neighbour.Y] && !obstacleMap[neighbour.X, neighbour.Y])
                    {
                        isVisited[neighbour.X, neighbour.Y] = true;
                        queue.Enqueue(new Coordinate(neighbour.X, neighbour.Y));
                        accessibleTileCount++;
                    }
                }
            }
        }

        int targetAccessibleTileCount = this.roomParameters.RoomSize.X * this.roomParameters.RoomSize.Y - currentObstacleCount;

        return targetAccessibleTileCount == accessibleTileCount;
    }
    #endregion
}