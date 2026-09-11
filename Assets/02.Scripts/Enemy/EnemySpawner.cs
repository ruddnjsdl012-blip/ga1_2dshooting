using UnityEngine;

// 역할: 일정 시간마다 적을 생성해준다.
public class EnemySpawner : MonoBehaviour
{
    // =========================
    // 스폰 설정
    // =========================

    [SerializeField] private float _spawnInterval = 3f;

    private float _timer;


    // =========================
    // 적 생성 데이터
    // =========================

    [SerializeField]
    private EnemySpawnDataTableSO _spawnDataTable;


    // =========================
    // 스폰 가능 여부
    // =========================

    private bool _canSpawn = true;


    // =========================
    // Update
    // =========================

    private void Update()
    {
        // 스폰이 막혀 있다면 아무것도 하지 않는다.
        if (!_canSpawn)
        {
            return;
        }


        _timer += Time.deltaTime;


        if (_timer >= _spawnInterval)
        {
            _timer = 0f;


            // 다음 적 생성 시간을 랜덤하게 설정
            _spawnInterval =
                Random.Range(1f, 3f);


            Spawn();
        }
    }


    // =========================
    // 적 스폰
    // =========================

    private void Spawn()
    {
        // =========================
        // 데이터 확인
        // =========================

        if (_spawnDataTable == null)
        {
            Debug.LogWarning(
                "EnemySpawnDataTableSO가 연결되지 않았습니다."
            );

            return;
        }


        if (_spawnDataTable.Datas == null ||
            _spawnDataTable.Datas.Length == 0)
        {
            Debug.LogWarning(
                "EnemySpawnDataTableSO에 적 데이터가 없습니다."
            );

            return;
        }


        // =========================
        // 1. 전체 가중치 계산
        // =========================

        int totalWeight = 0;


        foreach (EnemySpawnData data
                 in _spawnDataTable.Datas)
        {
            totalWeight += data.Weight;
        }


        // 가중치가 잘못된 경우
        if (totalWeight <= 0)
        {
            Debug.LogWarning(
                "적의 가중치가 올바르지 않습니다."
            );

            return;
        }


        // =========================
        // 2. 랜덤 가중치 선택
        // =========================

        int randomWeight =
            Random.Range(
                0,
                totalWeight
            );


        // =========================
        // 3. 누적 가중치로 적 선택
        // =========================

        int cumulativeWeight = 0;


        foreach (EnemySpawnData data
                 in _spawnDataTable.Datas)
        {
            cumulativeWeight +=
                data.Weight;


            if (randomWeight < cumulativeWeight)
            {
                // =========================
                // 오브젝트 풀에서 적 가져오기
                // =========================

                GameObject enemy =
                    EnemyPool.Instance.GetEnemy(
                        data.EnemyPrefab
                    );


                // 사용 가능한 적이 없는 경우
                if (enemy == null)
                {
                    return;
                }


                // =========================
                // 위치 설정
                // =========================

                enemy.transform.position =
                    transform.position;


                // =========================
                // 회전 설정
                // =========================

                enemy.transform.rotation =
                    Quaternion.identity;


                // =========================
                // 적 활성화
                // =========================

                enemy.SetActive(true);


                break;
            }
        }
    }


    // =========================
    // 스폰 중지
    // =========================

    public void StopSpawn()
    {
        _canSpawn = false;

        // 기존 타이머 초기화
        _timer = 0f;
    }


    // =========================
    // 스폰 재개
    // =========================

    public void StartSpawn()
    {
        _canSpawn = true;

        // 스폰 재개 후 바로 생성되지 않도록 초기화
        _timer = 0f;
    }
}