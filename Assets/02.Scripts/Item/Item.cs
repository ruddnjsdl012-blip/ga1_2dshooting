using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType Type;
    public float Value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("플레이어 태그 오브젝트에 플레이어 컴포넌트가 없습니다.");
            return;
        }

        switch (Type)
        {
            case ItemType.Heal:
            {
                player.Heal((int)(Value));
                break;
            }

            case ItemType.MoveSeepUp:
            {
                player.GetComponent<PlayerMove>().SpeedUp(Value);
                break;
            }

            case ItemType.FireRateUp:
            {
                // todo: 속성을 직접 수정하는게 아니라 메서드를 통한 수정
                player.GetComponent<PlayerFire>().CoolTime -= Value;
                Debug.Log($"플레이어 공속: {player.GetComponent<PlayerFire>().CoolTime}");
                break;
            }
        }

        Destroy(gameObject);
    }
}