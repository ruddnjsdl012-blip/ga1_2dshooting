using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // =========================
    // 적 스포너
    // =========================

    [SerializeField] private EnemySpawner _enemySpawner;


    // =========================
    // 배경
    // =========================

    [SerializeField] private BackgroundScroller _backgroundScroller;


    // =========================
    // 플레이어
    // =========================

    [SerializeField] private Player _player;


    // =========================
    // 스테이지 전환 중인지 여부
    // =========================

    private bool _isTransitioning = false;


    // =========================
    // 스테이지 전환 시작
    // =========================

    public void StartStageTransition(int stage)
    {
        // 이미 스테이지 전환 중이면
        // 중복 실행하지 않는다.
        if (_isTransitioning)
        {
            return;
        }


        StartCoroutine(
            StageTransitionCoroutine(stage)
        );
    }


    // =========================
    // 스테이지 전환 코루틴
    // =========================

    private IEnumerator StageTransitionCoroutine(
        int stage
    )
    {
        _isTransitioning = true;


        Debug.Log(
            $"스테이지 전환 시작 : Stage {stage}"
        );


        // =========================
        // 1. 배경 스크롤 정지
        // =========================

        if (_backgroundScroller != null)
        {
            _backgroundScroller.StopScroll();

            Debug.Log(
                "배경 스크롤 정지"
            );
        }


        // =========================
        // 2. 적 스폰 중지
        // =========================

        if (_enemySpawner != null)
        {
            _enemySpawner.StopSpawn();

            Debug.Log(
                "EnemySpawner 스폰 중지"
            );
        }


        // =========================
        // 3. 현재 존재하는 Enemy 찾기
        // =========================

        Enemy[] enemies =
            FindObjectsByType<Enemy>(
                FindObjectsSortMode.None
            );


        // =========================
        // 4. 현재 Enemy 제거
        // =========================

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.StageClearDestroy();
            }
        }


        Debug.Log(
            $"현재 Enemy {enemies.Length}마리 제거"
        );


        // =========================
        // 5. 플레이어 화면 위로 이동
        // =========================

        if (_player != null)
        {
            yield return
                _player.MoveOutForStageTransition();
        }
        else
        {
            Debug.LogWarning(
                "StageManager에 Player가 연결되지 않았습니다."
            );
        }


        // =========================
        // 6. 잠시 대기
        // =========================

        yield return new WaitForSeconds(
            0.3f
        );


        // =========================
        // 7. 배경 변경
        // =========================

        if (_backgroundScroller != null)
        {
            _backgroundScroller.ChangeBackground(
                stage
            );

            Debug.Log(
                $"배경 변경 완료 : Stage {stage}"
            );
        }
        else
        {
            Debug.LogWarning(
                "StageManager에 BackgroundScroller가 연결되지 않았습니다."
            );
        }


        // =========================
        // 8. 잠시 대기
        // =========================

        yield return new WaitForSeconds(
            0.3f
        );


        // =========================
        // 9. 플레이어 화면 아래에서 등장
        // =========================

        if (_player != null)
        {
            yield return
                _player.MoveInForStageTransition();
        }


        // =========================
        // 10. 배경 스크롤 재개
        // =========================

        if (_backgroundScroller != null)
        {
            _backgroundScroller.StartScroll();

            Debug.Log(
                "배경 스크롤 재개"
            );
        }


        // =========================
        // 11. 적 스폰 재개
        // =========================

        if (_enemySpawner != null)
        {
            _enemySpawner.StartSpawn();

            Debug.Log(
                "EnemySpawner 스폰 재개"
            );
        }


        // =========================
        // 전환 종료
        // =========================

        _isTransitioning = false;


        Debug.Log(
            $"스테이지 전환 완료 : Stage {stage}"
        );
    }
}