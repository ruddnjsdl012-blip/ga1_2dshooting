using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _damage;

    // 생성할 아이템 프리팹들
    [SerializeField] private Item[] _itemPrefabs;

    private void Update()
    {
        Move();
    }

    protected abstract void Move();

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            SpawnItem();
            Destroy(gameObject);
            return;
        }

        // 피격 애니메이션 실행
    }

    private void SpawnItem()
    {
        if (Random.Range(0, 100) > 30) return;

        if (_itemPrefabs == null || _itemPrefabs.Length == 0)
        {
            Debug.LogWarning("생성할 아이템 프리팹이 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, _itemPrefabs.Length);

        Instantiate(
            _itemPrefabs[randomIndex],
            transform.position,
            transform.rotation
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogWarning("플레이어가 null입니다.");
            return;
        }

        player.TakeDamage(_damage);

        Destroy(gameObject);
    }
}