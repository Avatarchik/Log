using UnityEngine;
using System.Collections;

public abstract class SingletonT<T> where T : new()
{
    private static readonly object padlock_ = new object();
    private static T instance_ = default(T);

    public static T Instance
    {
        get
        {
            lock (padlock_)
            {
                if (instance_ == null)
                {
                    instance_ = new T();
                }
                return instance_;
            }
        }
    }
}