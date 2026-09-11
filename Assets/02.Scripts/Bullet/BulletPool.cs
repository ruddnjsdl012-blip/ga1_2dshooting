using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // =========================
    // 싱글톤
    // =========================

    private static BulletPool _instance = null;

    public static BulletPool Instance => _instance;


    // =========================
    // 총알 프리팹
    // =========================

    [Header("총알 프리팹")] [SerializeField] private Bullet _bulletPrefab;


    // =========================
    // 풀 사이즈
    // =========================

    [Header("풀 사이즈")] [SerializeField] private int _poolSize = 50;


    // =========================
    // 총알 풀
    // =========================

    private Bullet[] _pool;


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


        // 총알 풀 생성
        _pool = new Bullet[_poolSize];


        // 총알 미리 생성
        for (int i = 0; i < _poolSize; i++)
        {
            Bullet bullet = Instantiate(
                _bulletPrefab,
                transform
            );

            // 처음에는 비활성화
            bullet.gameObject.SetActive(false);

            // 풀에 저장
            _pool[i] = bullet;
        }
    }


    // =========================
    // 총알 가져오기
    // =========================

    public Bullet getbullet()
    {
        foreach (Bullet bullet in _pool)
        {
            if (bullet.gameObject.activeSelf == false)
            {
                return bullet;
            }
        }

        // 사용 가능한 총알이 없으면 랜덤으로 하나 반환
        return _pool[Random.Range(0, _pool.Length)];
    }
}