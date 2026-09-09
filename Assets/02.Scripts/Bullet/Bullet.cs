using UnityEngine;

public class Bullet : MonoBehaviour
{
    private AudioSource _audioSource;
    public float Speed = 10f;
    public int Damage = 5;

    private void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.pitch = UnityEngine.Random.Range(-0.8f, 3f);
        _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 적 컴포넌트 찾기
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<Enemy>();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }

        // 어떤 것과 충돌하든 총알 삭제
        Destroy(gameObject);
    }
}