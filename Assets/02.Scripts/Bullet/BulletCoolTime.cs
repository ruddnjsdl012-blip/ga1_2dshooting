using UnityEngine;

public class BulletCoolTime : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    public float CoolTime = 0.5f;

    private float lastFireTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= lastFireTime + CoolTime)
            {
                Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);

                lastFireTime = Time.time;
            }
        }
    }
}