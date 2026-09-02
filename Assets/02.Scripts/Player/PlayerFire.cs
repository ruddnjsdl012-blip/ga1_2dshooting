using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  //목표 : 스페이스바를 누를 떄마다 총알을 생성해서 발사하고 싶다.
  // 필요 속성
  // - 총알 프리팹
  // - 생성 위치 (총구)
  public GameObject BulletPrefab;
  // -생성 위치 (총구)
  public Transform FirePoint;
  
  
  private void Update()
  {
      if (Input.GetKeyDown(KeyCode.Space))
      {
          //2. 총알 프래팹을 생성한다.
          // instantiate 는 프리팹을 복사해서(monobahaviour를 상속받는) 게임 오브젝트를 생성하고 씬에 넣어주는 기능
          
          GameObject bullet = Instantiate(BulletPrefab);
          bullet.transform.position = FirePoint.position; // 생성한 총알의 위치를 나(플레이어)의 위치로

      }
  }

  
}
