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

    // 현재 스테이지
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

        _bestscore = PlayerPrefs.GetInt(
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
        // 점수 추가
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
    // 스테이지 확인
    // =========================

    private void CheckStage()
    {
        // 현재 점수로 몇 번째 스테이지인지 계산한다.
        //
        // 0 ~ 999     → Stage 1
        // 1000 ~ 1999 → Stage 2
        // 2000 ~ 2999 → Stage 3
        // 3000 ~ 3999 → Stage 4
        // 4000 이상   → Stage 5

        int targetStage =
            (_currentScore / 1000) + 1;


        // Stage 5를 초과하지 않도록 제한
        targetStage =
            Mathf.Clamp(targetStage, 1, 5);


        // 현재 스테이지와 같으면 아무것도 하지 않는다.
        if (targetStage == _currentStage)
        {
            return;
        }


        // 스테이지가 변경되었다.
        _currentStage = targetStage;


        Debug.Log(
            $"스테이지 변경: Stage {_currentStage}"
        );


        // StageManager에게 스테이지 전환 요청
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