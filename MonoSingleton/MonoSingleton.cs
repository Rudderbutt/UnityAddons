using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField, Tooltip("Print debug information. Default is false (don't print debug information).")]
    bool debugLog = false;
    [SerializeField, Tooltip("Do or don't destroy object on load. Default is true (destroy on load).")]
    bool destroyOnLoad = true;

    static T s_instance;

    public static T Instance
    {
        get
        {
            return s_instance ?? new GameObject(typeof(T).Name).AddComponent<T>();
        }
    }

    protected virtual void Awake()
    {
        if (s_instance)
        {
            if (debugLog)
                Debug.LogWarningFormat("A {0} instance was previously initialized.", typeof(T).Name);

            Destroy(gameObject);
            return;
        }

        s_instance = GetComponent<T>();

        if (!destroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (!s_instance)
            s_instance = GetComponent<T>();
    }

    protected virtual void OnDestroy()
    {
        if (s_instance != GetComponent<T>())
            return;


        s_instance = null;
    }
}
