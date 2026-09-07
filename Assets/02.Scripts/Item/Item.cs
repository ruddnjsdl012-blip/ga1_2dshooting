using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private float _value;

    private const float WaitTime = 2f;
    private float _waitTime = 0f;
    private const float Needspeed = 3f;


    private void Update()
    {
        _waitTime += Time.deltaTime;
        if (_waitTime >= WaitTime)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (_waitTime >= WaitTime)
        {
            FollowPlayer();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("플레이어 태그 오브젝트에 플레이어 컴포넌트가 없습니다.");
            return;
        }

        switch (_type)
        {
            case ItemType.Heal:
            {
                player.Heal((int)(_value));
                break;
            }

            case ItemType.MoveSeepUp:
            {
                player.GetComponent<PlayerMove>().SpeedUp(_value);
                break;
            }

            case ItemType.FireRateUp:
            {
                // todo: 속성을 직접 수정하는게 아니라 메서드를 통한 수정
                player.GetComponent<PlayerFire>().CoolTime -= _value;
                Debug.Log($"플레이어 공속: {player.GetComponent<PlayerFire>().CoolTime}");
                break;
            }
        }

        Destroy(gameObject);
    }
}