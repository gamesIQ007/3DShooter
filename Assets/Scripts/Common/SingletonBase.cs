using UnityEngine;

/// <summary>
/// Базовый класс для синглтонов
/// </summary>
[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] private bool doNotDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exist, instance will be destroyed = " + typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (doNotDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Destroy()
    {
        Instance = null;
    }
}
