using UnityEngine;

// 역할: 일정 시간마다 적을 생성해준다.
public class EnemySpawner : MonoBehaviour
{
    // =========================
    // 스폰 설정
    // =========================

    [SerializeField] private float _spawnInterval = 3f;

    private float _timer;

    // 생성할 적 프리팹들
    [SerializeField] private Enemy[] _enemyPrefabs;

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
        // 각 스포너가 적을 스폰할 때 확률에 따라
        // 다른 타입의 적을 생성한다.
        //
        // 50%: DownwardEnemy
        // 30%: AimedEnemy
        // 20%: HomingEnemy

        int enemyPrefabIndex = 0;

        int randomPercent = Random.Range(0, 100);

        if (randomPercent < 50)
        {
            enemyPrefabIndex = 0;
        }
        else if (randomPercent < 80)
        {
            enemyPrefabIndex = 1;
        }
        else
        {
            enemyPrefabIndex = 2;
        }

        Enemy enemy = Instantiate(
            _enemyPrefabs[enemyPrefabIndex]
        );

        enemy.transform.position = transform.position;
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