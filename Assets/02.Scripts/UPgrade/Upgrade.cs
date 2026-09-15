using UnityEngine;

public class Upgrade
{
    // =========================
    // 기획자가 설정하는 값
    // =========================

    private string _name;

    private float _defaultValue;

    private float _increaseValue;

    private float _defaultCost;

    private float _increaseCost;


    // =========================
    // 실행 중에 변경되는 값
    // =========================

    private int _level;

    private float _currentValue;

    private float _nextValue;

    private int _cost;


    // =========================
    // 외부에서 읽기
    // =========================

    public string Name => _name;

    public int Level => _level;

    public float CurrentValue => _currentValue;

    public float NextValue => _nextValue;

    public int Cost => _cost;


    // =========================
    // 생성자
    // =========================

    public Upgrade(
        int level,
        string name,
        float defaultValue,
        float increaseValue,
        float defaultCost,
        float increaseCost)
    {
        _level = level;

        _name = name;

        _defaultValue = defaultValue;

        _increaseValue = increaseValue;

        _defaultCost = defaultCost;

        _increaseCost = increaseCost;


        Calculate();
    }


    // =========================
    // 레벨업
    // =========================

    public void LevelUp()
    {
        _level += 1;

        Calculate();
    }


    // =========================
    // 저장된 레벨 적용
    // =========================

    public void SetLevel(int level)
    {
        _level = level;

        Calculate();
    }


    // =========================
    // 값 계산
    // =========================

    private void Calculate()
    {
        // =========================
        // 현재 능력치
        // =========================

        _currentValue =
            _defaultValue +
            _level * _increaseValue;


        // =========================
        // 다음 능력치
        // =========================

        _nextValue =
            _defaultValue +
            (_level + 1) *
            _increaseValue;


        // =========================
        // 업그레이드 비용
        // =========================

        _cost =
            (int)(
                _defaultCost *
                Mathf.Pow(
                    _increaseCost,
                    _level
                )
            );
    }
}