using PoolCode;
using Runner.Config;
using Runner.Core;
using UnityEngine;
using VContainer;

namespace MapCode
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private int chunksCount;
        [SerializeField] private int minObstacleRate;
        [SerializeField] private int maxObstacleRate;

        [SerializeField] private Pool chunksPool;
        [SerializeField] private Pool obstaclePool;
        [SerializeField] private Pool coinPool;
        [SerializeField] private Pool bonusPool;
        [SerializeField] private MapSpawnSettings spawnSettings;
        
        [SerializeField] private float mapBorder;
        [SerializeField] private float mapSpeed;
        [SerializeField] private int minCoinRate = 1;
        [SerializeField] private int maxCoinRate = 3;
        [SerializeField] private int minBonusRate = 8;
        [SerializeField] private int maxBonusRate = 14;

        private IGameStateService _gameStateService;
        private ISpeedService _speedService;

        private GameObject _lastChunk;
        private int _obstacleRate;
        private int _coinRate;
        private int _bonusRate;

        [Inject]
        public void Construct(IGameStateService gameStateService, ISpeedService speedService)
        {
            _gameStateService = gameStateService;
            _speedService = speedService;
        }

        private void Awake()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            _obstacleRate = RandomRate(GetMinObstacleRate(), GetMaxObstacleRate());
            _coinRate = RandomRate(GetMinCoinRate(), GetMaxCoinRate());
            _bonusRate = RandomRate(GetMinBonusRate(), GetMaxBonusRate());
            
            for (int i = 0; i < chunksCount; i++)
            {
                ActivateChunk();
            }
        }

        private void Update()
        {
            if (_gameStateService != null && _gameStateService.State != GameState.Playing)
                return;

            float currentSpeed = _speedService?.CurrentSpeed ?? mapSpeed;
            GameObject releasedChunk = null;
            
            foreach (var chunk in chunksPool.activeObject)
            {
                chunk.transform.position += Vector3.left * Time.deltaTime * currentSpeed;

                if (chunk.transform.position.x <= mapBorder)
                {
                    releasedChunk = chunk;
                }
            }

            if (releasedChunk != null)
            {
                chunksPool.DisableObject(releasedChunk);
                ActivateChunk();

                _obstacleRate--;

                if (_obstacleRate <= 0)
                {
                    _obstacleRate = RandomRate(GetMinObstacleRate(), GetMaxObstacleRate());
                    ActivateObstacle();
                }

                _coinRate--;
                if (_coinRate <= 0)
                {
                    _coinRate = RandomRate(GetMinCoinRate(), GetMaxCoinRate());
                    ActivateCoin();
                }

                _bonusRate--;
                if (_bonusRate <= 0)
                {
                    _bonusRate = RandomRate(GetMinBonusRate(), GetMaxBonusRate());
                    ActivateBonus();
                }
            }
            
            GameObject releasedObstacle = null;
            
            foreach (var obstacle in obstaclePool.activeObject)
            {
                obstacle.transform.position += Vector3.left * Time.deltaTime * currentSpeed;

                if (obstacle.transform.position.x <= mapBorder)
                {
                    releasedObstacle = obstacle;
                }
            }

            if (releasedObstacle != null)
            {
                obstaclePool.DisableObject(releasedObstacle);
            }
            UpdateAndRecyclePool(coinPool, currentSpeed);
            UpdateAndRecyclePool(bonusPool, currentSpeed);
        }

        private void ActivateChunk()
        {
            Vector3 pos = new Vector3(mapBorder, 0, 0);
            
            if (_lastChunk != null)
            { 
                pos = _lastChunk.transform.position + Vector3.right * GetChunkLength();
            }

            GameObject chunk = chunksPool.ActivateObject();
            chunk.transform.position = pos;
            
            _lastChunk = chunk;
        }

        private void ActivateObstacle()
        {
            GameObject obstacle = obstaclePool.ActivateObject();
            Vector2 offsetX = GetObstacleSpawnOffsetX();
            float xPos = Random.Range(_lastChunk.transform.position.x + offsetX.x, _lastChunk.transform.position.x + offsetX.y);
            obstacle.transform.position = new Vector3(xPos, 0, 0);
        }

        private void ActivateCoin()
        {
            if (coinPool == null)
                return;

            GameObject coin = coinPool.ActivateObject();
            Vector2 offsetX = GetCoinSpawnOffsetX();
            Vector2 spawnY = GetCoinSpawnY();
            float xPos = Random.Range(_lastChunk.transform.position.x + offsetX.x, _lastChunk.transform.position.x + offsetX.y);
            float yPos = Random.Range(spawnY.x, spawnY.y);
            coin.transform.position = new Vector3(xPos, yPos, coin.transform.position.z);
        }

        private void ActivateBonus()
        {
            if (bonusPool == null)
                return;

            GameObject bonus = bonusPool.ActivateObject();
            Vector2 offsetX = GetBonusSpawnOffsetX();
            Vector2 spawnY = GetBonusSpawnY();
            float xPos = Random.Range(_lastChunk.transform.position.x + offsetX.x, _lastChunk.transform.position.x + offsetX.y);
            float yPos = Random.Range(spawnY.x, spawnY.y);
            bonus.transform.position = new Vector3(xPos, yPos, bonus.transform.position.z);
        }

        private void UpdateAndRecyclePool(Pool pool, float currentSpeed)
        {
            if (pool == null)
                return;

            GameObject released = null;

            foreach (var item in pool.activeObject)
            {
                if (item.activeSelf == false)
                {
                    released = item;
                    break;
                }

                item.transform.position += Vector3.left * Time.deltaTime * currentSpeed;

                if (item.transform.position.x <= mapBorder)
                {
                    released = item;
                    break;
                }
            }

            if (released != null)
                pool.DisableObject(released);
        }

        private int RandomRate(int minInclusive, int maxInclusive)
        {
            if (maxInclusive < minInclusive)
                maxInclusive = minInclusive;

            return Random.Range(minInclusive, maxInclusive + 1);
        }

        private int GetMinObstacleRate() => spawnSettings != null ? spawnSettings.MinObstacleRate : minObstacleRate;
        private int GetMaxObstacleRate() => spawnSettings != null ? spawnSettings.MaxObstacleRate : maxObstacleRate;
        private int GetMinCoinRate() => spawnSettings != null ? spawnSettings.MinCoinRate : minCoinRate;
        private int GetMaxCoinRate() => spawnSettings != null ? spawnSettings.MaxCoinRate : maxCoinRate;
        private int GetMinBonusRate() => spawnSettings != null ? spawnSettings.MinBonusRate : minBonusRate;
        private int GetMaxBonusRate() => spawnSettings != null ? spawnSettings.MaxBonusRate : maxBonusRate;
        private float GetChunkLength() => spawnSettings != null ? spawnSettings.ChunkLength : 5f;
        private Vector2 GetObstacleSpawnOffsetX() => spawnSettings != null ? spawnSettings.ObstacleSpawnOffsetX : new Vector2(-3f, -1f);
        private Vector2 GetCoinSpawnOffsetX() => spawnSettings != null ? spawnSettings.CoinSpawnOffsetX : new Vector2(-3.5f, -0.5f);
        private Vector2 GetBonusSpawnOffsetX() => spawnSettings != null ? spawnSettings.BonusSpawnOffsetX : new Vector2(-3.2f, -0.8f);
        private Vector2 GetCoinSpawnY() => spawnSettings != null ? spawnSettings.CoinSpawnY : new Vector2(0.8f, 1.8f);
        private Vector2 GetBonusSpawnY() => spawnSettings != null ? spawnSettings.BonusSpawnY : new Vector2(0.9f, 1.7f);
    }
}
