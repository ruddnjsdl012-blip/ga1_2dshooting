using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUIEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private float _normalScale = 1f;
    [SerializeField] private float _maxScale = 1.5f;

    [SerializeField] private float _scaleUpDuration = 0.1f;
    [SerializeField] private float _scaleDownDuration = 0.2f;

    private Coroutine _effectCoroutine;

    private void Start()
    {
        if (_scoreText != null)
        {
            _scoreText.transform.localScale =
                Vector3.one * _normalScale;
        }
    }

    public void PlayScoreEffect()
    {
        if (_scoreText == null)
        {
            Debug.LogWarning("ScoreUIEffect: Score Text가 연결되지 않았습니다.");
            return;
        }

        if (_effectCoroutine != null)
        {
            StopCoroutine(_effectCoroutine);
        }

        _effectCoroutine = StartCoroutine(ScoreEffect());
    }

    private IEnumerator ScoreEffect()
    {
        // 커짐
        float timer = 0f;

        while (timer < _scaleUpDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _scaleUpDuration;

            float scale = Mathf.Lerp(
                _normalScale,
                _maxScale,
                progress
            );

            _scoreText.transform.localScale =
                Vector3.one * scale;

            yield return null;
        }

        // 작아짐
        timer = 0f;

        while (timer < _scaleDownDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _scaleDownDuration;

            float scale = Mathf.Lerp(
                _maxScale,
                _normalScale,
                progress
            );

            _scoreText.transform.localScale =
                Vector3.one * scale;

            yield return null;
        }

        _scoreText.transform.localScale =
            Vector3.one * _normalScale;

        _effectCoroutine = null;
    }
}