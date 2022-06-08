using UnityEngine;

[System.Serializable]
public class EnemyType 
{
    [SerializeField] EnemyController _enemy;
    public EnemyController Enemy => _enemy;

    [SerializeField] int _poolSize;
    public int PoolSize => _poolSize;

    public EnemyType(EnemyController enemy)
    {
        _enemy = enemy;
    }
}

public enum EnemyTypes
{
    Normal,
    Boss
};