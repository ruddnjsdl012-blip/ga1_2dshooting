using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 10;

    private int CurrentHealth;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        Debug.Log("적 체력 : " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}