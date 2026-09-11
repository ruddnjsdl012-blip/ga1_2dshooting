using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // =========================
    // 싱글톤
    // =========================

    private static EnemyPool _instance = null;

    public static EnemyPool Instance => _instance;


    // =========================
    // 적 프리팹
    // =========================

    [Header("적 프리팹")]
    [SerializeField] private GameObject[] _enemyPrefabs;


    // =========================
    // 적 종류별 풀 사이즈
    // =========================

    [Header("적 종류별 풀 사이즈")]
    [SerializeField] private int _poolSize = 10;


    // =========================
    // 적 풀
    // =========================

    private GameObject[][] _pools;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;


        // =========================
        // 적 프리팹 확인
        // =========================

        if (_enemyPrefabs == null ||
            _enemyPrefabs.Length == 0)
        {
            Debug.LogError(
                "EnemyPool: 적 프리팹이 등록되지 않았습니다."
            );

            return;
        }


        // =========================
        // 종류별 풀 배열 생성
        // =========================

        _pools =
            new GameObject[_enemyPrefabs.Length][];


        // =========================
        // 적 종류별 풀 생성
        // =========================

        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            // 현재 적 종류의 풀 생성
            _pools[i] =
                new GameObject[_poolSize];


            // 적 미리 생성
            for (int j = 0; j < _poolSize; j++)
            {
                GameObject enemy =
                    Instantiate(
                        _enemyPrefabs[i],
                        transform
                    );


                // 처음에는 비활성화
                enemy.SetActive(false);


                // 풀에 저장
                _pools[i][j] = enemy;
            }
        }
    }


    // =========================
    // 적 가져오기
    // =========================

    public GameObject GetEnemy(GameObject enemyPrefab)
    {
        // =========================
        // 프리팹 찾기
        // =========================

        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            if (_enemyPrefabs[i] != enemyPrefab)
            {
                continue;
            }


            // =========================
            // 비활성화된 적 찾기
            // =========================

            foreach (GameObject enemy in _pools[i])
            {
                if (!enemy.activeSelf)
                {
                    // Enemy 컴포넌트 가져오기
                    Enemy enemyComponent =
                        enemy.GetComponent<Enemy>();


                    // 체력 및 상태 초기화
                    if (enemyComponent != null)
                    {
                        enemyComponent.ResetEnemy();
                    }


                    return enemy;
                }
            }


            // =========================
            // 사용할 적이 없는 경우
            // =========================

            Debug.LogWarning(
                $"사용 가능한 {enemyPrefab.name} 적이 없습니다."
            );

            return null;
        }


        // =========================
        // 등록되지 않은 프리팹
        // =========================

        Debug.LogWarning(
            $"EnemyPool에 등록되지 않은 적 프리팹입니다. : {enemyPrefab.name}"
        );

        return null;
    }
}