using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // =========================
    // 점수 데이터
    // =========================

    private int _bestscore;
    private int _currentScore;


    // =========================
    // UI
    // =========================

    [SerializeField] private TextMeshProUGUI _bestScoreTextUI;
    [SerializeField] private TextMeshProUGUI _currentScoreTextUI;


    // =========================
    // 현재 점수 가져오기
    // =========================

    public int GetScore()
    {
        return _currentScore;
    }


    // =========================
    // 점수 추가
    // =========================

    public void AddScore(int score)
    {
        // 기존 점수에 추가
        _currentScore += score;


        // 최고 점수 갱신
        if (_currentScore > _bestscore)
        {
            _bestscore = _currentScore;
        }


        // 점수 UI 갱신
        Refresh();
    }


    // =========================
    // 초기화
    // =========================

    private void Start()
    {
        Refresh();
    }


    // =========================
    // UI 갱신
    // =========================

    private void Refresh()
    {
        if (_bestScoreTextUI != null)
        {
            _bestScoreTextUI.text = $"BestScore: {_bestscore}";
        }

        if (_currentScoreTextUI != null)
        {
            _currentScoreTextUI.text = $"Score: {_currentScore}";
        }
    }
}