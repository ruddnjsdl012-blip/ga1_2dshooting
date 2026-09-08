using UnityEngine;

public class AimedEnemy : Enemy
{
    private GameObject _player;
    private Vector2 _direction;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player == null)
        {
            Debug.Log("플레이어 태그를 가진 게임 오브젝트를 찾지 못했습니다.");
            return;
        }

        _direction = _player.transform.position - transform.position;

        float dx = _direction.x; //플레이어와 에너미 사이의 밑변      
        float dy = _direction.y; //플레이어와 에너미 사이의 높이 
        // tan0 = dy / dx
        // tan^ * tan = tan^ * dy / dx
        // 0 = tant^ * dy  / dx
        //각도 = 세타 * Rad2Deg
        float seta = Mathf.Atan2(dy, dx);
        float angle = seta * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        _direction.Normalize();
    }

    protected override void Move()
    {
        if (_player == null) return;

        //  방향과 속도에 맞게 이동한다.
        transform.Translate(_direction * _moveSpeed * Time.deltaTime);
    }
}