using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // =========================================================
    // 이동
    // =========================================================

    [Header("이동")]
    [SerializeField] private float _speed = 5f;

    public float Speed => _speed;


    // =========================================================
    // Q / E 추가 속도
    // =========================================================

    private float _speedOffset = 0f;


    // =========================================================
    // 이동 범위
    // =========================================================

    [Header("이동 제한")]
    public float MaxPositionY;
    public float MinPositionY;
    public float MaxPositionX;
    public float MinPositionX;


    // =========================================================
    // 애니메이터
    // =========================================================

    [Header("애니메이션")]
    [SerializeField] private Animator _animator;


    // =========================================================
    // Unity
    // =========================================================

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 업그레이드 이동속도 적용
        ApplyMoveSpeedUpgrade();

        // 키보드 수동 이동
        Move();

        // Q / E 속도 조절
        SpeedChange();
    }


    // =========================================================
    // 업그레이드 이동속도 적용
    // =========================================================

    private void ApplyMoveSpeedUpgrade()
    {
        if (UpgradeManager.Instance == null)
        {
            return;
        }

        float upgradeSpeed =
            UpgradeManager.Instance.GetMoveSpeed();

        _speed =
            upgradeSpeed +
            _speedOffset;
    }


    // =========================================================
    // 수동 이동
    // =========================================================

    private void Move()
    {
        float h = SimpleInput.GetAxisRaw("Horizontal");
        float v = SimpleInput.GetAxisRaw("Vertical");

        Vector2 normalizedDirection =
            new Vector2(h, v).normalized;


        // -----------------------------------------
        // 애니메이션
        // -----------------------------------------

        if (_animator != null)
        {
            _animator.SetInteger(
                "X",
                Mathf.RoundToInt(normalizedDirection.x)
            );
        }


        // -----------------------------------------
        // 실제 이동
        // -----------------------------------------

        Vector2 newPosition =
            (Vector2)transform.position +
            normalizedDirection *
            _speed *
            Time.deltaTime;


        // -----------------------------------------
        // 이동 범위 제한
        // -----------------------------------------

        newPosition =
            ClampPosition(newPosition);

        transform.position = newPosition;
    }


    // =========================================================
    // 이동 범위
    // =========================================================

    private Vector2 ClampPosition(Vector2 position)
    {
        // Y 제한
        if (position.y > MaxPositionY)
        {
            position.y = MaxPositionY;
        }
        else if (position.y < MinPositionY)
        {
            position.y = MinPositionY;
        }


        // X 제한
        // 오른쪽 끝 → 왼쪽으로 이동
        if (position.x > MaxPositionX)
        {
            position.x = MinPositionX;
        }
        else if (position.x < MinPositionX)
        {
            position.x = MaxPositionX;
        }


        return position;
    }


    // =========================================================
    // 속도 확인
    // =========================================================

    public float Getspeed()
    {
        return _speed;
    }


    // =========================================================
    // 속도 증가
    // =========================================================

    public void SpeedUp(float upValue)
    {
        if (upValue < 0)
        {
            Debug.LogWarning(
                "속도 증가량은 0보다 작을 수 없습니다."
            );

            return;
        }

        _speedOffset += upValue;
    }


    // =========================================================
    // Q / E 속도 조절
    // =========================================================

    private void SpeedChange()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _speedOffset++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _speedOffset--;

            if (_speedOffset < -5f)
            {
                _speedOffset = -5f;
            }
        }
    }
}