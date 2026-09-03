using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;

    private void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy와 충돌했을 때만 총알 삭제
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}