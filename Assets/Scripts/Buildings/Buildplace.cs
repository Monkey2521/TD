using UnityEngine;
using UnityEngine.EventSystems;

public class Buildplace : ClickableObject
{
    [Header("Settings")]
    [SerializeField] Tower _towerPrefab;
    Tower _tower;

    bool _isEmpty;
    public bool IsEmpty => _isEmpty;

    GoldInventory _inventory;
    EventManager _eventManager;

    void Start()
    {
        _inventory = GoldInventory.GetInventory();

        _eventManager = EventManager.GetEventManager();
        _eventManager.OnGameStart.AddListener(Restart);
    }

    void Restart()
    {
        _isEmpty = true; 

        Destroy(_tower);
        _tower = null;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (_isEmpty && _inventory.RemoveGold(_towerPrefab.Cost))
        {
           _tower = Instantiate(_towerPrefab, transform.position + Vector3.up, Quaternion.identity, transform);

            _isEmpty = false;
        } 
    }
}
