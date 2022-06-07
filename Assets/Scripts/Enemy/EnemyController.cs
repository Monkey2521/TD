using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : ClickableObject, IDamageable, IMoveable, IPrioritable
{
    bool _isPriorityTarget;
    public bool IsPriorityTarget {
        get => _isPriorityTarget;
        set => _isPriorityTarget = value;
    }

    static EnemyController _priorityTarget;
    [SerializeField] GameObject _priorityMarker;

    [Header("Settings")]
    [SerializeField] EnemyStats _stats;
    public int Damage => _stats.Damage;
    public float Speed => _stats.Speed;

    [SerializeField] int _maxHP;
    int _hp;
    public int HP => _hp;
    public int MaxHP => _maxHP;

    [SerializeField] HealthBar _health;
    public HealthBar Health => _health;
    [Space(5)]
    [SerializeField] int _scorePoints;
    public int ScorePoints => _scorePoints;
    [SerializeField] int _goldReward;
    public int GoldReward => _goldReward;

    EnemySpawner _spawner;
    EventManager _eventManager;

    public void Init(EnemySpawner spawner)
    {
        _spawner = spawner;

        _hp = _maxHP;
        _health.UpdateHealth();
        
        if (_eventManager == null)
        {
            _eventManager = EventManager.GetEventManager();
            _eventManager.OnGameOver.AddListener(ReturnToPool);
        }
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is IDamageable && other.tag == "Player")
        {
            (other as IDamageable).TakeDamage(Damage);
        }
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        _health.UpdateHealth();

        if (_hp <= 0)
        {
            _eventManager.OnEnemyKilled?.Invoke(this);

            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform() => transform;

    public void Move()
    {

    }
    
    public void SetPriority()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (_priorityTarget != null)
        {
            _priorityTarget.IsPriorityTarget = false;
            _priorityTarget.SetPriority();
        }

        _priorityTarget = this;
        _isPriorityTarget = true;
        SetPriority();
    }
}
