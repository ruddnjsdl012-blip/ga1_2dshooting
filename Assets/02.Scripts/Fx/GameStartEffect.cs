using System.Collections;
using TMPro;
using UnityEngine;

public class GameStartEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameStartText;
    [SerializeField] private AudioSource _audioSource;

    [Header("등장")]
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _startScale = 0.5f;
    [SerializeField] private float _maxScale = 1.2f;

    [Header("깜빡임")]
    [SerializeField] private float _blinkDuration = 1.5f;
    [SerializeField] private float _blinkSpeed = 0.15f;

    [Header("사라짐")]
    [SerializeField] private float _fadeOutDuration = 1f;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = _gameStartText.transform.localScale;

        // 효과음 재생
        if (_audioSource != null)
        {
            _audioSource.Play();
        }

        StartCoroutine(GameStartEffectCoroutine());
    }

    private IEnumerator GameStartEffectCoroutine()
    {
        SetAlpha(0f);

        _gameStartText.transform.localScale =
            _originalScale * _startScale;

        // 등장
        yield return StartCoroutine(ScaleIn());

        // 깜빡임
        yield return StartCoroutine(Blink());

        // 사라짐
        yield return StartCoroutine(FadeOut());

        _gameStartText.gameObject.SetActive(false);
    }

    private IEnumerator ScaleIn()
    {
        float timer = 0f;

        while (timer < _fadeInDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeInDuration;

            SetAlpha(progress);

            float scale = Mathf.Lerp(
                _startScale,
                _maxScale,
                progress
            );

            _gameStartText.transform.localScale =
                _originalScale * scale;

            yield return null;
        }

        SetAlpha(1f);

        _gameStartText.transform.localScale =
            _originalScale * _maxScale;
    }

    private IEnumerator Blink()
    {
        float timer = 0f;
        bool visible = true;

        while (timer < _blinkDuration)
        {
            visible = !visible;

            SetAlpha(visible ? 1f : 0f);

            yield return new WaitForSeconds(_blinkSpeed);

            timer += _blinkSpeed;
        }

        SetAlpha(1f);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < _fadeOutDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeOutDuration;

            float alpha = 1f - progress;
            SetAlpha(alpha);

            float scale = Mathf.Lerp(
                _maxScale,
                1f,
                progress
            );

            _gameStartText.transform.localScale =
                _originalScale * scale;

            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _gameStartText.color;
        color.a = alpha;

        _gameStartText.color = color;
    }
}