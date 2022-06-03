using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    [SerializeField][Range(5, 30)] int _damage;
    public int Damage => _damage;

    [SerializeField][Range(0.5f, 5f)] float _speed;
    public float Speed => _speed;
}
