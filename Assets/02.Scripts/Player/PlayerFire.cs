using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // =========================
    // 주총알
    // =========================

    [Header("주총알")]
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private Transform _bulletFirePointLeft;
    [SerializeField] private Transform _bulletFirePointRight;


    // =========================
    // 보조총알
    // =========================

    [Header("보조총알")]
    [SerializeField] private GameObject _capsuleBulletPrefab;

    [SerializeField] private Transform _capsuleFirePointLeft;
    [SerializeField] private Transform _capsuleFirePointRight;


    // =========================
    // 공격 설정
    // =========================

    [Header("공격 설정")]
    [SerializeField] private float _coolTime = 0.5f;

    [SerializeField] private bool _autoAttack = false;

    private float _lastFireTime = -1f;


    // =========================
    // 자동 / 수동 공격
    // =========================

    private void Update()
    {
        // 자동 공격
        if (_autoAttack)
        {
            if (Time.time >= _lastFireTime + _coolTime)
            {
                Shoot();
            }
        }

        // 수동 공격
        if (!_autoAttack && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }


    // =========================
    // 총알 발사
    // =========================

    private void Shoot()
    {
        // -------------------------
        // 주총알 왼쪽
        // -------------------------

        if (_bulletPrefab != null && _bulletFirePointLeft != null)
        {
            Instantiate(
                _bulletPrefab,
                _bulletFirePointLeft.position,
                _bulletFirePointLeft.rotation
            );
        }


        // -------------------------
        // 주총알 오른쪽
        // -------------------------

        if (_bulletPrefab != null && _bulletFirePointRight != null)
        {
            Instantiate(
                _bulletPrefab,
                _bulletFirePointRight.position,
                _bulletFirePointRight.rotation
            );
        }


        // -------------------------
        // 보조총알 왼쪽
        // -------------------------

        if (_capsuleBulletPrefab != null && _capsuleFirePointLeft != null)
        {
            Instantiate(
                _capsuleBulletPrefab,
                _capsuleFirePointLeft.position,
                _capsuleFirePointLeft.rotation
            );
        }


        // -------------------------
        // 보조총알 오른쪽
        // -------------------------

        if (_capsuleBulletPrefab != null && _capsuleFirePointRight != null)
        {
            Instantiate(
                _capsuleBulletPrefab,
                _capsuleFirePointRight.position,
                _capsuleFirePointRight.rotation
            );
        }


        // 마지막 발사 시간 저장
        _lastFireTime = Time.time;

        Debug.Log("주총알 2개 + 보조총알 2개 발사");
    }


    // =========================
    // 자동 공격 설정
    // =========================

    public void SetAuto(bool auto)
    {
        _autoAttack = auto;

        Debug.Log("자동 공격 : " + _autoAttack);
    }


    // =========================
    // 공격 속도 증가
    // =========================

    public void FireRateUp(float value)
    {
        _coolTime -= value;

        if (_coolTime < 0.05f)
        {
            _coolTime = 0.05f;
        }

        Debug.Log(
            "공격 속도 증가! 현재 쿨타임 : "
            + _coolTime
        );
    }
}