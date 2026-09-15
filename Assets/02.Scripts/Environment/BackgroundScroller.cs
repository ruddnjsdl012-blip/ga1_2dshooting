using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // =========================
    // 배경 오브젝트 5개
    // =========================

    [Header("배경 오브젝트 5개")]
    [SerializeField] private Transform[] _backgrounds;


    // =========================
    // 배경 이동 속도
    // =========================

    [Header("배경 이동 속도")]
    [SerializeField] private float _moveSpeed = 3f;


    // =========================
    // 현재 스테이지
    // =========================

    private int _currentStage = 1;


    // =========================
    // 배경 스크롤 가능 여부
    // =========================

    private bool _canScroll = true;


    // =========================
    // Awake
    // =========================

    private void Awake()
    {
        if (_backgrounds == null ||
            _backgrounds.Length == 0)
        {
            Debug.LogError(
                "Backgrounds 배열에 배경이 연결되지 않았습니다."
            );

            return;
        }


        Debug.Log(
            $"BackgroundScroller Awake 실행 : 배경 {_backgrounds.Length}개"
        );
    }


    // =========================
    // Start
    // =========================

    private void Start()
    {
        ChangeBackground(_currentStage);
    }


    // =========================
    // Update
    // =========================

    private void Update()
    {
        if (!_canScroll)
        {
            return;
        }


        // =========================
        // 현재 스테이지 배경 가져오기
        // =========================

        Transform currentBackground =
            GetCurrentBackground();


        if (currentBackground == null)
        {
            return;
        }


        // =========================
        // 배경 이동
        // =========================

        currentBackground.position +=
            Vector3.down *
            _moveSpeed *
            Time.deltaTime;


        // =========================
        // Sprite Renderer 가져오기
        // =========================

        SpriteRenderer spriteRenderer =
            currentBackground.GetComponent<SpriteRenderer>();


        if (spriteRenderer == null)
        {
            return;
        }


        // =========================
        // 카메라 가져오기
        // =========================

        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            return;
        }


        // =========================
        // 카메라 아래쪽 위치
        // =========================

        float cameraBottom =
            mainCamera.transform.position.y -
            mainCamera.orthographicSize;


        // =========================
        // 배경의 위쪽 위치
        // =========================

        float backgroundTop =
            spriteRenderer.bounds.max.y;


        // =========================
        // 배경이 화면 아래로
        // 완전히 내려갔는지 확인
        // =========================

        if (backgroundTop <= cameraBottom)
        {
            MoveBackgroundToTop(
                currentBackground
            );
        }
    }


    // =========================
    // 현재 스테이지 배경 가져오기
    // =========================

    private Transform GetCurrentBackground()
    {
        int index =
            _currentStage - 1;


        if (_backgrounds == null)
        {
            return null;
        }


        if (index < 0 ||
            index >= _backgrounds.Length)
        {
            return null;
        }


        return _backgrounds[index];
    }


    // =========================
    // 배경을 화면 위쪽으로 이동
    // =========================

    private void MoveBackgroundToTop(
        Transform background
    )
    {
        // =========================
        // Sprite Renderer 가져오기
        // =========================

        SpriteRenderer spriteRenderer =
            background.GetComponent<SpriteRenderer>();


        if (spriteRenderer == null)
        {
            return;
        }


        // =========================
        // 카메라 가져오기
        // =========================

        Camera mainCamera =
            Camera.main;


        if (mainCamera == null)
        {
            return;
        }


        // =========================
        // 카메라 위쪽 위치
        // =========================

        float cameraTop =
            mainCamera.transform.position.y +
            mainCamera.orthographicSize;


        // =========================
        // 배경 실제 높이
        // =========================

        float backgroundHeight =
            spriteRenderer.bounds.size.y;


        // =========================
        // 배경을 카메라 위쪽으로 이동
        // =========================

        float newY =
            cameraTop +
            backgroundHeight / 2f;


        background.position =
            new Vector3(
                background.position.x,
                newY,
                background.position.z
            );
    }


    // =========================
    // 배경 스크롤 정지
    // =========================

    public void StopScroll()
    {
        _canScroll = false;


        Debug.Log(
            "배경 스크롤 정지"
        );
    }


    // =========================
    // 배경 스크롤 재개
    // =========================

    public void StartScroll()
    {
        _canScroll = true;


        Debug.Log(
            "배경 스크롤 재개"
        );
    }


    // =========================
    // 배경 변경
    // =========================

    public void ChangeBackground(int stage)
    {
        Debug.Log(
            $"ChangeBackground 호출됨 : Stage {stage}"
        );


        // =========================
        // 배열 확인
        // =========================

        if (_backgrounds == null)
        {
            Debug.LogError(
                "Backgrounds 배열이 없습니다."
            );

            return;
        }


        if (_backgrounds.Length == 0)
        {
            Debug.LogError(
                "Backgrounds 배열의 크기가 0입니다."
            );

            return;
        }


        // =========================
        // 스테이지 번호 확인
        // =========================

        if (stage < 1 ||
            stage > _backgrounds.Length)
        {
            Debug.LogError(
                $"잘못된 Stage입니다 : {stage}"
            );

            return;
        }


        // =========================
        // 현재 스테이지 변경
        // =========================

        _currentStage = stage;


        // =========================
        // 배경 활성화 / 비활성화
        // =========================

        for (int i = 0;
            i < _backgrounds.Length;
            i++)
        {
            if (_backgrounds[i] == null)
            {
                continue;
            }


            if (i == stage - 1)
            {
                _backgrounds[i].gameObject.SetActive(
                    true
                );
            }
            else
            {
                _backgrounds[i].gameObject.SetActive(
                    false
                );
            }
        }


        // =========================
        // 현재 배경 가져오기
        // =========================

        Transform currentBackground =
            GetCurrentBackground();


        if (currentBackground != null)
        {
            Camera mainCamera =
                Camera.main;


            if (mainCamera != null)
            {
                SpriteRenderer spriteRenderer =
                    currentBackground.GetComponent<SpriteRenderer>();


                if (spriteRenderer != null)
                {
                    // =========================
                    // 카메라 아래쪽
                    // =========================

                    float cameraBottom =
                        mainCamera.transform.position.y -
                        mainCamera.orthographicSize;


                    // =========================
                    // 카메라 위쪽
                    // =========================

                    float cameraTop =
                        mainCamera.transform.position.y +
                        mainCamera.orthographicSize;


                    // =========================
                    // 카메라 중앙
                    // =========================

                    float cameraCenter =
                        (cameraBottom + cameraTop) / 2f;


                    // =========================
                    // 배경을 화면 중앙에 배치
                    // =========================

                    currentBackground.position =
                        new Vector3(
                            currentBackground.position.x,
                            cameraCenter,
                            currentBackground.position.z
                        );
                }
            }
        }


        Debug.Log(
            $"배경 변경 완료 : Stage {stage}"
        );
    }
}