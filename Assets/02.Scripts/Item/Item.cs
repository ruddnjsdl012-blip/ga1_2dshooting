using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private float _value;

    [Header("아이템 회전 이미지 7장")]
    [SerializeField] private Sprite[] _rotationSprites;

    [Header("이미지 변경 속도")]
    [SerializeField] private float _animationSpeed = 0.1f;

    private float _animationTimer = 0f;
    private int _currentSpriteIndex = 0;

    private const float WaitTime = 2f;
    private float _waitTimer = 0f;

    private const float MoveSpeed = 5f;

    private Player _player = null;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _player = null;

        _spriteRenderer =
            GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        _waitTimer = 0f;

        _animationTimer = 0f;

        _currentSpriteIndex = 0;

        UpdateSprite();

        if (_player == null)
        {
            GameObject playerObject =
                GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                _player =
                    playerObject.GetComponent<Player>();
            }
        }
    }


    private void Update()
    {
        // 7장의 이미지를 순서대로 변경
        UpdateAnimation();

        // 2초 대기
        _waitTimer += Time.deltaTime;

        if (_waitTimer < WaitTime)
        {
            return;
        }

        // 2초 후 플레이어 추적
        FollowPlayer();
    }


    private void UpdateAnimation()
    {
        if (_rotationSprites == null ||
            _rotationSprites.Length == 0)
        {
            return;
        }

        if (_spriteRenderer == null)
        {
            return;
        }

        _animationTimer += Time.deltaTime;

        if (_animationTimer < _animationSpeed)
        {
            return;
        }

        _animationTimer = 0f;

        _currentSpriteIndex++;

        if (_currentSpriteIndex >=
            _rotationSprites.Length)
        {
            _currentSpriteIndex = 0;
        }

        UpdateSprite();
    }


    private void UpdateSprite()
    {
        if (_rotationSprites == null ||
            _rotationSprites.Length == 0)
        {
            return;
        }

        if (_spriteRenderer == null)
        {
            return;
        }

        _spriteRenderer.sprite =
            _rotationSprites[_currentSpriteIndex];
    }


    private void FollowPlayer()
    {
        if (_player == null)
        {
            return;
        }

        Vector3 direction =
            (
                _player.transform.position
                - transform.position
            ).normalized;

        transform.position +=
            direction
            * MoveSpeed
            * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Player player =
            other.GetComponent<Player>();

        if (player == null)
        {
            return;
        }

        ApplyItemEffect(player);

        gameObject.SetActive(false);
    }


    private void ApplyItemEffect(Player player)
    {
        switch (_type)
        {
            case ItemType.Heal:

                player.Heal((int)_value);

                Debug.Log(
                    $"회복 아이템 획득! +{_value}"
                );

                break;


            case ItemType.MoveSeepUp:

                Debug.Log(
                    $"이동 속도 증가 아이템 획득! +{_value}"
                );

                break;


            case ItemType.FireRateUp:

                Debug.Log(
                    $"발사 속도 증가 아이템 획득! +{_value}"
                );

                break;


            default:

                Debug.LogWarning(
                    $"알 수 없는 ItemType입니다. : {_type}"
                );

                break;
        }
    }
}