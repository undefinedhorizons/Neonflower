using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumSwapTime = 0.12f;
    [SerializeField] private float maximumTapTime = 0.7f;

    private InputManager _inputManager;
    private Slide _slide;
    private PlayerGun _gun;
    private Camera _mainCamera;
    [SerializeField] private GameObject player;

    private Vector2 _startPosition;
    private float _startTime;
    private Vector2 _startCameraPos;

    private Vector2 _endPosition;
    private float _endTime;
    private Vector2 _endCameraPos;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _slide = player.GetComponent<Slide>();
        _gun = player.GetComponent<PlayerGun>();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
        _startCameraPos = _mainCamera.transform.position;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        _endCameraPos = _mainCamera.transform.position;
        // Debug.Log(_endCameraPos + " " + _startCameraPos);
        _endPosition.y -= (_endCameraPos - _startCameraPos).y;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (GameManager.Instance.IsGameStopped())
        {
            return;
        }

        var position = player.transform.position;

        Debug.Log("Start Position: " + _startPosition + " End Position: " + _endPosition);

        if (Vector3.Distance(_startPosition, _endPosition) <= minimumDistance &&
            (_endTime - _startTime) <= maximumTapTime)
        {
            // Debug.Log("Tap! Time:" + (_endTime - _startTime));
            // Debug.Log("Shoot: ");
            // Debug.Log((_endPosition - new Vector2(position.x, position.y)).normalized);
            // var shift = new Vector2(0, 5);
            _gun.Shoot((_endPosition - new Vector2(position.x, position.y)).normalized);
            return;
        }

        if (Vector3.Distance(_startPosition, _endPosition) >= minimumDistance
            && (_endTime - _startTime) <= maximumSwapTime)
        {
            //  Debug.Log("Time:" + (_endTime - _startTime));
            // Debug.Log("Slide: ");
            // Debug.Log((_endPosition - _startPosition).normalized);
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
            // Debug.Log((_endPosition - _startPosition).normalized);
            _slide.StartSlide((_endPosition - _startPosition).normalized);
        }
    }
}