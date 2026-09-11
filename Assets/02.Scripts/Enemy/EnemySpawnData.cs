using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _weight;

    public GameObject EnemyPrefab => _enemyPrefab;
    public int Weight => _weight;
}