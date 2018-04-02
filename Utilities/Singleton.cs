using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance
    {
        get { return instance; }
    }
    protected static T instance = null;

    public bool dontDestroyOnLoad = false;

    public virtual void Start()
    {
        // catches any new instances of this type
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);

        instance = this as T;
    }
}
