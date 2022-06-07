using UnityEngine;

[System.Serializable]
public class EnemyType 
{
    [SerializeField] EnemyController _enemy;
    public EnemyController Enemy => _enemy;

    [SerializeField] EnemyTypes _enemyType;
    public EnemyTypes Type => _enemyType;

    [SerializeField] int _poolSize;
    public int PoolSize => _poolSize;
}

public enum EnemyTypes
{
    Normal,
    Boss
};