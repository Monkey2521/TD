using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Debug settings")]
    bool _isDebug;

    [Header("Settings")]
    [SerializeField] List<EnemyType> _enemiesPrefabs = new List<EnemyType>();
    List<List<EnemyController>> _enemiesPool = new List<List<EnemyController>>();
    List<List<EnemyController>> _enemies = new List<List<EnemyController>>();

    bool _isSpawning;

    EventManager _eventManager;
    
    void Start()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnGameOver.AddListener(ReturnAllToPool);

        _eventManager.OnGameOver.AddListener(ChangePool);
        _eventManager.OnGameStart.AddListener(ChangePool);

        foreach(EnemyType enemyType in _enemiesPrefabs)
        {
            List<EnemyController> enemies = new List<EnemyController>();

            for (int i = 0; i < enemyType.PoolSize; i++)
            {
                EnemyController enemy = Instantiate(enemyType.Enemy, transform);

                enemy.Init(this);

                enemy.gameObject.SetActive(false);
                enemy.transform.position = transform.position;

                enemies.Add(enemy);
            }

            _enemiesPool.Add(enemies);
        }
    }

    void ReturnAllToPool()
    {
        foreach (List<EnemyController> enemies in _enemies)
            foreach(EnemyController enemy in enemies)
            {
                enemy.ReturnToPool();
            }
    }

    public void AddToPool(EnemyController enemy)
    {
        enemy.transform.position = transform.position;

        // _moveSystem.RemoveMoveable(enemy);
        /*
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
        
        _enemiesPool.Add(enemy);
        */
    }

    void ChangePool()
    {
        _isSpawning = !_isSpawning;

        if (_isSpawning) PoolEnemy();
    }

    void PoolEnemy()
    {
        if (!_isSpawning) return;



        WaitForPool();
    }
    async void WaitForPool()
    {
        await System.Threading.Tasks.Task.Delay(10);

        PoolEnemy();
    }
}
