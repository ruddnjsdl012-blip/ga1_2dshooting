using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _damage;

    // 생성할 아이템 프리팹들
    [SerializeField] private Item[] _itemPrefabs;

    // 죽을 때 생성할 이펙트 프리팹
    [SerializeField] private GameObject _deathEffectPrefab;

    // Animator
    private Animator _animator;
    // 에너미가 공격 당할 떄 재생시켜주는 피격 사운드 
    private AudioSource _damagedAudioSource;
    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _damagedAudioSource = GetComponent<AudioSource>();

        if (_animator == null)
        {
            Debug.LogError($"{gameObject.name}에 Animator가 없습니다.");
        }
    }


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
        _health -= damage;

        // 죽지 않았다면 피격 애니메이션 실행
        if (_health > 0)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Hit");
            }
            _damagedAudioSource.Play();
            return;
        }


        // =========================
        // 사망
        // =========================

        // 아이템 생성
        SpawnItem();

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
        if (_itemPrefabs == null || _itemPrefabs.Length == 0)
        {
            Debug.LogWarning("생성할 아이템 프리팹이 없습니다.");
            return;
        }


        // 랜덤 아이템 선택
        int randomIndex = Random.Range(0, _itemPrefabs.Length);


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


        Player player = other.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogWarning("플레이어가 null입니다.");
            return;
        }


        // 플레이어에게 데미지
        player.TakeDamage(_damage);

        // 적 제거
        Destroy(gameObject);
    }
}