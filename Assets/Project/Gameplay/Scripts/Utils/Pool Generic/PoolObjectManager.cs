using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSource.Utils.Pool
{ 
    public class PoolObjectManager : MonoBehaviour
    {
        [Header("Object Pool")]
        [SerializeField] private PoolObject poolObject;
        [SerializeField] private int maxObjects, initialAmount;
        private int currentObjects;
        private PoolGeneric<PoolObject> objectPool;
        private List<PoolObject> instantiatedObject = new List<PoolObject>();

        [Header("Object Attributes")]
        [SerializeField] private float rate;
        [SerializeField] private float speed, duration;

        [Header("Spawning Attributes")]
        [SerializeField] private float yMinRandomValue;
        [SerializeField] private float yMaxRandomValue;
        [SerializeField] private bool swapSide;

        private void Awake()
        {
            objectPool = new PoolGeneric<PoolObject>(CreateNewObject, PoolObject.TurnOn, PoolObject.TurnOff, initialAmount);
            EventManager.Subscribe("OnObjectReturnToPool", SubstractObject);
            EventManager.Subscribe("OnResetGame", ResetSpawners);
        }

        private void Start() { StartCoroutine(SpawnObjects()); }

        private PoolObject CreateNewObject() { return Instantiate(poolObject); }

        private IEnumerator SpawnObjects()
        {
            while (true)
            {
                if (currentObjects < maxObjects)
                {
                    PositioningPipe();
                    yield return new WaitForSeconds(rate);
                }
                else yield return new WaitForEndOfFrame();
            }
        }

        private void PositioningPipe()
        {
            poolObject = objectPool.Get();
            poolObject.SetPool(objectPool);
            poolObject.SetSpeed(speed);
            poolObject.SetDuration(duration);
            poolObject.transform.position = transform.position + new Vector3(0, Random.Range(yMinRandomValue, yMaxRandomValue), 0);

            if(swapSide)
            {
                int n = Random.Range(0, 2) * 2 - 1;
                Vector3 objectScale = poolObject.transform.localScale;
                poolObject.transform.localScale = new Vector3(objectScale.x * n, objectScale.y, objectScale.z);
            }

            instantiatedObject.Add(poolObject);
            currentObjects++;
        }

        public void SubstractObject(params object[] parameters)
        {
            instantiatedObject.Remove((PoolObject)parameters[0]);
            currentObjects--;
        }

        public void ResetSpawners(params object[] parameters)
        {
            StopAllCoroutines();
            StartCoroutine(SpawnObjects());
        }
    }
}
