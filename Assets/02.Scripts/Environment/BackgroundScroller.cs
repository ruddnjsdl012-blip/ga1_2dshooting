using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Transform _background1;
    [SerializeField] private Transform _background2;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _backgroundHeight = 10f;

    private void Update()
    {
        float moveAmount = _moveSpeed * Time.deltaTime;

        _background1.position += Vector3.down * moveAmount;
        _background2.position += Vector3.down * moveAmount;

        if (_background1.position.y <= -_backgroundHeight)
        {
            _background1.position = _background2.position + Vector3.up * _backgroundHeight;
        }

        if (_background2.position.y <= -_backgroundHeight)
        {
            _background2.position = _background1.position + Vector3.up * _backgroundHeight;
        }
    }
}