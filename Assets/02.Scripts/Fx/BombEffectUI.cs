using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombEffectUI : MonoBehaviour
{
    // =========================
    // 이미지
    // =========================

    [SerializeField] private Image _image;


    // =========================
    // 폭탄 이미지
    // =========================

    [SerializeField] private Sprite[] _bombSprites;


    // =========================
    // 애니메이션 설정
    // =========================

    [Header("이미지 애니메이션")]
    [SerializeField] private float _frameTime = 0.1f;

    [SerializeField] private float _duration = 3f;


    // =========================
    // 크기 애니메이션 설정
    // =========================

    [Header("크기 애니메이션")]

    // 시작 크기
    [SerializeField] private float _startScale = 0.3f;

    // 가장 커지는 크기
    [SerializeField] private float _maxScale = 2f;

    // 다시 작아지는 크기
    [SerializeField] private float _endScale = 0.5f;


    // =========================
    // 크기 변화 시간
    // =========================

    [SerializeField] private float _scaleUpTime = 0.5f;

    [SerializeField] private float _scaleDownTime = 0.5f;


    // =========================
    // 코루틴
    // =========================

    private Coroutine _animationCoroutine;


    // =========================
    // RectTransform
    // =========================

    private RectTransform _rectTransform;


    // =========================
    // 초기화
    // =========================

    private void Awake()
    {
        // Image 확인
        if (_image == null)
        {
            Debug.LogError(
                "BombEffectUI: Image가 연결되지 않았습니다."
            );

            return;
        }


        // Image의 RectTransform 가져오기
        _rectTransform = _image.rectTransform;


        // 시작할 때 숨기기
        _image.enabled = false;


        // 시작 크기 설정
        _rectTransform.localScale =
            Vector3.one * _startScale;
    }


    // =========================
    // 폭탄 효과 실행
    // =========================

    public void Play()
    {
        // Image 확인
        if (_image == null)
        {
            Debug.LogError(
                "BombEffectUI: Image가 연결되지 않았습니다."
            );

            return;
        }


        // 폭탄 이미지 확인
        if (_bombSprites == null ||
            _bombSprites.Length == 0)
        {
            Debug.LogError(
                "BombEffectUI: 폭탄 이미지가 등록되지 않았습니다."
            );

            return;
        }


        // RectTransform 확인
        if (_rectTransform == null)
        {
            _rectTransform = _image.rectTransform;
        }


        // 이미 실행 중이면 중지
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }


        // 애니메이션 시작
        _animationCoroutine =
            StartCoroutine(PlayAnimation());
    }


    // =========================
    // 전체 애니메이션
    // =========================

    private IEnumerator PlayAnimation()
    {
        // 이미지 표시
        _image.enabled = true;


        // 시작 크기
        _rectTransform.localScale =
            Vector3.one * _startScale;


        // =========================
        // 1단계
        // 이미지 + 확대
        // =========================

        float timer = 0f;
        int index = 0;


        while (timer < _scaleUpTime)
        {
            // 이미지 변경
            _image.sprite = _bombSprites[index];


            // 다음 이미지
            index++;

            if (index >= _bombSprites.Length)
            {
                index = 0;
            }


            // 확대 비율 계산
            float progress =
                timer / _scaleUpTime;


            // 0 → 1
            progress = Mathf.Clamp01(progress);


            // 시작 크기 → 최대 크기
            float currentScale =
                Mathf.Lerp(
                    _startScale,
                    _maxScale,
                    progress
                );


            // 크기 적용
            _rectTransform.localScale =
                Vector3.one * currentScale;


            // 다음 프레임까지 대기
            yield return new WaitForSeconds(
                _frameTime
            );


            timer += _frameTime;
        }


        // 최대 크기
        _rectTransform.localScale =
            Vector3.one * _maxScale;


        // =========================
        // 2단계
        // 다시 축소
        // =========================

        timer = 0f;


        while (timer < _scaleDownTime)
        {
            // 이미지 변경
            _image.sprite = _bombSprites[index];


            // 다음 이미지
            index++;

            if (index >= _bombSprites.Length)
            {
                index = 0;
            }


            // 축소 비율 계산
            float progress =
                timer / _scaleDownTime;


            // 0 → 1
            progress = Mathf.Clamp01(progress);


            // 최대 크기 → 종료 크기
            float currentScale =
                Mathf.Lerp(
                    _maxScale,
                    _endScale,
                    progress
                );


            // 크기 적용
            _rectTransform.localScale =
                Vector3.one * currentScale;


            // 다음 프레임까지 대기
            yield return new WaitForSeconds(
                _frameTime
            );


            timer += _frameTime;
        }


        // 종료 크기
        _rectTransform.localScale =
            Vector3.one * _endScale;


        // =========================
        // 3단계
        // 남은 시간 동안 이미지 유지
        // =========================

        float remainingTime =
            _duration -
            _scaleUpTime -
            _scaleDownTime;


        if (remainingTime > 0f)
        {
            timer = 0f;


            while (timer < remainingTime)
            {
                // 이미지 변경
                _image.sprite =
                    _bombSprites[index];


                // 다음 이미지
                index++;

                if (index >= _bombSprites.Length)
                {
                    index = 0;
                }


                // 대기
                yield return new WaitForSeconds(
                    _frameTime
                );


                timer += _frameTime;
            }
        }


        // =========================
        // 애니메이션 종료
        // =========================

        _image.enabled = false;


        // 크기 원상복구
        _rectTransform.localScale =
            Vector3.one;


        _animationCoroutine = null;
    }
}