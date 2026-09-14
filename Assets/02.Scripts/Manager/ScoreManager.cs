using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    // =========================
    // 점수
    // =========================

    private int _bestscore;
    private int _currentScore;

    private const string BestScoreKey = "BestScore";


    // =========================
    // UI
    // =========================

    [SerializeField] private TextMeshProUGUI _bestScoreTextUI;
    [SerializeField] private TextMeshProUGUI _currentScoreTextUI;

    [SerializeField] private ScoreUIEffect _scoreUIEffect;


    // =========================
    // 스테이지
    // =========================

    [SerializeField] private StageManager _stageManager;

    private int _currentStage = 1;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        _bestscore =
            PlayerPrefs.GetInt(
                BestScoreKey,
                0
            );
    }


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
        _currentScore += score;


        // =========================
        // 최고 점수 갱신
        // =========================

        if (_currentScore > _bestscore)
        {
            _bestscore = _currentScore;

            PlayerPrefs.SetInt(
                BestScoreKey,
                _bestscore
            );

            PlayerPrefs.Save();
        }


        // =========================
        // UI 갱신
        // =========================

        Refresh();


        // =========================
        // 점수 증가 효과
        // =========================

        if (_scoreUIEffect != null)
        {
            _scoreUIEffect.PlayScoreEffect();
        }


        // =========================
        // 스테이지 확인
        // =========================

        CheckStage();
    }


    // =========================
    // 점수 사용
    // =========================

    public bool UseScore(int cost)
    {
        // 점수가 부족하면 사용하지 않는다.
        if (_currentScore < cost)
        {
            Debug.Log(
                $"점수가 부족합니다. " +
                $"현재 점수 : {_currentScore} / " +
                $"필요 점수 : {cost}"
            );

            return false;
        }


        // 점수 차감
        _currentScore -= cost;


        // UI 갱신
        Refresh();


        Debug.Log(
            $"점수 사용 : -{cost} / " +
            $"현재 점수 : {_currentScore}"
        );


        return true;
    }


    // =========================
    // 스테이지 확인
    // =========================

    private void CheckStage()
    {
        int targetStage =
            (_currentScore / 1000) + 1;


        targetStage =
            Mathf.Clamp(
                targetStage,
                1,
                5
            );


        if (targetStage == _currentStage)
        {
            return;
        }


        _currentStage = targetStage;


        Debug.Log(
            $"스테이지 변경: Stage {_currentStage}"
        );


        if (_stageManager != null)
        {
            _stageManager.StartStageTransition();
        }
        else
        {
            Debug.LogWarning(
                "ScoreManager에 StageManager가 연결되지 않았습니다."
            );
        }
    }


    // =========================
    // UI 갱신
    // =========================

    private void Refresh()
    {
        if (_currentScoreTextUI != null)
        {
            _currentScoreTextUI.text =
                $"Score: {_currentScore}";
        }


        if (_bestScoreTextUI != null)
        {
            _bestScoreTextUI.text =
                $"BestScore: {_bestscore}";
        }
    }
}