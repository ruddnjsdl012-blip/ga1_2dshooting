using UnityEngine;

public class UpgradeSaveData : MonoBehaviour
{
    public string[] Name;
    public int[] Level;

    public UpgradeSaveData(int count)
    {
        Name = new string[count];
        Level = new int[count];
    }
}