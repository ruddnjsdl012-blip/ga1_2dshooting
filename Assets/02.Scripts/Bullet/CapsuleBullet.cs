using UnityEngine;

public class CapsuleBullet : MonoBehaviour

{
    public float Speed;

    private void Update()
    {
        Vector2 direction = Vector2.up; //  new Vector2(1, 0);
        transform.Translate(direction * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 했다!");

        //나죽고
        Destroy(this.gameObject);

        if (collision.gameObject.CompareTag("Enemy"))

            //너죽자
            Destroy(collision.gameObject);
    }
}