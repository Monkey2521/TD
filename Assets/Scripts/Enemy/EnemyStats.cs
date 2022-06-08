using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    [SerializeField][Range(1, 30)] int _damage;
    public int Damage => _damage;

    [SerializeField][Range(0.001f, 15f)] float _speed;
    public float Speed => _speed;
}
