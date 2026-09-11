using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] private GameObject _playerBomb;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _bombRadius = 3f;

    private void Start()
    {
        // 3초 후 폭발
        Invoke(nameof(Explode), _duration);
    }

    private void Explode()
    {
        // 폭발 범위 안의 모든 Collider 검색
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            _bombRadius
        );

        foreach (Collider2D collider in colliders)
        {
            // Collider가 Enemy 본체 또는 자식에 있어도 찾을 수 있도록 처리
            Enemy enemy = collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                Debug.Log($"폭탄으로 {enemy.name} 처치!");

                // 적을 한 방에 처치
                enemy.TakeDamage(999999);
            }
        }

        Debug.Log("폭탄 폭발!");

        // 폭탄 제거
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // 폭탄의 공격 범위 표시
        Gizmos.DrawWireSphere(
            transform.position,
            _bombRadius
        );
    }
}