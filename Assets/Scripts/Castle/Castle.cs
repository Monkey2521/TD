using UnityEngine;

public class Castle : MonoBehaviour, IDamageable
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    [SerializeField] int _maxHP;
    int _hp;
    public int HP => _hp;
    public int MaxHP => _maxHP;

    [SerializeField] HealthBar _health;
    public HealthBar Health => _health;

    EventManager _eventManager;

    void Start()
    {
        _eventManager = EventManager.GetEventManager();
        _eventManager.OnGameStart.AddListener(Restart);
    }

    void Restart()
    {
        _hp = _maxHP;
        gameObject.SetActive(true);

        _health.Init(this);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        _health.UpdateHealth();

        if (_hp <= 0)
        {
            gameObject.SetActive(false);

            _eventManager.OnGameOver?.Invoke();
        }
    }
} 
