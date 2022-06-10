using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Mathf;

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

        ChangeTowerRange();
    }

    public void ChangeTowerRange()
    {
        if (_tower != null)
        {
            _tower.ChangeRange();
        }
    }

    public bool Build(Tower tower)
    {
        if (_isEmpty && _inventory.RemoveGold(tower.BuildCost))
        {
            //_attackRangePreview.enabled = false;

            _tower = Instantiate(tower, transform.position + Vector3.up, Quaternion.identity, transform);
            _tower.Init(this);
            ChangeTowerRange();

            _isEmpty = false;

            return true;
        }

        return false;
    }
/*
    public void ShowTowerRange(Tower tower)
    {
        _attackRangePreview.enabled = true;

        for (int i = 0; i < tower.MAX_LINE_POSITIONS_COUNT; i++)
        {
            _attackRangePreview.SetPosition(i, new Vector3(Cos(Deg2Rad * i) * tower.AttackRange, 0f, Sin(Deg2Rad * i) * tower.AttackRange));
        }
    }*/
}
