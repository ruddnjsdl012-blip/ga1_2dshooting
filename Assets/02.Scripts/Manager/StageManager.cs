using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;

    public void StartStageTransition()
    {
        // 새로운 적 스폰 중지
        if (_enemySpawner != null)
        {
            _enemySpawner.StopSpawn();
        }

        // 현재 존재하는 모든 Enemy 찾기
        Enemy[] enemies = FindObjectsByType<Enemy>(
            FindObjectsSortMode.None
        );

        // 모든 Enemy 제거
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.StageClearDestroy();
            }
        }

        Debug.Log("스테이지 전환 시작");
    }
}