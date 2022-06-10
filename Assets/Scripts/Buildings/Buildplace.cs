using UnityEngine;
using UnityEngine.EventSystems;

public class Buildplace : ClickableObject
{
    [Header("Settings")]
    Tower _tower;
    public Tower Tower => _tower;

    bool _isEmpty = true;
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

        _eventManager.OnBuildplaceClick?.Invoke(this);
    }

    public bool Build(Tower tower)
    {
        if (_isEmpty && _inventory.RemoveGold(tower.BuildCost))
        {
            _tower = Instantiate(tower, transform.position + Vector3.up, Quaternion.identity, transform);
            _tower.Init(this);

            _isEmpty = false;

            return true;
        }

        return false;
    }
}
