using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance = null;

    public static UpgradeManager Instance => _instance;

    private Upgrade _attackUpgrade;
    private Upgrade _moveSpeedUpgrade;
    private Upgrade _fireRateUpgrade;

    private const string AttackLevelKey = "Upgrade_Attack_Level";
    private const string MoveSpeedLevelKey = "Upgrade_MoveSpeed_Level";
    private const string FireRateLevelKey = "Upgrade_FireRate_Level";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

            return;
        }

        // =========================
        // 공격력
        // =========================

        _attackUpgrade =
            new Upgrade(
                0,
                "공격력",
                5f,
                5f,
                100f,
                1.5f
            );

        // =========================
        // 이동속도
        // =========================

        _moveSpeedUpgrade =
            new Upgrade(
                0,
                "이동속도",
                5f,
                0.5f,
                100f,
                1.5f
            );

        // =========================
        // 발사속도
        // =========================

        _fireRateUpgrade =
            new Upgrade(
                0,
                "발사속도",
                0.5f,
                -0.05f,
                100f,
                1.5f
            );
    }

    private void Start()
    {
        Load();
        RefreshUI();
    }

    public Upgrade GetUpgrade(int index)
    {
        switch (index)
        {
            case 0:
                return _attackUpgrade;

            case 1:
                return _moveSpeedUpgrade;

            case 2:
                return _fireRateUpgrade;

            default:
                Debug.LogError(
                    $"잘못된 Upgrade Index입니다 : {index}"
                );

                return null;
        }
    }

    public bool LevelUp(int index)
    {
        Upgrade upgrade =
            GetUpgrade(index);

        if (upgrade == null)
        {
            return false;
        }

        bool success =
            ScoreManager.Instance.UseScore(
                upgrade.Cost
            );

        if (success == false)
        {
            return false;
        }

        upgrade.LevelUp();

        Save();

        RefreshUI();

        Debug.Log(
            $"{upgrade.Name} 업그레이드 성공!"
        );

        Debug.Log(
            $"현재 레벨 : {upgrade.Level}"
        );

        Debug.Log(
            $"현재 능력치 : {upgrade.CurrentValue}"
        );

        Debug.Log(
            $"다음 능력치 : {upgrade.NextValue}"
        );

        return true;
    }

    public float GetAttackPower()
    {
        return _attackUpgrade.CurrentValue;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeedUpgrade.CurrentValue;
    }

    public float GetFireRate()
    {
        return _fireRateUpgrade.CurrentValue;
    }

    private void Save()
    {
        PlayerPrefs.SetInt(AttackLevelKey, _attackUpgrade.Level);

        PlayerPrefs.SetInt(MoveSpeedLevelKey, _moveSpeedUpgrade.Level);

        PlayerPrefs.SetInt(FireRateLevelKey, _fireRateUpgrade.Level);

        PlayerPrefs.Save();

        Debug.Log("업그레이드 데이터 저장 완료");
    }

    private void Load()
    {
        int attackLevel =
            PlayerPrefs.GetInt(AttackLevelKey, 0);

        int moveSpeedLevel = PlayerPrefs.GetInt(MoveSpeedLevelKey, 0);

        int fireRateLevel = PlayerPrefs.GetInt(FireRateLevelKey, 0);

        _attackUpgrade.SetLevel(attackLevel);

        _moveSpeedUpgrade.SetLevel(moveSpeedLevel);

        _fireRateUpgrade.SetLevel(fireRateLevel);

        Debug.Log($"공격력 레벨 불러오기 : {attackLevel}");

        Debug.Log($"이동속도 레벨 불러오기 : {moveSpeedLevel}");

        Debug.Log($"발사속도 레벨 불러오기 : {fireRateLevel}");
    }

    private void RefreshUI()
    {
        UI_Upgrade[] upgradeUIs =
            FindObjectsByType<UI_Upgrade>(
                FindObjectsSortMode.None
            );

        foreach (UI_Upgrade upgradeUI in upgradeUIs)
        {
            if (upgradeUI == null)
            {
                continue;
            }

            upgradeUI.Refresh();
        }

        Debug.Log($"업그레이드 UI 갱신 완료 : {upgradeUIs.Length}개");
    }
}