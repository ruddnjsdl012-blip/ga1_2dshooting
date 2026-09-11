using UnityEngine;

[System.Serializable]
public class ItemSpawnData
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private int _weight;

    public GameObject ItemPrefab => _itemPrefab;
    public int Weight => _weight;
}