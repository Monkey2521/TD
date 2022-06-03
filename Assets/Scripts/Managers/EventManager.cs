using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField]bool _isDebug;

    [Header("Events")]
    public UnityEvent OnInventoryUpdate;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;

    static EventManager _instance;
    public static EventManager GetEventManager() => _instance;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_isDebug) Debug.Log("EventManager already created");
            Destroy(gameObject);
        }
    }
}
