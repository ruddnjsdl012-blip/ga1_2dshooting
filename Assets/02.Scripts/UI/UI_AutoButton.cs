using UnityEngine;
using UnityEngine.UI;

public class UI_AutoButton : MonoBehaviour
{
    [Header("Auto 버튼 이미지")]
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    [Header("버튼 사운드")]
    [SerializeField] private AudioSource _audioSource;

    [Header("버튼 크기 애니메이션")]
    [SerializeField] private AnimationCurve _bumpCurve;
    [SerializeField] private float _bumpDuration = 0.3f;
    [SerializeField] private float _bumpScale = 1.1f;

    private Image _myImage;

    private bool _autoMode = false;

    private bool _isBumping = false;
    private float _elapsedTime = 0f;
    private Vector3 _originalScale;

    private Player _player;
    private PlayerFire _playerFire;
    private PlayerMove _playerMove;
    private PlayerAutoMove _playerAutoMove;

    private void Start()
    {
        _myImage = GetComponent<Image>();

        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        _originalScale = transform.localScale;

        _player = GameObject.FindAnyObjectByType<Player>();

        if (_player == null)
        {
            Debug.LogError(
                "UI_AutoButton : Player를 찾을 수 없습니다."
            );

            return;
        }

        _playerFire =
            _player.GetComponent<PlayerFire>();

        _playerMove =
            _player.GetComponent<PlayerMove>();

        _playerAutoMove =
            _player.GetComponent<PlayerAutoMove>();

        if (_playerFire == null)
        {
            Debug.LogError(
                "UI_AutoButton : Player에 PlayerFire가 없습니다."
            );
        }

        if (_playerMove == null)
        {
            Debug.LogError(
                "UI_AutoButton : Player에 PlayerMove가 없습니다."
            );
        }

        if (_playerAutoMove == null)
        {
            Debug.LogError(
                "UI_AutoButton : Player에 PlayerAutoMove가 없습니다."
            );
        }

        _autoMode = false;

        UpdateAutoMode();
    }

    private void Update()
    {
        if (!_isBumping)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _bumpDuration)
        {
            transform.localScale = _originalScale;

            _isBumping = false;

            return;
        }

        float time =
            _elapsedTime /
            _bumpDuration;

        float curveValue =
            _bumpCurve.Evaluate(time);

        float currentScale =
            Mathf.Lerp(
                1f,
                _bumpScale,
                curveValue
            );

        transform.localScale =
            _originalScale *
            currentScale;
    }

    public void AutoToggle()
    {
        _autoMode = !_autoMode;

        PlayAnimation();

        PlaySound();

        UpdateAutoMode();
    }

    private void PlayAnimation()
    {
        _isBumping = true;

        _elapsedTime = 0f;
    }

    private void PlaySound()
    {
        if (_audioSource == null)
        {
            return;
        }

        _audioSource.Play();
    }

    private void UpdateAutoMode()
    {
        if (_myImage != null)
        {
            if (_autoMode)
            {
                _myImage.sprite = _onSprite;
            }
            else
            {
                _myImage.sprite = _offSprite;
            }
        }

        if (_playerFire != null)
        {
            _playerFire.SetAuto(_autoMode);
        }

        if (_playerMove != null)
        {
            _playerMove.enabled = !_autoMode;
        }

        if (_playerAutoMove != null)
        {
            _playerAutoMove.enabled = _autoMode;
        }

        Debug.Log(
            "AUTO MODE : " +
            (_autoMode ? "ON" : "OFF")
        );
    }
}