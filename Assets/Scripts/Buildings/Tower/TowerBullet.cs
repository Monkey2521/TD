using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    int _speed;
    int _damage;

    public void Init(int speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }
}
