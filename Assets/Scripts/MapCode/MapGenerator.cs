using System;
using PoolCode;
using UnityEngine;

namespace MapCode
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private int chunksCount;

        [SerializeField] private Pool chunksPool;
        
        [SerializeField] private float mapBorder;
        [SerializeField] private float mapSpeed;

        private GameObject _lastChunk;

        private void Awake()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
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
                chunk.transform.position += Vector3.left * Time.deltaTime;

                if (chunk.transform.position.x <= mapBorder)
                {
                    releasedChunk = chunk;
                }
            }

            if (releasedChunk != null)
            {
                chunksPool.DisableObject(releasedChunk);
                ActivateChunk();
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
            
        }
    }
}
