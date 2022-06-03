public interface IDamageable
{
    public int HP { get; }
    public int MaxHP { get; }

    public HealthBar Health { get; }

    public void TakeDamage(int damage);
}
