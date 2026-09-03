using UnityEngine;

public class Bullet : MonoBehaviour
{
    //목적: 총알을 위로 움직이고 싶다.
    public float Speed;

    private void Update()
    {
        Vector2 direction = Vector2.up;
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}