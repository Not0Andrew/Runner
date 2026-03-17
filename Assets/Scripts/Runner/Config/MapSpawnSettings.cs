using System;
using UnityEngine;

namespace Runner.Config
{
    [CreateAssetMenu(fileName = "MapSpawnSettings", menuName = "Runner/Map Spawn Settings")]
    [Serializable]
    public sealed class MapSpawnSettings : ScriptableObject
    {
        [Header("Chunk")]
        [Min(1f)]
        [SerializeField] private float chunkLength = 5f;

        [Header("Obstacle Spawn Cadence")]
        [Min(1)]
        [SerializeField] private int minObstacleRate = 1;
        [Min(1)]
        [SerializeField] private int maxObstacleRate = 3;

        [Header("Coin Spawn Cadence")]
        [Min(1)]
        [SerializeField] private int minCoinRate = 1;
        [Min(1)]
        [SerializeField] private int maxCoinRate = 3;

        [Header("Bonus Spawn Cadence")]
        [Min(1)]
        [SerializeField] private int minBonusRate = 8;
        [Min(1)]
        [SerializeField] private int maxBonusRate = 14;

        [Header("Spawn X Offset From Last Chunk")]
        [SerializeField] private Vector2 obstacleSpawnOffsetX = new Vector2(-3f, -1f);
        [SerializeField] private Vector2 coinSpawnOffsetX = new Vector2(-3.5f, -0.5f);
        [SerializeField] private Vector2 bonusSpawnOffsetX = new Vector2(-3.2f, -0.8f);

        [Header("Spawn Y")]
        [SerializeField] private Vector2 coinSpawnY = new Vector2(0.8f, 1.8f);
        [SerializeField] private Vector2 bonusSpawnY = new Vector2(0.9f, 1.7f);

        public float ChunkLength => chunkLength;
        public int MinObstacleRate => minObstacleRate;
        public int MaxObstacleRate => maxObstacleRate;
        public int MinCoinRate => minCoinRate;
        public int MaxCoinRate => maxCoinRate;
        public int MinBonusRate => minBonusRate;
        public int MaxBonusRate => maxBonusRate;
        public Vector2 ObstacleSpawnOffsetX => obstacleSpawnOffsetX;
        public Vector2 CoinSpawnOffsetX => coinSpawnOffsetX;
        public Vector2 BonusSpawnOffsetX => bonusSpawnOffsetX;
        public Vector2 CoinSpawnY => coinSpawnY;
        public Vector2 BonusSpawnY => bonusSpawnY;
    }
}
