using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTPool
{
    public abstract class BasePool
    {
        protected List<PoolOnject> activityPoolObjects;
        protected List<PoolOnject> inactivityPoolObjects;


        public virtual void Init()
        {
            activityPoolObjects = new List<PoolOnject>();
            inactivityPoolObjects = new List<PoolOnject>();
        }

        public virtual void AddPool(PoolObjectStateType poolObjectState, string targetName, ISources target)
        {
            PoolOnject poolOnject = default(PoolOnject);
            switch (poolObjectState)
            {
                case PoolObjectStateType.Activity:
                    if (!target.Target.activeInHierarchy)
                        target.Target.SetActive(true);
                    poolOnject = new PoolOnject();
                    poolOnject.targetName = targetName;
                    poolOnject.sources = target;
                    activityPoolObjects.Add(poolOnject);
                    break;
                case PoolObjectStateType.Inactivity:
                    if (target.Target.activeInHierarchy)
                        target.Target.SetActive(false);
                    poolOnject = activityPoolObjects.Find(p => p.targetName == targetName && p.sources == target);
                    if (poolOnject != null)
                    {
                        activityPoolObjects.Remove(poolOnject);
                        inactivityPoolObjects.Add(poolOnject);
                    }
                    else
                    {
                        poolOnject = new PoolOnject();
                        poolOnject.targetName = targetName;
                        poolOnject.sources = target;
                        inactivityPoolObjects.Add(poolOnject);
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual T GetPool<T>(PoolObjectStateType poolObjectState, string targetName)
        {
            Debug.Log(activityPoolObjects.Count + "====" + inactivityPoolObjects.Count);
            T t = default(T);

            PoolOnject poolOnject = default(PoolOnject);
            switch (poolObjectState)
            {
                case PoolObjectStateType.Activity:
                    poolOnject = activityPoolObjects.Find(p => p.targetName == targetName);
                    activityPoolObjects.Remove(poolOnject);
                    break;
                case PoolObjectStateType.Inactivity:
                    poolOnject = inactivityPoolObjects.Find(p => p.targetName == targetName);
                    inactivityPoolObjects.Remove(poolOnject);
                    break;
                default:
                    break;
            }
            if (poolOnject != null)
            {
                if (!poolOnject.sources.Equals(null))
                {
                    t = poolOnject.sources.Target.GetComponent<T>();
                }
                else
                {
                    activityPoolObjects.Remove(poolOnject);
                    inactivityPoolObjects.Remove(poolOnject);
                }
            }
            return t;
        }

        public virtual void Clear()
        {
            activityPoolObjects.Clear();
            inactivityPoolObjects.Clear();
        }

    }
}
