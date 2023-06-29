using System.Collections.Generic;
using UnityEngine;

namespace PoolCode
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] public List<GameObject> activeObject;
        [SerializeField] private List<GameObject> inactiveObjects;

        public GameObject ActivateObject()
        {
            int rand = Random.Range(0, inactiveObjects.Count);
            GameObject obj = inactiveObjects[rand];

            inactiveObjects.Remove(obj);
            activeObject.Add(obj);
            
            obj.SetActive(true);

            return obj;
        }

        public void DisableObject(GameObject obj)
        {
            activeObject.Remove(obj);
            inactiveObjects.Add(obj);
            obj.SetActive(false);
        }
    }
}
