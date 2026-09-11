using UnityEngine;

public class ItemPool : MonoBehaviour
{
    // =========================
    // 싱글톤
    // =========================

    private static ItemPool _instance = null;

    public static ItemPool Instance => _instance;


    // =========================
    // 아이템 프리팹
    // =========================

    [Header("회복 아이템")]
    [SerializeField] private Item _healItemPrefab;

    [Header("이동 속도 증가 아이템")]
    [SerializeField] private Item _moveSpeedUpItemPrefab;

    [Header("발사 속도 증가 아이템")]
    [SerializeField] private Item _fireRateUpItemPrefab;


    // =========================
    // 풀 사이즈
    // =========================

    [Header("각 아이템 풀 사이즈")]
    [SerializeField] private int _poolSize = 10;


    // =========================
    // 아이템 풀
    // =========================

    private Item[] _healPool;
    private Item[] _moveSpeedUpPool;
    private Item[] _fireRateUpPool;


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
        // 배열 생성
        // =========================

        _healPool = new Item[_poolSize];
        _moveSpeedUpPool = new Item[_poolSize];
        _fireRateUpPool = new Item[_poolSize];


        // =========================
        // 회복 아이템 생성
        // =========================

        for (int i = 0; i < _poolSize; i++)
        {
            Item item = Instantiate(
                _healItemPrefab,
                transform
            );

            item.gameObject.SetActive(false);

            _healPool[i] = item;
        }


        // =========================
        // 이동 속도 증가 아이템 생성
        // =========================

        for (int i = 0; i < _poolSize; i++)
        {
            Item item = Instantiate(
                _moveSpeedUpItemPrefab,
                transform
            );

            item.gameObject.SetActive(false);

            _moveSpeedUpPool[i] = item;
        }


        // =========================
        // 발사 속도 증가 아이템 생성
        // =========================

        for (int i = 0; i < _poolSize; i++)
        {
            Item item = Instantiate(
                _fireRateUpItemPrefab,
                transform
            );

            item.gameObject.SetActive(false);

            _fireRateUpPool[i] = item;
        }
    }


    // =========================
    // 아이템 가져오기
    // =========================

    public Item GetItem(ItemType type)
    {
        switch (type)
        {
            // =========================
            // 회복
            // =========================

            case ItemType.Heal:

                return GetItemFromPool(_healPool);


            // =========================
            // 이동 속도 증가
            // =========================

            case ItemType.MoveSeepUp:

                return GetItemFromPool(_moveSpeedUpPool);


            // =========================
            // 발사 속도 증가
            // =========================

            case ItemType.FireRateUp:

                return GetItemFromPool(_fireRateUpPool);


            // =========================
            // 예외
            // =========================

            default:

                Debug.LogWarning(
                    $"알 수 없는 ItemType입니다. : {type}"
                );

                return null;
        }
    }


    // =========================
    // 풀에서 비활성화된 아이템 찾기
    // =========================

    private Item GetItemFromPool(Item[] pool)
    {
        foreach (Item item in pool)
        {
            if (!item.gameObject.activeSelf)
            {
                return item;
            }
        }


        // 사용 가능한 아이템이 없는 경우
        Debug.LogWarning(
            "사용 가능한 아이템이 없습니다."
        );

        return null;
    }
}