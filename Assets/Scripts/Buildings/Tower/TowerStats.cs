using UnityEngine;

[System.Serializable]
public class TowerStats
{
    [SerializeField][Range(5, 30)] int _bulletDamage;
    public int BulletDamage => _bulletDamage;

    [SerializeField][Range(0.5f, 5f)] float _bulletSpeed;
    public float BulletSpeed => _bulletSpeed;

    [SerializeField][Range(0.1f, 5f)] float _attackTime;
    public float AttackTime => _attackTime;

    [SerializeField][Range(0.1f, 10f)] float _attackRange;
    public float AttackRange => _attackRange;

    public static TowerStats operator +(TowerStats first, TowerStats other)
    {
        TowerStats stats = new TowerStats();

        stats._bulletDamage = first._bulletDamage + other._bulletDamage;
        stats._bulletSpeed = first._bulletSpeed + other._bulletSpeed;
        stats._attackTime = first._attackTime + other._attackTime;

        return stats;
    }

    public static TowerStats operator *(TowerStats first, int multiplier)
    {
        TowerStats stats = new TowerStats();

        stats._bulletDamage = first._bulletDamage * multiplier;
        stats._bulletSpeed = first._bulletSpeed * multiplier;
        stats._attackTime = first._attackTime * multiplier;

        return stats;
    }
}
