using UnityEngine;

public class PlayerMove : MonoBehaviour
{
     // 이 스크립트의 목적 : 키보드 입력에 따라서 플레이어 이동 처리를 하고싶다. 
     
     // 필요 필드 :
     public float Speed;
     
     public float MinX = -2.4f;
     public float MaxX = 2.4f;
     public float MinY = -4.7f;
     public float MaxY = 0f;
     
     
     // 매 프레임마다 실행된다.
     // 초당 프레임 실행 횟수는 : 별다른 설정이 없을 경우 가능한 많이 
   private void Update()
   {

         
         
         // 1. 키보드 입력을 받는다.
         // if (Input.GetKey(KeyCode.LeftArrow)) 를 넣으면 왼쪽으로 움직인다는 값을 넣는거임
         // Debug.Log("왼쪽 방향키를 누르는 중");
         {
             float h = Input.GetAxisRaw("Horizontal");  // 키보드 왼/오른쪽 입력 상태에 따라 -1f ~0 ~ 1f 까지 반환
             float v = Input.GetAxisRaw("Vertical");    // 키보드 위/아래 입력 상태에 따라 -1f ~0 ~ 1f 까지 반환
             
             Debug.Log($"h:{h}, v:{v}");


             // 2. 키보드 입력에 따라 방향을 구한다.
             // 게임에는 벡터라는 타입이 있다 벡터는(크기와 방향을 의미)
             Vector2 direction = new Vector2(h, v); // 왼쪽 방향
             // Vector2 direction = Vector2.left;  -> 이미 만들어져있음


             // 3. 방향과 속력에 따라 이동한다.
             // 속도 = 방향 * 속력 
                                                         // 매직 넘버란 : 보는 사람에 따라 의미가 달라질 수 있는
                                                         // 헷갈리는 숫자
             Vector2 normalized = (direction * Speed).normalized;
             // 벡터의 길이를 1로 만들어 주는 것 (즉,방향만 유지된다) 변수명은 의미가 있어야 한다.
             
             transform.Translate(direction * Speed * Time.deltaTime);
             // deltaTime : 이전 프레임으로 부터 지금 프레임까지 시간이 얼마나 지났는지 Ms단위로 반환 
             // Ms -> 1/1000 초 
             
             // 새로운 위치 = 현재 위치 + (방향 * 속력 * 시간)
             // transform.position += (Vector3)direction * Speed * Time.deltaTime;
             
             float posX = transform.position.x;
             float posY = Mathf.Clamp(transform.position.y, MinY, MaxY);

             if (posX > MaxX)
             {
                 posX = MinX;
             }
             else if (posX < MinX)
             {
                 posX = MaxX;
             }

             transform.position = new Vector3(posX, posY, transform.position.z);
             
             SpeedChange();
         }
    }

    private void SpeedChange()
    {
        // E = 속도 증가
        if (Input.GetKeyDown(KeyCode.E))
        {
            Speed += 1f;
        }

        // Q = 속도 감소
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Speed = Mathf.Max(0f, Speed - 1f);
        }
    }
    
}
