using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private float _value;

    private const float WaitTime = 2f;
    private float _waitTimer = 0f;
    private const float MoveSpeed = 5f;

    private Player _player = null;

    private void Start()
    {
        // 안전하게 플레이어 찾기
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.GetComponent<Player>();
        }

        if (_player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다. (태그 또는 Player 컴포넌트 확인 필요)");
        }
    }

    private void Update()
    {
        // 플레이어를 아직 못 찾았으면 계속 찾기 시도
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.GetComponent<Player>();
            }

            return;
        }

        _waitTimer += Time.deltaTime;
        if (_waitTimer >= WaitTime)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (_player == null) return;

        Vector2 direction = _player.transform.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * MoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 현재 오브젝트에서 찾고, 없으면 부모에서도 찾기
        Player player = other.GetComponent<Player>();
        if (player == null)
        {
            player = other.GetComponentInParent<Player>();
        }

        if (player == null)
        {
            Debug.LogWarning("플레이어 태그 오브젝트에 플레이어 컴포넌트가 없습니다.");
            return;
        }

        switch (_type)
        {
            case ItemType.Heal:
            {
                player.Heal((int)_value);
                Debug.Log("플레이어 체력: {player.health()}");
                break;
            }

            case ItemType.MoveSeepUp:
            {
                player.GetComponent<PlayerMove>().SpeedUp(_value);
                Debug.Log($"플레이어 이동소고:{player.GetComponent<PlayerMove>().Getspeed()}");
                break;
            }

            case ItemType.FireRateUp:
            {
                player.GetComponent<PlayerFire>().FireRateUp(_value);
                break;
            }
        }

        Destroy(gameObject);
    }
}