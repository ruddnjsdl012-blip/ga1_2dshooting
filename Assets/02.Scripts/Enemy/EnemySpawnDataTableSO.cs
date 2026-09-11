using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnDataTable", menuName = "Game/Enemy Spawn Data Table")]
public class EnemySpawnDataTableSO : ScriptableObject
{
    [SerializeField] private EnemySpawnData[] _datas;

    public EnemySpawnData[] Datas => _datas;
}