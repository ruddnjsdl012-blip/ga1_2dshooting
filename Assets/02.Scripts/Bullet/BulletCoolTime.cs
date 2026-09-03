using UnityEngine;

public class BulletCoolTime : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    public float CoolTime = 0.5f;

    private float LastFireTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= LastFireTime + CoolTime)
            {
                Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);

                LastFireTime = Time.time;
            }
        }
    }
}