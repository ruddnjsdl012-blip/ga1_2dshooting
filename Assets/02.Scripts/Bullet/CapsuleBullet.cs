using UnityEngine;

public class CapsuleBullet : MonoBehaviour

{
    public float Speed;

    private void Update()
    {
        Vector2 direction = Vector2.up; //  new Vector2(1, 0);
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}