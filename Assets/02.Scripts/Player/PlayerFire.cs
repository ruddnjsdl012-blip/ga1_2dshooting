using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    public float CoolTime = 0.5f;
    public bool AutoAttack = false;

    private float LastFireTime = -1f;

    private void Update()
    {
        // 1번으로 자동/수동 전환
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
            }
        }

        // 수동 공격
        if (!AutoAttack && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            BulletPrefab,
            FirePoint.position,
            FirePoint.rotation
        );

        Debug.Log("총알 생성됨 : " + bullet.name);

        LastFireTime = Time.time;
    }
}