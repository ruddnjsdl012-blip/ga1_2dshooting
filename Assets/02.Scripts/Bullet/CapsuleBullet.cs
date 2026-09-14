using UnityEngine;

public class CapsuleBullet : MonoBehaviour
{
    // =========================
    // 총알 설정
    // =========================

    [Header("총알 설정")]
    [SerializeField] private float _speed = 10f;

    [SerializeField] private int _damage = 5;


    // =========================
    // 총알 이동
    // =========================

    private void Update()
    {
        transform.Translate(
            Vector2.up * _speed * Time.deltaTime,
            Space.Self
        );
    }


    // =========================
    // 충돌
    // =========================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<Enemy>();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }

        // 보조총알은 기존 방식대로 삭제
        Destroy(gameObject);
    }
}