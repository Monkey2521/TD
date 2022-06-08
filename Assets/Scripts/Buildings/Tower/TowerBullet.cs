using UnityEngine;

public class TowerBullet : MonoBehaviour, IMoveable
{
    [SerializeField] Rigidbody _rigidbody;

    float _speed;
    int _damage;
    public float Speed => _speed;

    Transform _targetTransform;
    IDamageable _target;

    Tower _tower;

    public void Init(Tower tower, float speed, int damage)
    {
        _tower = tower; 
        _speed = speed;
        _damage = damage;
        gameObject.SetActive(false);
    }

    public void Upgrade(float speed, int damage)
    {
        _speed += speed;
        _damage += damage;
    }

    public void Attack(IDamageable target)
    {
        gameObject.SetActive(true);
        _targetTransform = target.GetTransform();
        _target = target;
    }

    public void ReturnToPool()
    {
        _target = null;
        gameObject.SetActive(false);

        _tower.AddToPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _targetTransform)
        {
            _target.TakeDamage(_damage);
            ReturnToPool();
        }
    }

    public void Move()
    {
        transform.LookAt(_targetTransform);
        Vector3 velocity = (_targetTransform.position - transform.position).normalized * Speed;
        _rigidbody.velocity = velocity; 
    }
}
