using UnityEngine;

public class Player : MonoBehaviour
{
    // =========================
    // 체력
    // =========================

    [SerializeField] private int _health = 100;

    // 읽기 전용 프로퍼티
    public int Health => _health;


    // =========================
    // 사망 이펙트
    // =========================

    [SerializeField] private GameObject _deathEffect;


    // =========================
    // 피격 사운드
    // =========================

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;


    // =========================
    // 폭탄
    // =========================

    [SerializeField] private GameObject _playerBomb;

    // 폭탄 재사용 쿨타임
    private const float BombCooldown = 10f;

    // 현재 폭탄 쿨타임
    private float _bombCooldownTimer = 0f;


    // =========================
    // Update
    // =========================

    private void Update()
    {
        // 폭탄 쿨타임 감소
        if (_bombCooldownTimer > 0f)
        {
            _bombCooldownTimer -= Time.deltaTime;
        }

        // B키를 누르면 폭탄 사용
        if (Input.GetKeyDown(KeyCode.B))
        {
            UseBomb();
        }
    }


    // =========================
    // 체력 관련
    // =========================

    public int GetHealth()
    {
        return _health;
    }


    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning("대미지는 음수일 수 없습니다.");
            return;
        }

        _health -= damage;

        // 피격 사운드 재생
        if (_audioSource != null && _hitSound != null)
        {
            _audioSource.PlayOneShot(_hitSound);
        }

        if (_health <= 0)
        {
            // 플레이어가 죽은 위치에 폭발 이펙트 생성
            if (_deathEffect != null)
            {
                Instantiate(
                    _deathEffect,
                    transform.position,
                    Quaternion.identity
                );
            }

            // 플레이어 삭제
            Destroy(gameObject);
        }
    }


    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        {
            Debug.LogWarning("힐량은 음수일 수 없습니다.");
            return;
        }

        _health += healAmount;
    }


    // =========================
    // 폭탄 사용
    // =========================

    private void UseBomb()
    {
        // 쿨타임 중이면 사용하지 않음
        if (_bombCooldownTimer > 0f)
        {
            Debug.Log(
                $"폭탄 쿨타임 중입니다. {_bombCooldownTimer:F1}초 남음"
            );

            return;
        }


        // 폭탄 프리팹이 연결되어 있는지 확인
        if (_playerBomb == null)
        {
            Debug.LogWarning("PlayerBomb 프리팹이 연결되지 않았습니다.");
            return;
        }


        // =========================
        // 화면 중앙 위치 계산
        // =========================

        Vector3 screenCenter = new Vector3(
            0.5f,
            0.5f,
            Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
        );

        Vector3 bombPosition =
            Camera.main.ViewportToWorldPoint(screenCenter);


        // 폭탄 생성
        Instantiate(
            _playerBomb,
            bombPosition,
            Quaternion.identity
        );


        // 폭탄 쿨타임 시작
        _bombCooldownTimer = BombCooldown;

        Debug.Log("화면 중앙에 폭탄을 설치했습니다.");
    }
}