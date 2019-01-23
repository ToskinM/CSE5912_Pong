using System.Linq;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T instance = null;
    public static T Instance
    {
        get
        {
            if (!instance)
                instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return instance;
        }
    }
}
