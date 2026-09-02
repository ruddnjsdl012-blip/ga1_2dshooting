using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 총알 프리팹
    public GameObject BulletPrefab;

    // 총알 생성 위치
    public Transform FirePoint;

    // 총알 발사 쿨타임
    public float CoolTime = 0.5f;

    // true = 자동 공격
    // false = 수동 공격
    public bool AutoAttack = false;

    // 마지막으로 총알을 발사한 시간
    private float LastFireTime;


    private void Update()
    {
        // 1번을 누르면 자동/수동 공격 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AutoAttack = !AutoAttack;
        }


        // 자동 공격
        if (AutoAttack)
        {
            if (Time.time >= LastFireTime + CoolTime)
            {
                Shoot();
                LastFireTime = Time.time;
            }
        }


        // 수동 공격
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time >= LastFireTime + CoolTime)
                {
                    Shoot();
                    LastFireTime = Time.time;
                }
            }
        }
    }


    // 총알 발사
    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab);

        bullet.transform.position = FirePoint.position;
    }
}