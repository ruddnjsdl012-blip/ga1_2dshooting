using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // =========================
    // 체력
    // =========================

    [SerializeField] private int _health = 100;

    // 적 이동 속도
    [SerializeField] protected float _moveSpeed;

    // 플레이어에게 주는 데미지
    [SerializeField] protected int _damage;

    // 적 처치 시 얻는 점수
    [SerializeField] private int _score = 100;


    // =========================
    // 아이템
    // =========================

    // 생성할 아이템 프리팹들
    [SerializeField] private Item[] _itemPrefabs;


    // =========================
    // 사망 이펙트
    // =========================

    [SerializeField] private GameObject _deathEffectPrefab;


    // =========================
    // Animator
    // =========================

    private Animator _animator;


    // =========================
    // 피격 사운드
    // =========================

    private AudioSource _damagedAudioSource;


    // =========================
    // 피격 이미지
    // =========================

    // 적이 맞았을 때 보여줄 하얀색 이미지
    [SerializeField] private Sprite _hitSprite;

    // 하얀색 이미지가 유지되는 시간
    [SerializeField] private float _hitDuration = 0.1f;

    private SpriteRenderer _spriteRenderer;

    // 원래 적 이미지
    private Sprite _normalSprite;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        // Animator 가져오기
        _animator = GetComponent<Animator>();

        // AudioSource 가져오기
        _damagedAudioSource = GetComponent<AudioSource>();

        // SpriteRenderer 가져오기
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 현재 Sprite 기억
        if (_spriteRenderer != null)
        {
            _normalSprite = _spriteRenderer.sprite;
        }

        // Animator가 없으면 경고
        if (_animator == null)
        {
            Debug.LogWarning(
                $"{gameObject.name}에 Animator가 없습니다."
            );
        }
    }


    // =========================
    // 이동
    // =========================

    private void Update()
    {
        Move();
    }

    protected abstract void Move();


    // =========================
    // 피격
    // =========================

    public void TakeDamage(int damage)
    {
        // 체력 감소
        _health -= damage;


        // =========================
        // 죽지 않았을 경우
        // =========================

        if (_health > 0)
        {
            // -------------------------
            // 피격 애니메이션
            // -------------------------

            if (_animator != null &&
                _animator.runtimeAnimatorController != null)
            {
                _animator.SetTrigger("Hit");
            }


            // -------------------------
            // 피격 사운드
            // -------------------------

            if (_damagedAudioSource != null)
            {
                _damagedAudioSource.Play();
            }


            // -------------------------
            // 피격 이미지
            // -------------------------

            if (_spriteRenderer != null &&
                _hitSprite != null)
            {
                StartCoroutine(HitFlash());
            }

            return;
        }


        // =========================
        // 사망
        // =========================

        // -------------------------
        // 아이템 생성
        // -------------------------

        SpawnItem();


        // -------------------------
        // 점수 추가
        // -------------------------

        ScoreManager scoreManager =
            GameObject.FindObjectOfType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.AddScore(_score);
        }
        else
        {
            Debug.LogWarning(
                "ScoreManager를 찾을 수 없습니다."
            );
        }


        // -------------------------
        // 사망 이펙트 생성
        // -------------------------

        if (_deathEffectPrefab != null)
        {
            Instantiate(
                _deathEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }


        // -------------------------
        // 적 제거
        // -------------------------

        Destroy(gameObject);
    }


    // =========================
    // 스테이지 전환 시 적 제거
    // =========================

    public void StageClearDestroy()
    {
        // 사망 이펙트 생성
        if (_deathEffectPrefab != null)
        {
            Instantiate(
                _deathEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // 적 제거
        Destroy(gameObject);
    }


    // =========================
    // 피격 이미지
    // =========================

    private IEnumerator HitFlash()
    {
        // 하얀색 이미지로 변경
        _spriteRenderer.sprite = _hitSprite;

        // 잠시 대기
        yield return new WaitForSeconds(_hitDuration);

        // 원래 이미지로 복구
        _spriteRenderer.sprite = _normalSprite;
    }


    // =========================
    // 아이템 생성
    // =========================

    private void SpawnItem()
    {
        // 30% 확률
        if (Random.Range(0, 100) > 30)
        {
            return;
        }


        // 아이템이 없으면 종료
        if (_itemPrefabs == null ||
            _itemPrefabs.Length == 0)
        {
            Debug.LogWarning(
                "생성할 아이템 프리팹이 없습니다."
            );

            return;
        }


        // 랜덤 아이템 선택
        int randomIndex =
            Random.Range(0, _itemPrefabs.Length);


        // 아이템 생성
        Instantiate(
            _itemPrefabs[randomIndex],
            transform.position,
            transform.rotation
        );
    }


    // =========================
    // 플레이어와 충돌
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player 태그가 아니면 무시
        if (!other.CompareTag("Player"))
        {
            return;
        }


        // Player 가져오기
        Player player =
            other.GetComponent<Player>();


        // Player가 없으면 종료
        if (player == null)
        {
            Debug.LogWarning(
                "플레이어가 null입니다."
            );

            return;
        }


        // 플레이어에게 데미지
        player.TakeDamage(_damage);


        // 적 제거
        Destroy(gameObject);
    }
}