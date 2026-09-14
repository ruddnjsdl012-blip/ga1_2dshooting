using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [Header("이동 속도")] [SerializeField] private float _minSpeed = 2.5f;
    [SerializeField] private float _maxSpeed = 5f;

    [Header("랜덤 목표 지점")] [SerializeField] private float _minTargetDistance = 2f;
    [SerializeField] private float _maxTargetDistance = 5f;

    [Header("목표 지점 도착")] [SerializeField] private float _targetReachDistance = 0.3f;

    [Header("방향 전환")] [SerializeField] private float _directionSmooth = 3f;

    [Header("화면 범위")] [SerializeField] private float _screenMargin = 0.7f;

    [Header("Y축 이동 제한")] [Range(0.5f, 1f)] [SerializeField]
    private float _maxHeightRatio = 0.7f;

    [Header("적 자동 회피")] [SerializeField] private float _enemyDetectRadius = 3f;
    [SerializeField] private float _enemyAvoidForce = 6f;
    [SerializeField] private float _enemyDangerDistance = 1.5f;

    private Camera _mainCamera;

    private Vector2 _currentDirection;
    private Vector2 _targetPosition;

    private float _currentSpeed;
    private float _targetSpeed;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        CalculateScreenBounds();

        _currentSpeed = Random.Range(
            _minSpeed,
            _maxSpeed
        );

        _targetSpeed = Random.Range(
            _minSpeed,
            _maxSpeed
        );

        SetRandomTarget();

        _currentDirection =
            GetDirectionToTarget();
    }

    private void Update()
    {
        CalculateScreenBounds();

        MoveToRandomTarget();

        KeepInsideScreen();
    }

    private void MoveToRandomTarget()
    {
        Vector2 currentPosition =
            transform.position;

        Vector2 randomDirection =
            GetRandomMoveDirection();

        Vector2 avoidDirection =
            GetEnemyAvoidDirection();

        Vector2 finalDirection;

        // 적이 발견되면
        // 랜덤 이동보다 회피 방향을 우선
        if (avoidDirection != Vector2.zero)
        {
            finalDirection =
                randomDirection +
                avoidDirection;
        }
        else
        {
            finalDirection =
                randomDirection;
        }

        if (finalDirection.magnitude < 0.01f)
        {
            finalDirection =
                Vector2.right;
        }

        finalDirection.Normalize();

        _currentDirection =
            Vector2.Lerp(
                _currentDirection,
                finalDirection,
                _directionSmooth *
                Time.deltaTime
            );

        if (_currentDirection.magnitude > 0.01f)
        {
            _currentDirection.Normalize();
        }

        _currentSpeed =
            Mathf.Lerp(
                _currentSpeed,
                _targetSpeed,
                2f *
                Time.deltaTime
            );

        currentPosition +=
            _currentDirection *
            _currentSpeed *
            Time.deltaTime;

        transform.position =
            new Vector3(
                currentPosition.x,
                currentPosition.y,
                transform.position.z
            );

        // 현재 목표 지점에 도착했는지 확인
        float distanceToTarget =
            Vector2.Distance(
                currentPosition,
                _targetPosition
            );

        if (distanceToTarget <= _targetReachDistance)
        {
            SetRandomTarget();
        }
    }

    private Vector2 GetRandomMoveDirection()
    {
        Vector2 currentPosition =
            transform.position;

        Vector2 direction =
            _targetPosition -
            currentPosition;

        if (direction.magnitude < 0.01f)
        {
            SetRandomTarget();

            direction =
                _targetPosition -
                currentPosition;
        }

        if (direction.magnitude < 0.01f)
        {
            return Random.insideUnitCircle.normalized;
        }

        return direction.normalized;
    }

    private Vector2 GetEnemyAvoidDirection()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                _enemyDetectRadius
            );

        if (enemies == null ||
            enemies.Length == 0)
        {
            return Vector2.zero;
        }

        Vector2 avoidDirection =
            Vector2.zero;

        int enemyCount = 0;

        Vector2 playerPosition =
            transform.position;

        foreach (Collider2D collider in enemies)
        {
            Enemy enemy =
                collider.GetComponent<Enemy>();

            if (enemy == null)
            {
                enemy =
                    collider.GetComponentInParent<Enemy>();
            }

            if (enemy == null)
            {
                continue;
            }

            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            Vector2 enemyPosition =
                enemy.transform.position;

            Vector2 awayFromEnemy =
                playerPosition -
                enemyPosition;

            float distance =
                awayFromEnemy.magnitude;

            if (distance < 0.01f)
            {
                awayFromEnemy =
                    Random.insideUnitCircle.normalized;

                distance = 0.01f;
            }
            else
            {
                awayFromEnemy.Normalize();
            }

            // 가까운 적일수록 회피 힘을 강하게 적용
            float danger =
                1f -
                Mathf.Clamp01(
                    distance /
                    _enemyDetectRadius
                );

            float force =
                danger *
                _enemyAvoidForce;

            // 매우 가까운 적은 추가로 강하게 회피
            if (distance <= _enemyDangerDistance)
            {
                force *= 2f;
            }

            avoidDirection +=
                awayFromEnemy *
                force;

            enemyCount++;
        }

        if (enemyCount == 0)
        {
            return Vector2.zero;
        }

        if (avoidDirection.magnitude < 0.01f)
        {
            return Vector2.zero;
        }

        return avoidDirection;
    }

    private void SetRandomTarget()
    {
        Vector2 currentPosition =
            transform.position;

        float distance =
            Random.Range(
                _minTargetDistance,
                _maxTargetDistance
            );

        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        Vector2 target =
            currentPosition +
            randomDirection *
            distance;

        target.x =
            Mathf.Clamp(
                target.x,
                _minX,
                _maxX
            );

        target.y =
            Mathf.Clamp(
                target.y,
                _minY,
                _maxY
            );

        int tryCount = 0;

        while (
            Vector2.Distance(
                currentPosition,
                target
            ) < _minTargetDistance &&
            tryCount < 10
        )
        {
            target =
                new Vector2(
                    Random.Range(
                        _minX,
                        _maxX
                    ),
                    Random.Range(
                        _minY,
                        _maxY
                    )
                );

            tryCount++;
        }

        _targetPosition =
            target;

        _targetSpeed =
            Random.Range(
                _minSpeed,
                _maxSpeed
            );
    }

    private Vector2 GetDirectionToTarget()
    {
        Vector2 direction =
            _targetPosition -
            (Vector2)transform.position;

        if (direction.magnitude < 0.01f)
        {
            return Random.insideUnitCircle.normalized;
        }

        return direction.normalized;
    }

    private void CalculateScreenBounds()
    {
        if (_mainCamera == null)
        {
            return;
        }

        float cameraDistance =
            Mathf.Abs(
                transform.position.z -
                _mainCamera.transform.position.z
            );

        Vector3 bottomLeft =
            _mainCamera.ViewportToWorldPoint(
                new Vector3(
                    0f,
                    0f,
                    cameraDistance
                )
            );

        Vector3 topRight =
            _mainCamera.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    cameraDistance
                )
            );

        _minX =
            bottomLeft.x +
            _screenMargin;

        _maxX =
            topRight.x -
            _screenMargin;

        _minY =
            bottomLeft.y +
            _screenMargin;

        float screenHeight =
            topRight.y -
            bottomLeft.y;

        _maxY =
            bottomLeft.y +
            screenHeight *
            _maxHeightRatio -
            _screenMargin;
    }

    private void KeepInsideScreen()
    {
        Vector3 position =
            transform.position;

        bool changed = false;

        if (position.x < _minX)
        {
            position.x = _minX;
            changed = true;
        }

        if (position.x > _maxX)
        {
            position.x = _maxX;
            changed = true;
        }

        if (position.y < _minY)
        {
            position.y = _minY;
            changed = true;
        }

        if (position.y > _maxY)
        {
            position.y = _maxY;
            changed = true;
        }

        transform.position =
            position;

        if (changed)
        {
            SetRandomTarget();
        }
    }

    public void SetSpeed(float speed)
    {
        _minSpeed = speed;
        _maxSpeed = speed;

        _currentSpeed = speed;
        _targetSpeed = speed;
    }

    public float GetSpeed()
    {
        return _currentSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        if (_mainCamera == null)
        {
            return;
        }

        float cameraDistance =
            Mathf.Abs(
                transform.position.z -
                _mainCamera.transform.position.z
            );

        Vector3 bottomLeft =
            _mainCamera.ViewportToWorldPoint(
                new Vector3(
                    0f,
                    0f,
                    cameraDistance
                )
            );

        Vector3 topRight =
            _mainCamera.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    cameraDistance
                )
            );

        float screenHeight =
            topRight.y -
            bottomLeft.y;

        float maxY =
            bottomLeft.y +
            screenHeight *
            _maxHeightRatio;

        // Y축 최대 이동 위치
        Gizmos.DrawLine(
            new Vector3(
                bottomLeft.x,
                maxY,
                transform.position.z
            ),
            new Vector3(
                topRight.x,
                maxY,
                transform.position.z
            )
        );

        // 적 감지 범위
        Gizmos.DrawWireSphere(
            transform.position,
            _enemyDetectRadius
        );
    }
}