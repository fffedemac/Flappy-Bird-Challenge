using System;
using System.Collections.Generic;

namespace GameSource.Utils.Pool
{ 
    public class PoolGeneric<T>
    {
        private List<T> uninstantiated = new List<T>();
        private Func<T> instantiateMethod;
        private Action<T> turnOn;
        private Action<T> turnOff;

        public PoolGeneric(Func<T> instantiateMethod, Action<T> turnOn, Action<T> turnOff, int initialAmount)
        {
            this.instantiateMethod = instantiateMethod;
            this.turnOn = turnOn;
            this.turnOff = turnOff;

            for (var i = 0; i < initialAmount; i++)
            {
                var obj = instantiateMethod();
                this.turnOff(obj);
                uninstantiated.Add(obj);
            }
        }

        public T Get()
        {
            T obj;

            if (uninstantiated.Count > 0)
            {
                obj = uninstantiated[0];
                uninstantiated.Remove(obj);
            }
            else
                obj = instantiateMethod();

            turnOn(obj);

            return obj;
        }

        public void ReturnToPool(T obj)
        {
            uninstantiated.Add(obj);
            turnOff(obj);
        }
    }
}