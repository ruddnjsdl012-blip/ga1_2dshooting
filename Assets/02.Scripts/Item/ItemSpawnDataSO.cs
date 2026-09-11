using UnityEngine;

[CreateAssetMenu(
    fileName = "ItemSpawnDataTable",
    menuName = "Game/Item Spawn Data Table"
)]
public class ItemSpawnDataTableSO : ScriptableObject
{
    [SerializeField] private ItemSpawnData[] _datas;

    public ItemSpawnData[] Datas => _datas;
}