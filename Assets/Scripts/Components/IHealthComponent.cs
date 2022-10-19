public interface IHealthComponent
{
    public void AlterHealth(float hpAmount);

    public void SetMaxHealth(float maxHealth);

    public float GetCurrentHealth();

    public bool CheckDeath();
}
