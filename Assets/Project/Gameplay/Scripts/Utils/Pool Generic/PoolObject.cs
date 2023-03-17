using UnityEngine;

namespace GameSource.Utils.Pool
{
    public class PoolObject : MonoBehaviour
    {
        private float speed, duration, durationTimer;
        private PoolGeneric<PoolObject> pool;

        private void Awake() { EventManager.Subscribe("OnResetGame", ResetObject); }

        private void Update()
        {
            transform.position += transform.right * -speed * Time.deltaTime;

            durationTimer += Time.deltaTime;
            if (durationTimer >= duration) ResetObject();
        }

        public static void TurnOn(PoolObject pooledObject) { pooledObject.gameObject.SetActive(true); }

        public static void TurnOff(PoolObject pooledObject) { pooledObject.gameObject.SetActive(false); }

        private void ResetObject(params object[] parameters)
        {
            if (!gameObject.activeInHierarchy) return;

            durationTimer = 0;
            EventManager.Trigger("OnObjectReturnToPool", this);
            pool.ReturnToPool(this);
        }

        public virtual void SetSpeed(float speed) { this.speed = speed; }

        public virtual void SetDuration(float duration) { this.duration = duration; }

        public virtual void SetPool(PoolGeneric<PoolObject> pool) { this.pool = pool; }
    }
}
