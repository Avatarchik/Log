using UnityEngine;
using System.Collections;

public abstract class SingletonC<T> : MonoBehaviour where T : MonoBehaviour
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
                    GameObject container = new GameObject(typeof(T).ToString());

                    instance_ = container.AddComponent<T>();

                    DontDestroyOnLoad(container);
                }
                return instance_;
            }
        }
    }

    public static void Create()
    {
        lock (padlock_)
        {
            if (instance_ == null)
            {
                GameObject container = new GameObject(typeof(T).ToString());

                instance_ = container.AddComponent<T>();

                DontDestroyOnLoad(container);
            }
        }
    }
}