using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("스테이지별 배경 2개")]
    [SerializeField] private BackGroundPair[] _backgroundPairs;

    [Header("배경 이동 속도")]
    [SerializeField] private float _moveSpeed = 3f;

    private int _currentStage = 1;
    private bool _canScroll = true;

    private void Awake()
    {
        if (_backgroundPairs == null ||
            _backgroundPairs.Length == 0)
        {
            Debug.LogError(
                "Background Pairs 배열에 배경이 연결되지 않았습니다."
            );

            return;
        }
    }

    private void Start()
    {
        ChangeBackground(_currentStage);
    }

    private void Update()
    {
        if (!_canScroll)
        {
            return;
        }

        BackGroundPair currentPair =
            GetCurrentBackgroundPair();

        if (currentPair == null)
        {
            return;
        }

        Transform backgroundA =
            currentPair.BackgroundA;

        Transform backgroundB =
            currentPair.BackgroundB;

        if (backgroundA == null ||
            backgroundB == null)
        {
            return;
        }

        // 두 배경을 아래로 이동
        backgroundA.position +=
            Vector3.down *
            _moveSpeed *
            Time.deltaTime;

        backgroundB.position +=
            Vector3.down *
            _moveSpeed *
            Time.deltaTime;

        // A가 아래로 빠졌는지 확인
        CheckBackgroundLoop(
            backgroundA,
            backgroundB
        );

        // B가 아래로 빠졌는지 확인
        CheckBackgroundLoop(
            backgroundB,
            backgroundA
        );
    }

    private BackGroundPair GetCurrentBackgroundPair()
    {
        int index =
            _currentStage - 1;

        if (_backgroundPairs == null)
        {
            return null;
        }

        if (index < 0 ||
            index >= _backgroundPairs.Length)
        {
            return null;
        }

        return _backgroundPairs[index];
    }

    private void CheckBackgroundLoop(
        Transform currentBackground,
        Transform otherBackground
    )
    {
        SpriteRenderer currentRenderer =
            currentBackground.GetComponent<SpriteRenderer>();

        if (currentRenderer == null)
        {
            return;
        }

        Camera mainCamera =
            Camera.main;

        if (mainCamera == null)
        {
            return;
        }

        float cameraBottom =
            mainCamera.transform.position.y -
            mainCamera.orthographicSize;

        float currentTop =
            currentRenderer.bounds.max.y;

        if (currentTop <= cameraBottom)
        {
            MoveBackgroundToTop(
                currentBackground,
                otherBackground
            );
        }
    }

    private void MoveBackgroundToTop(
        Transform currentBackground,
        Transform otherBackground
    )
    {
        SpriteRenderer currentRenderer =
            currentBackground.GetComponent<SpriteRenderer>();

        SpriteRenderer otherRenderer =
            otherBackground.GetComponent<SpriteRenderer>();

        if (currentRenderer == null ||
            otherRenderer == null)
        {
            return;
        }

        float otherTop =
            otherRenderer.bounds.max.y;

        float currentHalfHeight =
            currentRenderer.bounds.extents.y;

        currentBackground.position =
            new Vector3(
                otherBackground.position.x,
                otherTop + currentHalfHeight,
                currentBackground.position.z
            );
    }

    public void StopScroll()
    {
        _canScroll = false;

        Debug.Log(
            "배경 스크롤 정지"
        );
    }

    public void StartScroll()
    {
        _canScroll = true;

        Debug.Log(
            "배경 스크롤 재개"
        );
    }

    public void ChangeBackground(int stage)
    {
        Debug.Log(
            $"ChangeBackground 호출됨 : Stage {stage}"
        );

        if (_backgroundPairs == null ||
            _backgroundPairs.Length == 0)
        {
            Debug.LogError(
                "Background Pairs 배열이 없습니다."
            );

            return;
        }

        if (stage < 1 ||
            stage > _backgroundPairs.Length)
        {
            Debug.LogError(
                $"잘못된 Stage입니다 : {stage}"
            );

            return;
        }

        _currentStage = stage;

        // =====================================================
        // 1. 모든 스테이지 배경 끄기
        // =====================================================

        for (int i = 0;
            i < _backgroundPairs.Length;
            i++)
        {
            BackGroundPair pair =
                _backgroundPairs[i];

            if (pair == null)
            {
                continue;
            }

            if (pair.BackgroundA != null)
            {
                pair.BackgroundA.gameObject.SetActive(false);
            }

            if (pair.BackgroundB != null)
            {
                pair.BackgroundB.gameObject.SetActive(false);
            }
        }

        // =====================================================
        // 2. 현재 스테이지 배경 가져오기
        // =====================================================

        BackGroundPair currentPair =
            _backgroundPairs[stage - 1];

        if (currentPair == null)
        {
            Debug.LogError(
                $"Stage {stage}의 BackGroundPair가 없습니다."
            );

            return;
        }

        if (currentPair.BackgroundA == null ||
            currentPair.BackgroundB == null)
        {
            Debug.LogError(
                $"Stage {stage}의 배경 A 또는 B가 없습니다."
            );

            return;
        }

        // =====================================================
        // 3. 현재 스테이지 배경만 켜기
        // =====================================================

        currentPair.BackgroundA.gameObject.SetActive(true);
        currentPair.BackgroundB.gameObject.SetActive(true);

        SpriteRenderer rendererA =
            currentPair.BackgroundA.GetComponent<SpriteRenderer>();

        SpriteRenderer rendererB =
            currentPair.BackgroundB.GetComponent<SpriteRenderer>();

        if (rendererA == null ||
            rendererB == null)
        {
            Debug.LogError(
                $"Stage {stage} 배경에 Sprite Renderer가 없습니다."
            );

            return;
        }

        // =====================================================
        // 4. A를 카메라 중앙에 배치
        // =====================================================

        Camera mainCamera =
            Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError(
                "Main Camera를 찾을 수 없습니다."
            );

            return;
        }

        float cameraCenterY =
            mainCamera.transform.position.y;

        currentPair.BackgroundA.position =
            new Vector3(
                currentPair.BackgroundA.position.x,
                cameraCenterY,
                currentPair.BackgroundA.position.z
            );

        // =====================================================
        // 5. B를 A 바로 위에 붙이기
        // =====================================================

        float backgroundATop =
            rendererA.bounds.max.y;

        float backgroundBHalfHeight =
            rendererB.bounds.extents.y;

        currentPair.BackgroundB.position =
            new Vector3(
                currentPair.BackgroundA.position.x,
                backgroundATop +
                backgroundBHalfHeight,
                currentPair.BackgroundB.position.z
            );

        Debug.Log(
            $"Stage {stage} 배경 변경 완료"
        );
    }
}