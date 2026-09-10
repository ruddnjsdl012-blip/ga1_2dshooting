using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // =========================
    // Singleton
    // =========================

    public static ScoreManager Instance = null;


    // =========================
    // 점수 데이터
    // =========================

    private int _bestscore;
    private int _currentScore;

    // 마지막으로 UI를 갱신한 점수
    private int _lastRefreshScore = -1;


    // =========================
    // PlayerPrefs Key
    // =========================

    private const string BestScoreKey = "BestScore";


    // =========================
    // UI
    // =========================

    [SerializeField] private TextMeshProUGUI _bestScoreTextUI;
    [SerializeField] private TextMeshProUGUI _currentScoreTextUI;


    // =========================
    // Awake
    // =========================

    private void Awake()
    {
        // 이미 ScoreManager가 존재하면
        // 나중에 생성된 매니저를 삭제
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 최초로 생성된 ScoreManager를 Instance로 지정
        Instance = this;

        // Scene이 바뀌어도 ScoreManager 유지
        DontDestroyOnLoad(gameObject);


        // =========================
        // 최고 점수 불러오기
        // =========================

        _bestscore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }


    // =========================
    // Start
    // =========================

    private void Start()
    {
        Refresh();
    }


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
        // 현재 점수에 점수 추가
        _currentScore += score;


        // 현재 점수가 최고 점수보다 높으면
        if (_currentScore > _bestscore)
        {
            // 최고 점수 갱신
            _bestscore = _currentScore;

            // 최고 점수 저장
            PlayerPrefs.SetInt(BestScoreKey, _bestscore);

            // 저장
            PlayerPrefs.Save();
        }


        // UI 갱신
        Refresh();
    }


    // =========================
    // UI 갱신
    // =========================

    private void Refresh()
    {
        // 점수가 변하지 않았다면
        // UI를 다시 갱신하지 않음
        if (_lastRefreshScore == _currentScore)
        {
            return;
        }


        // 현재 점수 UI
        if (_currentScoreTextUI != null)
        {
            _currentScoreTextUI.text = $"Score: {_currentScore}";
        }


        // 최고 점수 UI
        if (_bestScoreTextUI != null)
        {
            _bestScoreTextUI.text = $"BestScore: {_bestscore}";
        }


        // 마지막으로 갱신한 점수 저장
        _lastRefreshScore = _currentScore;
    }
}