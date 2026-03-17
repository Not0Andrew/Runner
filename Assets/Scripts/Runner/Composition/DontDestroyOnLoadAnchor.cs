using UnityEngine;

public sealed class DontDestroyOnLoadAnchor : MonoBehaviour
{
    private static DontDestroyOnLoadAnchor _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
