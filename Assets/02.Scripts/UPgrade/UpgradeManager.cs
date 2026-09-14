using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance = null;

    public static UpgradeManager Instance => _instance;


    private Upgrade _attackUpgrade;
    private Upgrade _moveSpeedUpgrade;
    private Upgrade _fireRateUpgrade;


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


    // =========================
    // 업그레이드 가져오기
    // =========================

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


    // =========================
    // 레벨업
    // =========================

    public bool LevelUp(int index)
    {
        Upgrade upgrade =
            GetUpgrade(index);


        if (upgrade == null)
        {
            return false;
        }


        // =========================
        // 점수 사용
        // =========================

        bool success =
            ScoreManager.Instance.UseScore(
                upgrade.Cost
            );


        // 점수가 부족하면 레벨업하지 않는다.
        if (success == false)
        {
            return false;
        }


        // =========================
        // 레벨업
        // =========================

        upgrade.LevelUp();


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


    // =========================
    // 현재 공격력
    // =========================

    public float GetAttackPower()
    {
        return _attackUpgrade.CurrentValue;
    }


    // =========================
    // 현재 이동속도
    // =========================

    public float GetMoveSpeed()
    {
        return _moveSpeedUpgrade.CurrentValue;
    }


    // =========================
    // 현재 발사속도
    // =========================

    public float GetFireRate()
    {
        return _fireRateUpgrade.CurrentValue;
    }
}