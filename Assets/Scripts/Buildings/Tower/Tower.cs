using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : ClickableObject
{
    [Header("Settings")]
    [SerializeField][Range(10, 50)] int _buildCost;
    public int BuildCost => _buildCost;

    [SerializeField] TowerStats _stats;

    [Space(5)]
    [SerializeField] int _bulletPoolSize;
    [SerializeField] TowerBullet _bulletPrefab;

    List<TowerBullet> _bulletsPool = new List<TowerBullet>();
    List<TowerBullet> _bullets = new List<TowerBullet>();

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
    MoveSystem _moveSystem;

    public void Init()
    {
        _eventManager = EventManager.GetEventManager();
        _inventory = GoldInventory.GetInventory();

        _eventManager.OnGameOver.AddListener(OnGameOver);

        _moveSystem = MoveSystem.GetMoveSystem();

        for (int i = 0; i < _bulletPoolSize; i++)
        {
            TowerBullet bullet = Instantiate(_bulletPrefab,transform.position, Quaternion.identity, transform);
            bullet.Init(this, _stats.BulletSpeed, _stats.BulletDamage);

            _bulletsPool.Add(bullet);
        }
    }

    public void AddToPool(TowerBullet bullet)
    {
        bullet.transform.position = transform.position;

        _moveSystem.RemoveMoveable(bullet);

        if (_bullets.Contains(bullet))
        {
            _bullets.Remove(bullet);
        }

        _bulletsPool.Add(bullet);
    }

    void ReturnAllToPool()
    {
        foreach(TowerBullet bullet in _bullets)
        {
            bullet.ReturnToPool();
        }
    }

    void OnGameOver()
    {
        ReturnAllToPool();
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

    void Attack(IDamageable target)
    {
        TowerBullet bullet;

        if (_bulletsPool.Count == 0)
        {
            bullet = Instantiate(_bulletPrefab);
            bullet.Init(this, _stats.BulletSpeed, _stats.BulletDamage);

            _bulletsPool.Add(bullet);

            if (_isDebug) Debug.Log("Need more bullets");
        }

        bullet = _bulletsPool[0];
        
        _bullets.Add(bullet);
        _bulletsPool.Remove(bullet);

        bullet.Attack(target);

        _moveSystem.AddMoveable(bullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is IDamageable && other.tag == "Enemy")
        {
            Attack(other as IDamageable);
        }
    }
}