using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    protected bool dontDestroy = false;
    protected string path;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (dontDestroy)
            {
                DontDestroyOnLoad(this as T);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
