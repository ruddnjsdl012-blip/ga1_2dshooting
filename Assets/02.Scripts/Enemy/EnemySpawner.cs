using UnityEngine;

// 역할: 일정 시간마다 적을 생성해준다.
public class EnemySpawner : MonoBehaviour
{
    // =========================
    // 스폰 설정
    // =========================

    [SerializeField] private float _spawnInterval = 3f;

    private float _timer;

    // 생성할 적과 각 적의 가중치 데이터
    [SerializeField] private EnemySpawnDataTableSO _spawnDataTable;

    // 현재 적을 생성할 수 있는지 여부
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

            // 다음 적이 생성되는 시간을 랜덤하게 설정
            _spawnInterval = Random.Range(1f, 3f);

            Spawn();
        }
    }


    // =========================
    // 적 스폰
    // =========================

    private void Spawn()
    {
        // 1. 모든 적의 가중치를 더한다.

        int totalWeight = 0;

        foreach (EnemySpawnData data in _spawnDataTable.Datas)
        {
            totalWeight += data.Weight;
        }


        // 2. 전체 가중치 범위에서
        // 랜덤한 숫자를 하나 뽑는다.

        int randomWeight = Random.Range(0, totalWeight);


        // 3. 가중치를 누적하면서
        // 선택된 구간을 찾는다.

        int cumulativeWeight = 0;

        foreach (EnemySpawnData data in _spawnDataTable.Datas)
        {
            cumulativeWeight += data.Weight;

            if (randomWeight < cumulativeWeight)
            {
                // 선택된 적을 생성한다.
                GameObject enemy = Instantiate(
                    data.EnemyPrefab,
                    transform.position,
                    Quaternion.identity
                );

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

        // 기존 타이머도 초기화한다.
        _timer = 0f;
    }


    // =========================
    // 스폰 재개
    // =========================

    public void StartSpawn()
    {
        _canSpawn = true;

        // 스폰 재개 후 바로 생성되지 않도록 타이머 초기화
        _timer = 0f;
    }
}