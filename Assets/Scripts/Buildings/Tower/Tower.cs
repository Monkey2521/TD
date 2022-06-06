using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : ClickableObject
{
    bool _canAttack;

    IDamageable _target;
    public IDamageable Target => _target;

    [Header("Settings")]
    [SerializeField][Range(10, 50)] int _buildCost;
    public int BuildCost => _buildCost;

    int _level = 1;

    [SerializeField] TowerStats _stats;
    public float AttackTime => _stats.AttackTime - _upgradeStatsPerLevel.AttackTime * (_level > 0 ? _level - 1 : 0);
    public float UpperAttackTime => _stats.AttackTime - _upgradeStatsPerLevel.AttackTime * _level;
    public int Damage => _stats.BulletDamage + _upgradeStatsPerLevel.BulletDamage * (_level > 0 ? _level - 1 : 0);
    public int UpperDamage => _stats.BulletDamage + _upgradeStatsPerLevel.BulletDamage * _level;
    public float AttackRange => _stats.AttackRange + _upgradeStatsPerLevel.AttackRange * (_level > 0 ? _level - 1 : 0);
    public float UpperAttackRange => _stats.AttackRange + _upgradeStatsPerLevel.AttackRange * _level;

    [Space(5)]
    [SerializeField] SphereCollider _attackRangeCollider;

    [SerializeField] int _bulletPoolSize;
    [SerializeField] TowerBullet _bulletPrefab;

    List<TowerBullet> _bulletsPool = new List<TowerBullet>();
    List<TowerBullet> _bullets = new List<TowerBullet>();

    [Header("Upgrade settings")]
    [SerializeField][Range(1, 20)] int _maxLevel;
    public int MaxLevel => _maxLevel;
    public int Level => _level;

    [SerializeField][Range(5, 50)] int _upgradeCost;
    [SerializeField][Range(5, 50)] int _additionalCost;
    public int UpgradeCost => _upgradeCost + _additionalCost * (_level > 0 ? _level - 1 : 0);

    [SerializeField] TowerStats _upgradeStatsPerLevel;

    EventManager _eventManager;
    GoldInventory _inventory;
    MoveSystem _moveSystem;

    public Buildplace Buildplace;

    public void Init(Buildplace buildplace)
    {
        this.Buildplace = buildplace;

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

        _attackRangeCollider.radius = _stats.AttackRange;
        _canAttack = true;
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

        _eventManager.OnTowerClick?.Invoke(this);
    }

    public bool Upgrade ()
    {
        if (_level < _maxLevel && _inventory.RemoveGold(UpgradeCost))
        {
            _level++;

            _attackRangeCollider.radius += _upgradeStatsPerLevel.AttackRange;

            if (_isDebug) Debug.Log("Upgrade tower, level = " + Level);
            
            return true;
        }

        return false;

    }

    async void WaitForAttack() 
    {
        await Task.Delay((int)(AttackTime * 1000));

        _canAttack = true;
    }

    void Attack(IDamageable target)
    {
        if (!_canAttack) return;

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

    void OnTriggerEnter(Collider other)
    {
        if (other is IDamageable && other.tag == "Enemy" && _target == null)
        {
            _target = other as IDamageable;

            Attack(_target);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other as IDamageable == _target)
        {
            Attack(_target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other as IDamageable == _target) _target = null;
    }
}