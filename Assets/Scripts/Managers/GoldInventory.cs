using UnityEngine;

public class GoldInventory : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    [SerializeField][Range(0, 5000)] int _startGold;
    [SerializeField] int _goldCount;

    static GoldInventory _instance;
    public static GoldInventory GetInventory() => _instance;

    EventManager _eventManager;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_isDebug) Debug.Log("Inventory already created");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnGameStart.AddListener(Restart);
        _eventManager.OnEnemyKilled.AddListener(AddGold);
    }

    void Restart()
    {
        _goldCount = _startGold;
        _eventManager.OnInventoryUpdate?.Invoke();
    }

    public int GetGold() => _goldCount;

    public void AddGold(int count)
    {
        _goldCount += count;
        _eventManager.OnInventoryUpdate?.Invoke();
    }

    void AddGold(EnemyController enemy)
    {
        _goldCount += enemy.GoldReward;
        _eventManager.OnInventoryUpdate?.Invoke();
    }

    public bool RemoveGold(int count)
    {
        if (_goldCount >= count)
        {
            _goldCount -= count;

            _eventManager.OnInventoryUpdate?.Invoke();

            return true;
        }

        return false;
    }
}
