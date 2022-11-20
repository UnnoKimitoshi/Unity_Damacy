using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected abstract bool DontDestroy { get; }

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"{typeof(T)}のインスタンスが存在しません");
                }
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (DontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
