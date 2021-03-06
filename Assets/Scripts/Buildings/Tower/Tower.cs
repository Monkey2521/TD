using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Mathf;

public class Tower : ClickableObject
{
    bool _canAttack;

    IDamageable _target;
    public IDamageable Target => _target;

    [Header("Settings")]
    [SerializeField] string _name;
    [SerializeField] Sprite _icon;
    public string Name => _name;
    public Sprite Icon => _icon;

    [SerializeField] ParticleSystem _upgradeParticle;

    [SerializeField] LineRenderer _attackRangeLine;
    [SerializeField] Material _emptyRange;
    [SerializeField] Material _onEnemyRange;
    bool _onEnemyInRange;

    public readonly int MAX_LINE_POSITIONS_COUNT = 360;

    [Header("Stats settings")]
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

    [HideInInspector] public Buildplace Buildplace;

    public void Init(Buildplace buildplace)
    {
        this.Buildplace = buildplace;

        _eventManager = EventManager.GetEventManager();
        _eventManager.OnGameOver.AddListener(OnGameOver);
        _eventManager.OnEnemyKilled.AddListener(CheckEnemy);
        _eventManager.OnEnemyReturnToPool.AddListener(CheckEnemy);

        _inventory = GoldInventory.GetInventory();

        _moveSystem = MoveSystem.GetMoveSystem();

        for (int i = 0; i < _bulletPoolSize; i++)
        {
            TowerBullet bullet = Instantiate(_bulletPrefab,transform.position, Quaternion.identity, transform);
            bullet.Init(this, _stats.BulletSpeed, _stats.BulletDamage);

            _bulletsPool.Add(bullet);
        }

        _attackRangeCollider.radius = _stats.AttackRange;
        _canAttack = true;

        _attackRangeLine.loop = true;
        _attackRangeLine.positionCount = MAX_LINE_POSITIONS_COUNT;

        InitRange();

        _attackRangeLine.enabled = false;

        _upgradeParticle.maxParticles = 1;
        PlayParticle();
    }

    void InitRange()
    {
        for (int i = 0; i < MAX_LINE_POSITIONS_COUNT; i++)
        {
            _attackRangeLine.SetPosition(i, new Vector3(Cos(Deg2Rad * i) * AttackRange, 0f, Sin(Deg2Rad * i) * AttackRange));
        }
    }

    public void ChangeRange()
    {
        _attackRangeLine.enabled = !_attackRangeLine.enabled;
        SetRangeMaterial();
    }

    void SetRangeMaterial()
    {
        _attackRangeLine.material = _onEnemyInRange ? _onEnemyRange : _emptyRange;
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
        while(_bullets.Count > 0)
        {
            _bullets[0].ReturnToPool();
        }
    }

    void CheckEnemy(EnemyController enemy)
    {
        if (enemy as IDamageable == _target)
        {
            ReturnAllToPool();
            _target = null;

            _onEnemyInRange = false;
            SetRangeMaterial();
        }
    }

    void OnGameOver()
    {
        ReturnAllToPool();
        Destroy(gameObject);
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

            foreach (TowerBullet bullet in _bullets)
            {
                bullet.Upgrade(_upgradeStatsPerLevel.BulletSpeed, _upgradeStatsPerLevel.BulletDamage);
            }
            foreach (TowerBullet bullet in _bulletsPool)
            {
                bullet.Upgrade(_upgradeStatsPerLevel.BulletSpeed, _upgradeStatsPerLevel.BulletDamage);
            }

            PlayParticle();
            _upgradeParticle.maxParticles++;

            InitRange();

            if (_isDebug) Debug.Log("Upgrade tower, level = " + Level);
            
            return true;
        }

        return false;

    }

    public void PlayParticle()
    {
        _upgradeParticle.Play();
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

        _canAttack = false;
        WaitForAttack();
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyController enemy;

        if (other.tag == "Enemy")
        {
            enemy = other.GetComponent<EnemyController>();

            _onEnemyInRange = true;
            SetRangeMaterial();
        }
        else return;

        if (enemy.IsPriorityTarget)
        {
            _target = enemy;

            Attack(_target);

        }
        else if (_target == null)
        {
            _target = enemy;

            Attack(_target);
        }
    }

    void OnTriggerStay(Collider other)
    {
        EnemyController enemy;

        if (other.tag == "Enemy")
        {
            enemy = other.GetComponent<EnemyController>();

            _onEnemyInRange = true;
            SetRangeMaterial();
        }
        else return;
        
        if (enemy.IsPriorityTarget)
        {
            _target = enemy;
            Attack(_target);
        }
        else if (_target == null)
        {
            _target = enemy;
            Attack(enemy);
        }
        else if (enemy as IDamageable == _target)
        {
            Attack(_target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        EnemyController enemy;

        if (other.tag == "Enemy")
        {
            enemy = other.GetComponent<EnemyController>();

            _onEnemyInRange = false;
            SetRangeMaterial();
        }
        else return;

        if (enemy as IDamageable == _target) _target = null;
    }
}