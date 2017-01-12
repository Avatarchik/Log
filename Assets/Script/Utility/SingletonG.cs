using UnityEngine;
using System.Collections;

public abstract class SingletonG<T> : MonoBehaviour where T : SingletonG<T>
{
    private static T instance_ = null;

    public static T Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType(typeof(T)) as T;

                if (instance_ == null)
                    Debug.LogError("No instance of " + typeof(T).ToString());
            }
            return instance_;
        }
    } 
}