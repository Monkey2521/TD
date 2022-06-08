using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Debug settings")]
    bool _isDebug;

    [Header("Settings")]
    [SerializeField] List<EnemyType> _enemiesPrefabs = new List<EnemyType>();
    List<List<EnemyController>> _enemiesPool = new List<List<EnemyController>>();
    List<List<EnemyController>> _enemies = new List<List<EnemyController>>();

    [SerializeField][Range(0.001f, 10f)] float _spawnTime;
    [SerializeField] Image _changeSpawnButton;
    [SerializeField] Sprite _onSpawningSprite;
    [SerializeField] Sprite _onPausedSprite;

    bool _isSpawning;

    EventManager _eventManager;
    MoveSystem _moveSystem;
    GameManager _gameManager;
    
    void Start()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnGameOver.AddListener(ChangePool);
        _eventManager.OnGameOver.AddListener(ReturnAllToPool);

        _moveSystem = MoveSystem.GetMoveSystem();

        _gameManager = GameManager.GetGameManager();

        foreach(EnemyType enemyType in _enemiesPrefabs)
        {
            List<EnemyController> enemies = new List<EnemyController>();

            for (int i = 0; i < enemyType.PoolSize; i++)
            {
                EnemyController enemy = Instantiate(enemyType.Enemy, transform);

                enemy.transform.position = transform.position;
                enemy.Init(this);

                enemy.gameObject.SetActive(false);

                enemies.Add(enemy);
            }

            _enemiesPool.Add(enemies);
            _enemies.Add(new List<EnemyController>());
        }

        _changeSpawnButton.sprite = _onPausedSprite;
    }

    void ReturnAllToPool()
    {
        foreach (List<EnemyController> enemies in _enemies)
            while(enemies.Count > 0)
            {
                enemies[0].ReturnToPool();
            }
    }

    public void AddToPool(EnemyController enemy)
    {
        enemy.transform.position = transform.position;

        int index = IndexOfEnemyType(enemy);
        if (index == -1)
        {
            if (_isDebug) Debug.Log("Enemy not found!");
            return;
        }

        if (_enemies[index].Contains(enemy))
        {
            _enemies[index].Remove(enemy);
        }

        _enemiesPool[index].Add(enemy);
    }

    public void ChangePool()
    {
        _isSpawning = !_isSpawning;

        switch (_isSpawning)
        {
            case true:
                _changeSpawnButton.sprite = _onSpawningSprite;
                PoolEnemy();
                break;
            case false:
                _changeSpawnButton.sprite = _onPausedSprite;
                break;
        }
    }

    void PoolEnemy()
    {
        if (!_isSpawning) return;

        EnemyController enemy;

        enemy = _enemiesPool[0][0];

        enemy.gameObject.SetActive(true);
        _enemies[0].Add(enemy);
        _enemiesPool[0].Remove(enemy);

        enemy.SetNavTarget(_gameManager.Castle.GetTransform().position);

        WaitForPool();
    }
    async void WaitForPool()
    {
        await System.Threading.Tasks.Task.Delay((int)(_spawnTime * 1000));

        PoolEnemy();
    }

    int IndexOfEnemyType(EnemyController enemy)
    {
        int index = 0;

        foreach(EnemyType enemies in _enemiesPrefabs)
        {
            if (enemies.Enemy.Type == enemy.Type)
                return index;
            index++;
        }

        return -1;
    }
}
