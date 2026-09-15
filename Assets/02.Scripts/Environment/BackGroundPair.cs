using UnityEngine;

[System.Serializable]
public class BackGroundPair
{
    [SerializeField] private Transform _backgroundA;
    [SerializeField] private Transform _backgroundB;

    public Transform BackgroundA => _backgroundA;
    public Transform BackgroundB => _backgroundB;
}