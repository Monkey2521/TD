using UnityEngine;

public class GoldInventory : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    int _goldCount;

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

        _eventManager = EventManager.GetEventManager();
    }

    public int GetGold() => _goldCount;

    public void AddGold(int count)
    {
        _goldCount += count;
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
