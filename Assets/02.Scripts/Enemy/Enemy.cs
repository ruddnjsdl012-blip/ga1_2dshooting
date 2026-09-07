using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _damage;

    // - 생성할 아이템 프리팹들
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
            Destroy(gameObject);
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        if (Random.Range(0, 100) > 30) return;

        // Todo: Scriptable Object를 사용해서 리팩토링
        // 이유 1: 배열을 사용했지만 각 아이템이 어떤 프리팹인지 알수가 없음
        // 이유 2: 각 아이템 스폰 확률을 매직 넘버로 하드코딩해서 유지보수가 어렵
        Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)], transform.position, transform.rotation);
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