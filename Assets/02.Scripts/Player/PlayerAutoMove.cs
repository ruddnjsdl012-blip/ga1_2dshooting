using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{   
    [SerializeField] private float _speed;
    

    private void Update()
    {
     // 1. 타겟을 구한다.
     GameObject target = GameObject.FindWithTag("Enemy");
     if (target == null) return;
     
     // 2. 방향을 구한다.
     Vector3 direction = target.transform.position - transform.position;
     direction.Normalize();
     direction.y = 0;
     
     //3.속도에 맞게 이동한다.
     transform.position += direction * _speed * Time.deltaTime;

    }
}
