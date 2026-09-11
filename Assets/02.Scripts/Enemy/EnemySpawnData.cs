//데이터 클래스 : 오직 데이터를 저장하고 전달하기 위해 만드는 클래스

using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject EnemyPrefab;
    public int Weight;
}