using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bullet 컴포넌트가 있는지 확인
        Bullet bullet = other.GetComponent<Bullet>();

        if (bullet != null)
        {
            // 총알을 삭제하지 않고 비활성화
            bullet.gameObject.SetActive(false);
        }
    }
}