using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : ClickableObject
{
    [Header("Settings")]
    [SerializeField][Range(10, 50)] int _buildCost;
    public int BuildCost => _buildCost;

    [SerializeField] TowerStats _stats;

    [Header("Upgrade settings")]
    [SerializeField][Range(1, 20)] int _maxLevel;
    int _level;
    public int MaxLevel => _maxLevel;
    public int Level => _level;

    [SerializeField][Range(5, 50)] int _upgradeCost;
    [SerializeField][Range(5, 50)] int _additionalCost;
    public int UpgradeCost => _upgradeCost + _additionalCost * (_level - 1);

    [SerializeField] TowerStats _upgradeStatsPerLevel;

    EventManager _eventManager;
    GoldInventory _inventory;

    public void Init()
    {
        _eventManager = EventManager.GetEventManager();
        _inventory = GoldInventory.GetInventory();

        _eventManager.OnGameOver.AddListener(OnGameOver);
    }

    void OnGameOver()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (Upgrade())
        {
            _level++;
            if (_isDebug) Debug.Log("Upgrade tower, level = " + Level);
        }
    }

    bool Upgrade ()
    {
        return (_level < _maxLevel && _inventory.RemoveGold(UpgradeCost));
    }

    void Attack()
    {

    }
}