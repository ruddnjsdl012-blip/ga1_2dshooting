using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Item")) return;

        Item item = other.GetComponent<Item>();
        if (item == null)
        {
            Debug.LogWarning("아이템 태그 오브젝트에 아이템 컴포넌트가 없습니다.");
            return;
        }

        switch (item.Type)
        {
            case "heal":
            {
                _health += (int)item.Value;
                Debug.Log($"플레이어 체력: {_health}");
                break;
            }

            case "moveSpeedUp":
            {
                GetComponent<PlayerMove>().Speed += item.Value;
                Debug.Log($"플레이어 이속: {GetComponent<PlayerMove>().Speed}");
                break;
            }

            case "fireRateUp":
            {
                GetComponent<PlayerFire>().CoolTime -= item.Value;
                Debug.Log($"플레이어 공속: {GetComponent<PlayerFire>().CoolTime}");
                break;
            }
        }

        Destroy(other.gameObject);
    }
}