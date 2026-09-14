using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [Header("기본 이동")]
    [SerializeField] private float _speed = 3f;

    [Header("랜덤 이동")]
    [SerializeField] private float _minChangeTime = 1.5f;
    [SerializeField] private float _maxChangeTime = 3f;
    [SerializeField] private float _turnSpeed = 1.5f;

    [Header("화면 이동 범위")]
    [SerializeField] private float _minPositionX = -7f;
    [SerializeField] private float _maxPositionX = 7f;
    [SerializeField] private float _minPositionY = -4f;
    [SerializeField] private float _maxPositionY = 4f;

    [Header("화면 안쪽으로 돌아오는 힘")]
    [SerializeField] private float _boundaryStrength = 4f;

    [Header("적 회피")]
    [SerializeField] private float _avoidDistance = 2.5f;
    [SerializeField] private float _avoidStrength = 3f;
    [SerializeField] private LayerMask _enemyLayer;

    private Vector2 _currentDirection;
    private Vector2 _targetDirection;

    private float _directionTimer;

    private void OnEnable()
    {
        // AUTO를 켰을 때 현재 위치에서
        // 갑자기 Y축으로 튀어나가지 않도록
        // 처음에는 이동하지 않는다.

        _currentDirection = Vector2.zero;
        _targetDirection = Vector2.zero;

        _directionTimer = 0.5f;
    }

    private void Update()
    {
        UpdateRandomDirection();

        UpdateDirection();

        Move();
    }

    private void UpdateRandomDirection()
    {
        _directionTimer -= Time.deltaTime;

        if (_directionTimer > 0f)
        {
            return;
        }

        SetRandomDirection();
    }

    private void SetRandomDirection()
    {
        // 화면 안에서 자연스럽게 돌아다니도록
        // X와 Y를 모두 랜덤으로 만든다.

        float randomX =
            Random.Range(-1f, 1f);

        float randomY =
            Random.Range(-1f, 1f);

        _targetDirection =
            new Vector2(
                randomX,
                randomY
            );

        if (_targetDirection.magnitude < 0.3f)
        {
            _targetDirection =
                Random.insideUnitCircle.normalized;
        }

        _targetDirection.Normalize();

        _directionTimer =
            Random.Range(
                _minChangeTime,
                _maxChangeTime
            );
    }

    private void UpdateDirection()
    {
        _currentDirection =
            Vector2.Lerp(
                _currentDirection,
                _targetDirection,
                _turnSpeed * Time.deltaTime
            );

        if (_currentDirection.magnitude > 0.01f)
        {
            _currentDirection.Normalize();
        }
    }

    private void Move()
    {
        Vector2 moveDirection =
            _currentDirection;

        // 화면 가장자리에 가까워지면
        // 화면 중앙 방향으로 자연스럽게 돌아온다.

        Vector2 boundaryDirection =
            GetBoundaryDirection();

        moveDirection +=
            boundaryDirection *
            _boundaryStrength;

        // 적이 가까우면 피한다.

        Vector2 avoidDirection =
            GetAvoidDirection();

        moveDirection +=
            avoidDirection *
            _avoidStrength;

        if (moveDirection.magnitude > 0.01f)
        {
            moveDirection.Normalize();
        }

        Vector2 newPosition =
            (Vector2)transform.position +
            moveDirection *
            _speed *
            Time.deltaTime;

        // 최종적으로 화면 밖으로 절대 나가지 못하게 한다.

        newPosition.x =
            Mathf.Clamp(
                newPosition.x,
                _minPositionX,
                _maxPositionX
            );

        newPosition.y =
            Mathf.Clamp(
                newPosition.y,
                _minPositionY,
                _maxPositionY
            );

        transform.position =
            newPosition;
    }

    private Vector2 GetBoundaryDirection()
    {
        Vector2 direction =
            Vector2.zero;

        float margin = 1.5f;

        if (transform.position.x <
            _minPositionX + margin)
        {
            direction.x += 1f;
        }

        if (transform.position.x >
            _maxPositionX - margin)
        {
            direction.x -= 1f;
        }

        if (transform.position.y <
            _minPositionY + margin)
        {
            direction.y += 1f;
        }

        if (transform.position.y >
            _maxPositionY - margin)
        {
            direction.y -= 1f;
        }

        return direction;
    }

    private Vector2 GetAvoidDirection()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                _avoidDistance,
                _enemyLayer
            );

        if (enemies.Length == 0)
        {
            return Vector2.zero;
        }

        Vector2 direction =
            Vector2.zero;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            Vector2 away =
                (Vector2)transform.position -
                (Vector2)enemy.transform.position;

            float distance =
                away.magnitude;

            if (distance <= 0.01f)
            {
                continue;
            }

            float power =
                1f -
                Mathf.Clamp01(
                    distance /
                    _avoidDistance
                );

            direction +=
                away.normalized *
                power;
        }

        if (direction.magnitude <= 0.01f)
        {
            return Vector2.zero;
        }

        return direction.normalized;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            _avoidDistance
        );
    }
}