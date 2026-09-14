using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _Titletext;

    [SerializeField] private TextMeshProUGUI _valueText;

    [SerializeField] private TextMeshProUGUI _socreCostText;


    // 0 = 공격력
    // 1 = 이동속도
    // 2 = 발사속도

    [SerializeField] private int _index;


    private Vector3 _originalScale;

    private Coroutine _scaleCoroutine;


    private void Start()
    {
        _originalScale = transform.localScale;

        Refresh();
    }


    public void OnClick()
    {
        bool success =
            UpgradeManager.Instance.LevelUp(_index);


        if (success)
        {
            Refresh();

            PlayPunchEffect();
        }
    }


    public void Refresh()
    {
        Upgrade upgrade =
            UpgradeManager.Instance.GetUpgrade(_index);


        if (upgrade == null)
        {
            return;
        }


        _Titletext.text =
            upgrade.Name;


        _valueText.text =
            $"{upgrade.CurrentValue} → {upgrade.NextValue}";


        _socreCostText.text =
            $"{upgrade.Cost}";
    }


    private void PlayPunchEffect()
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }


        _scaleCoroutine =
            StartCoroutine(PunchScale());
    }


    private IEnumerator PunchScale()
    {
        float duration = 0.1f;

        float timer = 0f;


        Vector3 targetScale =
            _originalScale * 1.15f;


        // 커지기
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t =
                timer / duration;

            transform.localScale =
                Vector3.Lerp(
                    _originalScale,
                    targetScale,
                    t
                );

            yield return null;
        }


        timer = 0f;


        // 작아지기
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t =
                timer / duration;

            transform.localScale =
                Vector3.Lerp(
                    targetScale,
                    _originalScale,
                    t
                );

            yield return null;
        }


        transform.localScale =
            _originalScale;


        _scaleCoroutine = null;
    }
}