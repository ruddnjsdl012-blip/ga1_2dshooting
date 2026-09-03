using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] protected float _moveSpeed;

    private void Update()
    {
        Move();
    }

    protected abstract void Move();


    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}