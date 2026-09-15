using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Enemy Spawner")]
    [SerializeField] private EnemySpawner _enemySpawner;

    [Header("Background Scroller")]
    [SerializeField] private BackgroundScroller _backgroundScroller;

    [Header("Player")]
    [SerializeField] private Player _player;

    [Header("Mission Start Image")]
    [SerializeField] private Image _missionStartImage;

    [Header("Mission Start 이미지 5개")]
    [SerializeField] private Sprite[] _missionStartSprites;

    [Header("Mission Start 표시 시간")]
    [SerializeField] private float _missionDisplayTime = 1f;

    [Header("스테이지 전환 대기 시간")]
    [SerializeField] private float _backgroundChangeDelay = 0.3f;

    private int _currentStage = 1;
    private bool _isTransitioning = false;
    private bool _gameStarted = false;

    private void Awake()
    {
        HideMissionImage();
    }

    private void Start()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        _isTransitioning = true;

        // 플레이어를 화면 아래로 이동
        if (_player != null)
        {
            _player.SetStartPositionForStageTransition();
        }

        // 배경 정지
        if (_backgroundScroller != null)
        {
            _backgroundScroller.StopScroll();
            _backgroundScroller.ChangeBackground(1);
        }

        // 적 생성 정지
        if (_enemySpawner != null)
        {
            _enemySpawner.StopSpawn();
        }

        // MISSION 1 START 표시
        ShowMissionImage(1);

        Debug.Log(
            "MISSION 1 START 표시"
        );

        yield return new WaitForSeconds(
            _missionDisplayTime
        );

        // MISSION 1 START 숨김
        HideMissionImage();

        Debug.Log(
            "MISSION 1 START 숨김"
        );

        // 플레이어 등장
        if (_player != null)
        {
            yield return _player.MoveInForStageTransition();
        }

        // 배경 스크롤 시작
        if (_backgroundScroller != null)
        {
            _backgroundScroller.StartScroll();
        }

        // 적 생성 시작
        if (_enemySpawner != null)
        {
            _enemySpawner.StartSpawn();
        }

        _currentStage = 1;
        _gameStarted = true;
        _isTransitioning = false;

        Debug.Log(
            "MISSION 1 시작 완료"
        );
    }

    public void StartStageTransition(int stage)
    {
        if (_isTransitioning)
        {
            return;
        }

        if (!_gameStarted)
        {
            return;
        }

        if (stage < 1 ||
            stage > 5)
        {
            Debug.LogWarning(
                $"잘못된 Stage입니다 : {stage}"
            );

            return;
        }

        StartCoroutine(
            StageTransitionCoroutine(stage)
        );
    }

    private IEnumerator StageTransitionCoroutine(
        int stage
    )
    {
        _isTransitioning = true;

        // 배경 정지
        if (_backgroundScroller != null)
        {
            _backgroundScroller.StopScroll();
        }

        // 적 생성 정지
        if (_enemySpawner != null)
        {
            _enemySpawner.StopSpawn();
        }

        // 현재 화면의 모든 적 제거
        Enemy[] enemies =
            FindObjectsByType<Enemy>(
                FindObjectsSortMode.None
            );

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.StageClearDestroy();
            }
        }

        // 플레이어 화면 위로 이동
        if (_player != null)
        {
            yield return _player.MoveOutForStageTransition();
        }

        // 잠시 대기
        yield return new WaitForSeconds(
            _backgroundChangeDelay
        );

        // 배경 변경
        if (_backgroundScroller != null)
        {
            _backgroundScroller.ChangeBackground(stage);
        }

        // 해당 스테이지 Mission 이미지 표시
        ShowMissionImage(stage);

        Debug.Log(
            $"MISSION {stage} START 표시"
        );

        yield return new WaitForSeconds(
            _missionDisplayTime
        );

        // Mission 이미지 숨김
        HideMissionImage();

        Debug.Log(
            $"MISSION {stage} START 숨김"
        );

        // 플레이어 화면 아래에서 등장
        if (_player != null)
        {
            yield return _player.MoveInForStageTransition();
        }

        // 배경 스크롤 시작
        if (_backgroundScroller != null)
        {
            _backgroundScroller.StartScroll();
        }

        // 적 생성 시작
        if (_enemySpawner != null)
        {
            _enemySpawner.StartSpawn();
        }

        _currentStage = stage;
        _isTransitioning = false;

        Debug.Log(
            $"Stage {stage} 시작 완료"
        );
    }

    private void ShowMissionImage(int stage)
    {
        if (_missionStartImage == null)
        {
            Debug.LogError(
                "Mission Start Image가 연결되지 않았습니다."
            );

            return;
        }

        if (_missionStartSprites == null ||
            _missionStartSprites.Length == 0)
        {
            Debug.LogError(
                "Mission Start 이미지가 연결되지 않았습니다."
            );

            return;
        }

        int index =
            stage - 1;

        if (index < 0 ||
            index >= _missionStartSprites.Length)
        {
            Debug.LogError(
                $"MISSION {stage} 이미지가 없습니다."
            );

            return;
        }

        _missionStartImage.sprite =
            _missionStartSprites[index];

        _missionStartImage.enabled = true;
    }

    private void HideMissionImage()
    {
        if (_missionStartImage == null)
        {
            return;
        }

        _missionStartImage.enabled = false;
    }
}