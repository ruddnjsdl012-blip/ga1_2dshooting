using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 100;
    public float speed = 3f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}