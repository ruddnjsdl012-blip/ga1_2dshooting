using UnityEngine;

public class Item : MonoBehaviour
{
    public string Type;
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
            case "heal":
            {
                player.TakeDamage((int)(Value * -1));
                //Debug.Log($"플레이어 체력: {player}");
                break;
            }

            case "moveSpeedUp":
            {
                player.GetComponent<PlayerMove>().Speed += Value;
                Debug.Log($"플레이어 이속: {player.GetComponent<PlayerMove>().Speed}");
                break;
            }

            case "fireRateUp":
            {
                player.GetComponent<PlayerFire>().CoolTime -= Value;
                Debug.Log($"플레이어 공속: {player.GetComponent<PlayerFire>().CoolTime}");
                break;
            }
        }

        Destroy(gameObject);
    }
}