using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // =========================
    // 체력
    // =========================

    [SerializeField] private int _health = 100;

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

    [SerializeField] private BombEffectUI _bombEffectUI;


    // =========================
    // 폭탄 쿨타임
    // =========================

    private const float BombCooldown = 10f;

    private float _bombCooldownTimer = 0f;


    // =========================
    // 스테이지 전환
    // =========================

    [SerializeField] private float _stageTransitionSpeed = 5f;

    private Vector3 _normalPosition;

    private bool _isStageTransitioning = false;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        _normalPosition = transform.position;
    }


    // =========================
    // Update
    // =========================

    private void Update()
    {
        // 스테이지 전환 중이라면
        // 폭탄 사용을 막는다.
        if (_isStageTransitioning)
        {
            return;
        }


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
    // 체력
    // =========================

    public int GetHealth()
    {
        return _health;
    }


    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning(
                "대미지는 음수일 수 없습니다."
            );

            return;
        }


        _health -= damage;


        // =========================
        // 피격 사운드
        // =========================

        if (_audioSource != null && _hitSound != null)
        {
            _audioSource.PlayOneShot(_hitSound);
        }


        // =========================
        // 사망
        // =========================

        if (_health <= 0)
        {
            if (_deathEffect != null)
            {
                Instantiate(
                    _deathEffect,
                    transform.position,
                    Quaternion.identity
                );
            }

            Destroy(gameObject);
        }
    }


    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        {
            Debug.LogWarning(
                "힐량은 음수일 수 없습니다."
            );

            return;
        }


        _health += healAmount;
    }


    // =========================
    // 스테이지 전환
    // =========================
    // 플레이어를 화면 위쪽으로 이동시킨다.
    // =========================

    public IEnumerator MoveOutForStageTransition()
    {
        _isStageTransitioning = true;


        // =========================
        // Main Camera 확인
        // =========================

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            _isStageTransitioning = false;

            yield break;
        }


        // =========================
        // 화면 위쪽 위치
        // =========================

        Vector3 topPosition =
            mainCamera.ViewportToWorldPoint(
                new Vector3(
                    0.5f,
                    1.2f,
                    Mathf.Abs(
                        mainCamera.transform.position.z
                        - transform.position.z
                    )
                )
            );


        // X 위치는 현재 플레이어 위치 유지
        topPosition.x =
            transform.position.x;


        // =========================
        // 플레이어 위로 이동
        // =========================

        yield return MoveToPosition(
            topPosition
        );
    }


    // =========================
    // 스테이지 전환
    // =========================
    // 플레이어를 화면 아래에서 등장시킨다.
    // =========================

    public IEnumerator MoveInForStageTransition()
    {
        // =========================
        // Main Camera 확인
        // =========================

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            _isStageTransitioning = false;

            yield break;
        }


        // =========================
        // 화면 아래쪽 위치
        // =========================

        Vector3 bottomPosition =
            mainCamera.ViewportToWorldPoint(
                new Vector3(
                    0.5f,
                    -0.2f,
                    Mathf.Abs(
                        mainCamera.transform.position.z
                        - transform.position.z
                    )
                )
            );


        // =========================
        // X 위치
        // =========================

        bottomPosition.x =
            _normalPosition.x;


        // =========================
        // 화면 아래로 이동
        // =========================

        transform.position =
            bottomPosition;


        // =========================
        // 원래 위치까지 이동
        // =========================

        yield return MoveToPosition(
            _normalPosition
        );


        // =========================
        // 스테이지 전환 종료
        // =========================

        _isStageTransitioning = false;
    }


    // =========================
    // 특정 위치까지 이동
    // =========================

    private IEnumerator MoveToPosition(
        Vector3 targetPosition
    )
    {
        while (
            Vector3.Distance(
                transform.position,
                targetPosition
            ) > 0.05f
        )
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _stageTransitionSpeed
                    * Time.deltaTime
                );


            yield return null;
        }


        // 오차 보정
        transform.position =
            targetPosition;
    }


    // =========================
    // 폭탄 사용
    // =========================

    private void UseBomb()
    {
        // =========================
        // 쿨타임 확인
        // =========================

        if (_bombCooldownTimer > 0f)
        {
            Debug.Log(
                $"폭탄 쿨타임 중입니다. " +
                $"{_bombCooldownTimer:F1}초 남음"
            );

            return;
        }


        // =========================
        // 폭탄 프리팹 확인
        // =========================

        if (_playerBomb == null)
        {
            Debug.LogWarning(
                "PlayerBomb 프리팹이 연결되지 않았습니다."
            );

            return;
        }


        // =========================
        // 화면 중앙 위치
        // =========================

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            return;
        }


        Vector3 screenCenter = new Vector3(
            0.5f,
            0.5f,
            Mathf.Abs(
                mainCamera.transform.position.z
                - transform.position.z
            )
        );


        Vector3 bombPosition =
            mainCamera.ViewportToWorldPoint(
                screenCenter
            );


        // =========================
        // 폭탄 생성
        // =========================

        Instantiate(
            _playerBomb,
            bombPosition,
            Quaternion.identity
        );


        // =========================
        // 폭탄 UI 실행
        // =========================

        if (_bombEffectUI != null)
        {
            _bombEffectUI.Play();
        }
        else
        {
            Debug.LogError(
                "Player의 Bomb Effect UI가 연결되지 않았습니다."
            );
        }


        // =========================
        // 쿨타임 시작
        // =========================

        _bombCooldownTimer =
            BombCooldown;


        Debug.Log(
            "B키 → 폭탄 사용"
        );
    }
}