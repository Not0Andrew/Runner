using PoolCode;
using UnityEngine;

namespace MapCode
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private int chunksCount;
        [SerializeField] private int minObstacleRate;
        [SerializeField] private int maxObstacleRate;

        [SerializeField] private Pool chunksPool;
        [SerializeField] private Pool obstaclePool;
        
        [SerializeField] private float mapBorder;
        [SerializeField] private float mapSpeed;

        private GameObject _lastChunk;
        private int _obstacleRate;

        private void Awake()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            _obstacleRate = Random.Range(minObstacleRate, maxObstacleRate);
            
            for (int i = 0; i < chunksCount; i++)
            {
                ActivateChunk();
            }
        }

        private void Update()
        {
            GameObject releasedChunk = null;
            
            foreach (var chunk in chunksPool.activeObject)
            {
                chunk.transform.position += Vector3.left * Time.deltaTime * mapSpeed;

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
                    _obstacleRate = Random.Range(minObstacleRate, maxObstacleRate);
                    ActivateObstacle();
                }
            }
            
            GameObject releasedObstacle = null;
            
            foreach (var obstacle in obstaclePool.activeObject)
            {
                obstacle.transform.position += Vector3.left * Time.deltaTime * mapSpeed;

                if (obstacle.transform.position.x <= mapBorder)
                {
                    releasedObstacle = obstacle;
                }
            }

            if (releasedObstacle != null)
            {
                obstaclePool.DisableObject(releasedObstacle);
            }
        }

        private void ActivateChunk()
        {
            Vector3 pos = new Vector3(mapBorder, 0, 0);
            
            if (_lastChunk != null)
            { 
                pos = _lastChunk.transform.position + Vector3.right * 5;
            }

            GameObject chunk = chunksPool.ActivateObject();
            chunk.transform.position = pos;
            
            _lastChunk = chunk;
        }

        private void ActivateObstacle()
        {
            GameObject obstacle = obstaclePool.ActivateObject();

            float xPos = Random.Range(_lastChunk.transform.position.x - 3f, _lastChunk.transform.position.x - 1f);
            obstacle.transform.position = new Vector3(xPos, 0, 0);
        }
    }
}
