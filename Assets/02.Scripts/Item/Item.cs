using UnityEngine;

public class Item : MonoBehaviour
{
    // =========================
    // 아이템 종류
    // =========================

    [SerializeField] private ItemType _type;


    // =========================
    // 아이템 효과 수치
    // =========================

    [SerializeField] private float _value;


    // =========================
    // 플레이어에게 이동하기 전 대기 시간
    // =========================

    private const float WaitTime = 2f;

    private float _waitTimer = 0f;


    // =========================
    // 플레이어를 따라가는 속도
    // =========================

    private const float MoveSpeed = 5f;


    // =========================
    // 플레이어
    // =========================

    private Player _player = null;


    // =========================
    // 오브젝트가 처음 생성될 때
    // =========================

    private void Awake()
    {
        _player = null;
    }


    // =========================
    // 오브젝트 풀에서 활성화될 때
    // =========================

    private void OnEnable()
    {
        // 대기 시간 초기화
        _waitTimer = 0f;


        // 플레이어 찾기
        if (_player == null)
        {
            GameObject playerObject =
                GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                _player =
                    playerObject.GetComponent<Player>();
            }
        }
    }


    // =========================
    // Update
    // =========================

    private void Update()
    {
        _waitTimer += Time.deltaTime;


        // 2초 동안 가만히 있음
        if (_waitTimer < WaitTime)
        {
            return;
        }


        // 2초가 지나면 플레이어를 따라감
        FollowPlayer();
    }


    // =========================
    // 플레이어 따라가기
    // =========================

    private void FollowPlayer()
    {
        if (_player == null)
        {
            return;
        }


        Vector3 direction =
            (
                _player.transform.position
                - transform.position
            ).normalized;


        transform.Translate(
            direction
            * MoveSpeed
            * Time.deltaTime
        );
    }


    // =========================
    // 플레이어와 충돌
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 아니면 무시
        if (!other.CompareTag("Player"))
        {
            return;
        }


        // Player 컴포넌트 가져오기
        Player player =
            other.GetComponent<Player>();


        if (player == null)
        {
            return;
        }


        // 아이템 효과 적용
        ApplyItemEffect(player);


        // =========================
        // 오브젝트 풀로 반환
        // =========================

        gameObject.SetActive(false);
    }


    // =========================
    // 아이템 효과 적용
    // =========================

    private void ApplyItemEffect(Player player)
    {
        switch (_type)
        {
            // =========================
            // 체력 회복
            // =========================

            case ItemType.Heal:

                player.Heal((int)_value);

                Debug.Log(
                    $"회복 아이템 획득! +{_value}"
                );

                break;


            // =========================
            // 이동 속도 증가
            // =========================

            case ItemType.MoveSeepUp:

                Debug.Log(
                    $"이동 속도 증가 아이템 획득! +{_value}"
                );

                // 나중에 Player의 이동 속도 증가 기능 연결

                break;


            // =========================
            // 발사 속도 증가
            // =========================

            case ItemType.FireRateUp:

                Debug.Log(
                    $"발사 속도 증가 아이템 획득! +{_value}"
                );

                // 나중에 PlayerFire의 발사 간격 감소 기능 연결

                break;


            // =========================
            // 예외
            // =========================

            default:

                Debug.LogWarning(
                    $"알 수 없는 ItemType입니다. : {_type}"
                );

                break;
        }
    }
}