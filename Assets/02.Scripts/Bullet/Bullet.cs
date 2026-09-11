using UnityEngine;

public class Bullet : MonoBehaviour
{
    // =========================
    // 총알 설정
    // =========================

    [Header("총알 설정")]
    [SerializeField] private float _speed = 10f;

    [SerializeField] private int _damage = 5;


    // =========================
    // 사운드
    // =========================

    private AudioSource _audioSource;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    // =========================
    // 총알 활성화
    // =========================

    private void OnEnable()
    {
        Debug.Log("Bullet Enabled");

        PlaySound();
    }


    // =========================
    // 총알 이동
    // =========================

    private void Update()
    {
        transform.Translate(
            Vector2.up * _speed * Time.deltaTime
        );
    }


    // =========================
    // 총알 생성 시 호출
    // =========================

    public void OnSpawn()
    {
        PlaySound();
    }


    // =========================
    // 총알 사운드
    // =========================

    private void PlaySound()
    {
        if (_audioSource == null)
        {
            return;
        }

        // 사운드 재생 속도 설정
        _audioSource.pitch = Random.Range(0.8f, 1.2f);

        // 사운드 재생
        _audioSource.Play();
    }


    // =========================
    // 충돌
    // =========================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 적 컴포넌트 찾기
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();


        // 자식 오브젝트에 Collider2D가 있는 경우
        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<Enemy>();
        }


        // 적을 찾았다면 대미지
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }


        // 총알을 풀로 반환
        ReturnToPool();
    }


    // =========================
    // 총알 풀 반환
    // =========================

    private void ReturnToPool()
    {
        // 총알 비활성화
        gameObject.SetActive(false);
    }
}