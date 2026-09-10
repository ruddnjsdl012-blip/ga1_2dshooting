using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // =========================================================
    // 이동
    // =========================================================

    [Header("이동")] [SerializeField] private float _speed = 5f;

    public float Speed => _speed;


    // =========================================================
    // 이동 범위
    // =========================================================

    [Header("이동 제한")] public float MaxPositionY;
    public float MinPositionY;
    public float MaxPositionX;
    public float MinPositionX;


    // =========================================================
    // 애니메이터
    // =========================================================

    [Header("애니메이션")] [SerializeField] private Animator _animator;


    // =========================================================
    // 자동 이동
    // =========================================================

    [Header("자동 이동")] [SerializeField] private bool _autoMove = true;

    // 랜덤 방향을 얼마나 자주 바꿀지
    [SerializeField] private float _changeDirectionTime = 2f;

    // 현재 랜덤 방향
    private Vector2 _randomDirection;

    // 방향 변경 타이머
    private float _directionTimer;


    // =========================================================
    // 적 회피
    // =========================================================

    [Header("적 회피")] [SerializeField] private LayerMask _enemyLayer;

    // 적을 감지하는 거리
    [SerializeField] private float _detectDistance = 5f;

    // 이 거리 안에 들어오면 회피
    [SerializeField] private float _avoidDistance = 3f;

    // 적을 피하는 힘
    [SerializeField] private float _avoidStrength = 3f;


    // =========================================================
    // Unity
    // =========================================================

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // 처음 이동 방향 설정
        SetRandomDirection();
    }

    private void Update()
    {
        if (_autoMove)
        {
            AutoMove();
        }
        else
        {
            Move();
        }

        SpeedChange();
    }


    // =========================================================
    // 자동 이동
    // =========================================================

    private void AutoMove()
    {
        // -----------------------------------------
        // 1. 랜덤 방향 변경
        // -----------------------------------------

        _directionTimer -= Time.deltaTime;

        if (_directionTimer <= 0f)
        {
            SetRandomDirection();
        }


        // -----------------------------------------
        // 2. 현재 랜덤 방향
        // -----------------------------------------

        Vector2 moveDirection = _randomDirection;


        // -----------------------------------------
        // 3. 적 회피 방향 계산
        // -----------------------------------------

        Vector2 avoidDirection = GetAvoidDirection();


        // -----------------------------------------
        // 4. 적이 있으면 회피
        // -----------------------------------------

        if (avoidDirection != Vector2.zero)
        {
            moveDirection += avoidDirection * _avoidStrength;
        }


        // -----------------------------------------
        // 5. 방향 정규화
        // -----------------------------------------

        moveDirection.Normalize();


        // -----------------------------------------
        // 6. 애니메이션
        // -----------------------------------------

        _animator.SetInteger(
            "X",
            Mathf.RoundToInt(moveDirection.x)
        );


        // -----------------------------------------
        // 7. 실제 이동
        // -----------------------------------------

        Vector2 newPosition =
            (Vector2)transform.position +
            moveDirection * _speed * Time.deltaTime;


        // -----------------------------------------
        // 8. 이동 범위 제한
        // -----------------------------------------

        newPosition = ClampPosition(newPosition);


        // -----------------------------------------
        // 9. 위치 적용
        // -----------------------------------------

        transform.position = newPosition;
    }


    // =========================================================
    // 랜덤 방향 설정
    // =========================================================

    private void SetRandomDirection()
    {
        // X와 Y를 랜덤하게 만든다.
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        _randomDirection = new Vector2(x, y).normalized;

        // 타이머 초기화
        _directionTimer = _changeDirectionTime;
    }


    // =========================================================
    // 적 회피
    // =========================================================

    private Vector2 GetAvoidDirection()
    {
        // 주변 적 탐색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            _detectDistance,
            _enemyLayer
        );


        // 적이 없으면 회피하지 않는다.
        if (enemies.Length == 0)
        {
            return Vector2.zero;
        }


        // 가장 가까운 적
        Collider2D closestEnemy = null;

        float closestDistance = Mathf.Infinity;


        // 모든 적 검사
        foreach (Collider2D enemy in enemies)
        {
            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );


            // 가장 가까운 적 저장
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }


        // 가장 가까운 적이 없다.
        if (closestEnemy == null)
        {
            return Vector2.zero;
        }


        // 아직 회피 거리 밖이면 회피하지 않는다.
        if (closestDistance > _avoidDistance)
        {
            return Vector2.zero;
        }


        // -----------------------------------------
        // 적과 플레이어 사이의 방향
        // -----------------------------------------

        Vector2 directionToEnemy =
            closestEnemy.transform.position -
            transform.position;


        // 적의 반대 방향
        Vector2 avoidDirection =
            -directionToEnemy.normalized;


        return avoidDirection;
    }


    // =========================================================
    // 수동 이동
    // =========================================================

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 normalizedDirection =
            new Vector2(h, v).normalized;


        _animator.SetInteger(
            "X",
            Mathf.RoundToInt(normalizedDirection.x)
        );


        Vector2 newPosition =
            (Vector2)transform.position +
            normalizedDirection *
            _speed *
            Time.deltaTime;


        newPosition = ClampPosition(newPosition);

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
    // 속도
    // =========================================================

    public float Getspeed()
    {
        return _speed;
    }


    public void SpeedUp(float upValue)
    {
        if (upValue < 0)
        {
            Debug.LogWarning("속도 증가량은 0보다 작을 수 없습니다.");
            return;
        }

        _speed += upValue;
    }


    // =========================================================
    // Q / E 속도 조절
    // =========================================================

    private void SpeedChange()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _speed++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _speed--;

            if (_speed < 0)
            {
                _speed = 0;
            }
        }
    }


    // =========================================================
    // 자동 이동 ON / OFF
    // =========================================================

    public void SetAutoMove(bool value)
    {
        _autoMove = value;
    }


    // =========================================================
    // Scene 화면에서 회피 범위 확인
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        // 적 감지 범위
        Gizmos.DrawWireSphere(
            transform.position,
            _detectDistance
        );


        // 실제 회피 범위
        Gizmos.DrawWireSphere(
            transform.position,
            _avoidDistance
        );
    }
}