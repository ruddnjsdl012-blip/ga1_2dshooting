using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // =========================
    // 체력
    // =========================

    [SerializeField] private int _baseHealth = 100;

    private int _health;
    private int _maxHealth;


    // =========================
    // 기본 설정
    // =========================

    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _damage;
    [SerializeField] private int _score = 100;


    // =========================
    // 아이템 / 사망 이펙트
    // =========================

    [SerializeField] private Item[] _itemPrefabs;
    [SerializeField] private GameObject _deathEffectPrefab;


    // =========================
    // 애니메이터 / 피격 사운드
    // =========================

    private Animator _animator;
    private AudioSource _damagedAudioSource;


    // =========================
    // 피격 이미지
    // =========================

    [SerializeField] private Sprite _hitSprite;
    [SerializeField] private float _hitDuration = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Sprite _normalSprite;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        // 기본 체력을 최대 체력으로 사용
        _maxHealth = _baseHealth;

        // 처음 체력 설정
        _health = _baseHealth;


        _animator =
            GetComponent<Animator>();


        _damagedAudioSource =
            GetComponent<AudioSource>();


        _spriteRenderer =
            GetComponent<SpriteRenderer>();


        // 현재 일반 스프라이트 저장
        if (_spriteRenderer != null)
        {
            _normalSprite =
                _spriteRenderer.sprite;
        }


        // Animator가 없어도 게임이 멈추지 않도록 처리
        if (_animator == null)
        {
            Debug.LogWarning(
                $"{gameObject.name}에 Animator가 없습니다."
            );
        }
    }


    // =========================
    // 적 재사용 초기화
    // =========================

    public void ResetEnemy()
    {
        // 체력 복구
        _health = _maxHealth;


        // 실행 중인 피격 코루틴 정리
        StopAllCoroutines();


        // 일반 스프라이트로 복구
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite =
                _normalSprite;
        }
    }


    // =========================
    // 매 프레임
    // =========================

    private void Update()
    {
        Move();
    }


    // =========================
    // 체력 밸런스 적용
    // =========================

    public void SetHealthBalance(float multiplier)
    {
        // 잘못된 배율 방지
        if (multiplier < 1f)
        {
            multiplier = 1f;
        }


        // 기본 체력에 배율 적용
        _maxHealth =
            Mathf.RoundToInt(
                _baseHealth * multiplier
            );


        // 현재 체력도 최대 체력으로 설정
        _health = _maxHealth;
    }


    // =========================
    // 적 이동
    // =========================

    protected abstract void Move();


    // =========================
    // 데미지
    // =========================

    public void TakeDamage(int damage)
    {
        // ---------------------------------
        // 비활성화된 적은 데미지를 받지 않음
        // ---------------------------------

        if (!gameObject.activeInHierarchy)
        {
            return;
        }


        // ---------------------------------
        // 이미 죽은 적은 다시 처리하지 않음
        // ---------------------------------

        if (_health <= 0)
        {
            return;
        }


        // ---------------------------------
        // 음수 데미지 방지
        // ---------------------------------

        if (damage < 0)
        {
            Debug.LogWarning(
                "데미지는 음수일 수 없습니다."
            );

            return;
        }


        // ---------------------------------
        // 체력 감소
        // ---------------------------------

        _health -= damage;


        // =================================
        // 아직 살아있는 경우
        // =================================

        if (_health > 0)
        {
            // ---------------------------------
            // Animator가 정상적으로 연결되어 있을 때만
            // Hit 트리거 실행
            // ---------------------------------

            if (_animator != null &&
                _animator.runtimeAnimatorController != null)
            {
                _animator.SetTrigger("Hit");
            }


            // ---------------------------------
            // 피격 사운드
            // ---------------------------------

            if (_damagedAudioSource != null)
            {
                _damagedAudioSource.Play();
            }


            // ---------------------------------
            // 피격 이미지
            // ---------------------------------

            if (_spriteRenderer != null &&
                _hitSprite != null &&
                gameObject.activeInHierarchy)
            {
                StartCoroutine(
                    HitFlash()
                );
            }

            return;
        }


        // =================================
        // 죽은 경우
        // =================================

        _health = 0;


        // ---------------------------------
        // 아이템 생성
        // ---------------------------------

        SpawnItem();


        // ---------------------------------
        // 점수 추가
        // ---------------------------------

        ScoreManager scoreManager =
            GameObject.FindObjectOfType<ScoreManager>();


        if (scoreManager != null)
        {
            scoreManager.AddScore(
                _score
            );
        }
        else
        {
            Debug.LogWarning(
                "ScoreManager를 찾을 수 없습니다."
            );
        }


        // ---------------------------------
        // 사망 이펙트 생성
        // ---------------------------------

        if (_deathEffectPrefab != null)
        {
            Instantiate(
                _deathEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }


        // ---------------------------------
        // 적 비활성화
        // ---------------------------------

        gameObject.SetActive(false);
    }


    // =========================
    // 스테이지 클리어용 적 제거
    // =========================

    public void StageClearDestroy()
    {
        // 이미 비활성화된 적이면 종료
        if (!gameObject.activeInHierarchy)
        {
            return;
        }


        // ---------------------------------
        // 사망 이펙트
        // ---------------------------------

        if (_deathEffectPrefab != null)
        {
            Instantiate(
                _deathEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }


        // ---------------------------------
        // 적 비활성화
        // ---------------------------------

        gameObject.SetActive(false);
    }


    // =========================
    // 피격 이미지
    // =========================

    private IEnumerator HitFlash()
    {
        // ---------------------------------
        // 코루틴 시작 시 비활성화되어 있으면 종료
        // ---------------------------------

        if (!gameObject.activeInHierarchy)
        {
            yield break;
        }


        // ---------------------------------
        // 흰색 피격 이미지
        // ---------------------------------

        _spriteRenderer.sprite =
            _hitSprite;


        // ---------------------------------
        // 잠시 대기
        // ---------------------------------

        yield return new WaitForSeconds(
            _hitDuration
        );


        // ---------------------------------
        // 대기하는 동안 적이 죽어서
        // 비활성화되었을 수도 있으므로 다시 확인
        // ---------------------------------

        if (!gameObject.activeInHierarchy)
        {
            yield break;
        }


        // ---------------------------------
        // 원래 이미지 복구
        // ---------------------------------

        _spriteRenderer.sprite =
            _normalSprite;
    }


    // =========================
    // 아이템 생성
    // =========================

    private void SpawnItem()
    {
        // ---------------------------------
        // 30% 확률
        // ---------------------------------

        if (Random.Range(0, 100) > 30)
        {
            return;
        }


        // ---------------------------------
        // 아이템 프리팹이 없는 경우
        // ---------------------------------

        if (_itemPrefabs == null ||
            _itemPrefabs.Length == 0)
        {
            Debug.LogWarning(
                "생성할 아이템 프리팹이 없습니다."
            );

            return;
        }


        // ---------------------------------
        // 랜덤 아이템 선택
        // ---------------------------------

        int randomIndex =
            Random.Range(
                0,
                _itemPrefabs.Length
            );


        // ---------------------------------
        // 아이템 생성
        // ---------------------------------

        Instantiate(
            _itemPrefabs[randomIndex],
            transform.position,
            transform.rotation
        );
    }


    // =========================
    // 플레이어와 충돌
    // =========================

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        // ---------------------------------
        // 이미 비활성화된 적이면 종료
        // ---------------------------------

        if (!gameObject.activeInHierarchy)
        {
            return;
        }


        // ---------------------------------
        // Player 태그가 아니면 무시
        // ---------------------------------

        if (!other.CompareTag("Player"))
        {
            return;
        }


        // ---------------------------------
        // Player 가져오기
        // ---------------------------------

        Player player =
            other.GetComponent<Player>();


        if (player == null)
        {
            Debug.LogWarning(
                "플레이어가 null입니다."
            );

            return;
        }


        // ---------------------------------
        // 플레이어에게 데미지
        // ---------------------------------

        player.TakeDamage(
            _damage
        );


        // ---------------------------------
        // 적 비활성화
        // ---------------------------------

        gameObject.SetActive(false);
    }
}