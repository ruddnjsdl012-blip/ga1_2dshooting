using UnityEngine;

public class CapsuleBullet : MonoBehaviour

{
    public float Speed = 10f;
    public int Damage = 5;

    private void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }

        // 충돌하면 총알은 무조건 삭제
        Destroy(gameObject);
    }
}