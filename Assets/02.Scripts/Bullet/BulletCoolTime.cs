using UnityEngine;

public class BulletCoolTime : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    public float CoolTime = 0.5f;

    private float _lastFireTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= _lastFireTime + CoolTime)
            {
                Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);

                _lastFireTime = Time.time;
            }
        }
    }
}