using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyController : ClickableObject, IDamageable, IPrioritable
{
    [SerializeField] bool _isPriorityTarget;
    public bool IsPriorityTarget {
        get => _isPriorityTarget;
        set => _isPriorityTarget = value;
    }

    static EnemyController _priorityTarget;

    [Header("Settings")]
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] GameObject _priorityMarker;

    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] EnemyTypes _enemyType;
    public EnemyTypes Type => _enemyType;

    [SerializeField] HealthBar _health;
    public HealthBar Health => _health;

    [Space(5)]
    [SerializeField] int _scorePoints;
    public int ScorePoints => _scorePoints;
    [SerializeField] int _goldReward;
    public int GoldReward => _goldReward;

    [Header("Stats settings")]
    [SerializeField] EnemyStats _stats;
    public int Damage => _stats.Damage;
    public float Speed => _stats.Speed;

    [SerializeField] int _maxHP;
    int _hp;
    public int HP => _hp;
    public int MaxHP => _maxHP;

    EnemySpawner _spawner;
    EventManager _eventManager;

    public void Init(EnemySpawner spawner)
    {
        _spawner = spawner;
        _health.Init(this);

        _hp = _maxHP;
        _health.UpdateHealth();
        
        if (_eventManager == null)
        {
            _eventManager = EventManager.GetEventManager();
            _eventManager.OnGameOver.AddListener(ReturnToPool);
        }

        _priorityMarker = Instantiate(_priorityMarker, transform.position + Vector3.up, Quaternion.identity, transform);
        _priorityMarker.SetActive(false);

        _navAgent.speed = Speed;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        _spawner.AddToPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Castle>().TakeDamage(Damage);
            
            if (_isPriorityTarget)
            {
                _priorityTarget = null;
                _isPriorityTarget = false;
                SetPriority();
            }

            ReturnToPool();
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

    public void SetNavTarget(Vector3 targetPosition)
    {
        _navAgent.destination = targetPosition;
    }
    
    public void SetPriority()
    {
        _priorityMarker.SetActive(_isPriorityTarget);
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
