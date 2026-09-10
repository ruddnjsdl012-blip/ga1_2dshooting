using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    private GameObject _target = null;


    private void Update()
    {
        if (_target == null)
        {
            FindNearestTarget();
        }

        Move();
    }

    private void Move()
    {
        if (_target == null) return;

        // 2. 방향을 구한다.
        Vector3 direction = _target.transform.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        // 3. 속도에 맞게 이동을한다.
        transform.position += direction * _speed * Time.deltaTime;
    }

    private void FindNearestTarget()
    {
        // 1. 타겟을 구한다.
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length == 0) return;

        _target = targets[0];
        float minDistance = float.MaxValue;

        // 1-1. 가장 가까운 타겟을 찾는다.
        foreach (GameObject enemy in targets)
        {
            // 거리를 구해서
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance) // 저장된 거리보다 짧다면
            {
                // 타겟 변경
                minDistance = distance;
                _target = enemy;
            }
        }
    }
}