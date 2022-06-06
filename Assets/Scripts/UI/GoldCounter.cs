using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text _goldText;
    
    EventManager _eventManager;
    GoldInventory _inventory;

    void Start()
    {
        _eventManager = EventManager.GetEventManager();
        _eventManager.OnInventoryUpdate.AddListener(UpdateCounter);
        _eventManager.OnGameStart.AddListener(Restart);

        _inventory = GoldInventory.GetInventory();
    }

    void UpdateCounter()
    {
        _goldText.text = _inventory.GetGold().ToString();
    }

    void Restart()
    {
        _goldText.text = "0";
    }
}
