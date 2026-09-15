using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // =========================
    // 체력
    // =========================

    [SerializeField] private int _health = 100;

    public int Health => _health;


    // =========================
    // 사망 이펙트
    // =========================

    [SerializeField] private GameObject _deathEffect;


    // =========================
    // 피격 사운드
    // =========================

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _hitSound;


    // =========================
    // 폭탄
    // =========================

    [SerializeField] private GameObject _playerBomb;

    [SerializeField] private BombEffectUI _bombEffectUI;


    private const float BombCooldown = 10f;

    private float _bombCooldownTimer = 0f;


    // =========================
    // 스테이지 전환 속도
    // =========================

    [Header("스테이지 전환 속도")]
    [SerializeField] private float _stageTransitionSpeed = 5f;


    // =========================
    // 원래 플레이어 위치
    // =========================

    private Vector3 _normalPosition;


    // =========================
    // 스테이지 전환 중
    // =========================

    private bool _isStageTransitioning = false;


    // =========================
    // 전환 전에 켜져 있던 스크립트
    // =========================

    private MonoBehaviour[] _disabledScripts;


    // =========================
    // Awake
    // =========================

    private void Awake()
    {
        // Inspector에서 배치한
        // 플레이어의 원래 위치를 기억합니다.

        _normalPosition =
            transform.position;
    }


    // =========================
    // Update
    // =========================

    private void Update()
    {
        if (_isStageTransitioning)
        {
            return;
        }


        // =========================
        // 폭탄 쿨타임
        // =========================

        if (_bombCooldownTimer > 0f)
        {
            _bombCooldownTimer -=
                Time.deltaTime;
        }


        // =========================
        // 폭탄
        // =========================

        if (Input.GetKeyDown(KeyCode.B))
        {
            UseBomb();
        }
    }


    // =========================
    // 체력 가져오기
    // =========================

    public int GetHealth()
    {
        return _health;
    }


    // =========================
    // 대미지
    // =========================

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning(
                "대미지는 음수일 수 없습니다."
            );

            return;
        }


        _health -= damage;


        if (_audioSource != null &&
            _hitSound != null)
        {
            _audioSource.PlayOneShot(
                _hitSound
            );
        }


        if (_health <= 0)
        {
            if (_deathEffect != null)
            {
                Instantiate(
                    _deathEffect,
                    transform.position,
                    Quaternion.identity
                );
            }


            Destroy(gameObject);
        }
    }


    // =========================
    // 회복
    // =========================

    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        {
            Debug.LogWarning(
                "힐량은 음수일 수 없습니다."
            );

            return;
        }


        _health += healAmount;
    }


    // =========================
    // 게임 시작 위치
    // =========================

    public void SetStartPositionForStageTransition()
    {
        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            return;
        }


        // 카메라 화면 아래쪽
        float bottomY =
            mainCamera.transform.position.y -
            mainCamera.orthographicSize;


        // 화면보다 5만큼 더 아래
        bottomY -= 5f;


        // 플레이어를 화면 밖으로 이동
        transform.position =
            new Vector3(
                _normalPosition.x,
                bottomY,
                _normalPosition.z
            );


        Debug.Log(
            "플레이어를 화면 아래 밖으로 이동했습니다."
        );
    }


    // =========================
    // 플레이어가 화면 위로 나가기
    // =========================

    public IEnumerator MoveOutForStageTransition()
    {
        _isStageTransitioning = true;


        DisablePlayerControl();


        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );


            EnablePlayerControl();

            _isStageTransitioning = false;

            yield break;
        }


        // 화면 위쪽
        float targetY =
            mainCamera.transform.position.y +
            mainCamera.orthographicSize +
            2f;


        Vector3 targetPosition =
            new Vector3(
                transform.position.x,
                targetY,
                transform.position.z
            );


        yield return MoveToPosition(
            targetPosition
        );


        Debug.Log(
            "플레이어 화면 위쪽 이동 완료"
        );
    }


    // =========================
    // 플레이어가 화면 아래에서 들어오기
    // =========================

    public IEnumerator MoveInForStageTransition()
    {
        _isStageTransitioning = true;


        // =========================================
        // 중요
        // =========================================
        // MoveOutForStageTransition()에서
        // 이미 플레이어 조작 스크립트를 꺼놓았기 때문에
        // 여기서 다시 DisablePlayerControl()을 호출하면 안 됩니다.
        //
        // 기존에 꺼놓은 스크립트 목록을 그대로 유지합니다.


        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );


            EnablePlayerControl();

            _isStageTransitioning = false;

            yield break;
        }


        // 화면 아래쪽
        float bottomY =
            mainCamera.transform.position.y -
            mainCamera.orthographicSize;


        // 화면 밖으로 5만큼 더 아래
        bottomY -= 5f;


        Vector3 bottomPosition =
            new Vector3(
                _normalPosition.x,
                bottomY,
                transform.position.z
            );


        // 플레이어를 화면 아래 밖에 배치
        transform.position =
            bottomPosition;


        Debug.Log(
            "플레이어 화면 아래에서 등장 준비"
        );


        // 원래 위치까지 이동
        yield return MoveToPosition(
            _normalPosition
        );


        // =========================================
        // 플레이어 조작 재개
        // =========================================

        EnablePlayerControl();


        _isStageTransitioning = false;


        Debug.Log(
            "플레이어 화면 아래 등장 완료"
        );
    }


    // =========================
    // 목표 위치까지 이동
    // =========================

    private IEnumerator MoveToPosition(
        Vector3 targetPosition
    )
    {
        while (
            Vector3.Distance(
                transform.position,
                targetPosition
            ) > 0.05f
        )
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _stageTransitionSpeed *
                    Time.deltaTime
                );


            yield return null;
        }


        transform.position =
            targetPosition;
    }


    // =========================
    // 플레이어 조작 정지
    // =========================

    private void DisablePlayerControl()
    {
        // 이미 저장된 스크립트 목록이 있다면
        // 다시 끄지 않습니다.

        if (_disabledScripts != null)
        {
            return;
        }


        MonoBehaviour[] scripts =
            GetComponents<MonoBehaviour>();


        _disabledScripts =
            new MonoBehaviour[
                scripts.Length
            ];


        int index = 0;


        foreach (MonoBehaviour script in scripts)
        {
            if (script == null)
            {
                continue;
            }


            if (script == this)
            {
                continue;
            }


            if (!script.enabled)
            {
                continue;
            }


            script.enabled = false;


            _disabledScripts[index] =
                script;


            index++;
        }


        Debug.Log(
            "스테이지 전환 : 플레이어 조작 정지"
        );
    }


    // =========================
    // 플레이어 조작 재개
    // =========================

    private void EnablePlayerControl()
    {
        if (_disabledScripts == null)
        {
            return;
        }


        foreach (
            MonoBehaviour script
            in _disabledScripts
        )
        {
            if (script == null)
            {
                continue;
            }


            script.enabled = true;
        }


        _disabledScripts = null;


        Debug.Log(
            "스테이지 전환 : 플레이어 조작 재개"
        );
    }


    // =========================
    // 폭탄 사용
    // =========================

    private void UseBomb()
    {
        if (_bombCooldownTimer > 0f)
        {
            Debug.Log(
                $"폭탄 쿨타임 중입니다. " +
                $"{_bombCooldownTimer:F1}초 남음"
            );

            return;
        }


        if (_playerBomb == null)
        {
            Debug.LogWarning(
                "PlayerBomb 프리팹이 연결되지 않았습니다."
            );

            return;
        }


        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            return;
        }


        Vector3 screenCenter =
            new Vector3(
                0.5f,
                0.5f,
                Mathf.Abs(
                    mainCamera.transform.position.z -
                    transform.position.z
                )
            );


        Vector3 bombPosition =
            mainCamera.ViewportToWorldPoint(
                screenCenter
            );


        Instantiate(
            _playerBomb,
            bombPosition,
            Quaternion.identity
        );


        if (_bombEffectUI != null)
        {
            _bombEffectUI.Play();
        }
        else
        {
            Debug.LogError(
                "Player의 Bomb Effect UI가 연결되지 않았습니다."
            );
        }


        _bombCooldownTimer =
            BombCooldown;


        Debug.Log(
            "B키 → 폭탄 사용"
        );
    }
}