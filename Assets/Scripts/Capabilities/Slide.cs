using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Slide : MonoBehaviour
{
    private enum Wall
    {
        Left,
        Right
    }

    [SerializeField] private Transform leftWallTransform;
    [SerializeField] private Transform rightWallTransform;
    [SerializeField] private float speed = 2f;
    [SerializeField] private double swipeDirectionMinY = -0.2;
    [SerializeField] private float maxTimeInSlide = .5f;

    [SerializeField] private GameObject playerModel;
    private Vector3 _target;
    private Wall _wall = Wall.Left;
    private const float MaxDistance = 0.05f;
    private SwipeDetection _swipeDetection;
    private bool _isSliding = false;
    private bool _isFalling = false;
    private SpriteRenderer _spriteRenderer;
    private float _timeInSlide = 0;
    private TrailRenderer _trail;
    private Rigidbody2D _rb;
    private const float MathEps = 0.001f;
    private const float MaxSameWallX = 0.9f;
    private readonly Vector2 _directionIfXEqualsZero = new(0.01f, .99995f);

    [SerializeField] private float maxSlideEnergy = 4f;
    private float _slideEnergy = 4f;
    [SerializeField] private float slideRestorationSpeed = 0.01f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trail = GetComponent<TrailRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }


    public void StartSlide(Vector2 direction)
    {
        if (_slideEnergy < 1)
        {
            direction = _wall == Wall.Left ? Vector2.right : Vector2.left;
        }

        if (_isSliding || _isFalling)
        {
            return;
        }

        if (direction.y <= swipeDirectionMinY)
        {
            return;
        }

        direction.y = Math.Max(0, direction.y);

        if (IsWrongDirection(direction))
        {
            return;
        }

        direction.y = Mathf.Abs(direction.y);

        _wall = _wall == Wall.Left ? Wall.Right : Wall.Left;
        var wallPos = _wall == Wall.Left ? leftWallTransform.position : rightWallTransform.position;
        var characterPos = transform.position;
        _target.x = wallPos.x;
        var distBetweenWalls = rightWallTransform.position.x - leftWallTransform.position.x;

        if (Math.Abs(direction.x) < MathEps)
        {
            direction = _directionIfXEqualsZero;
        }

        // Debug.Log("Final Direction " + direction);
        _target.y = characterPos.y + Mathf.Abs(direction.y / direction.x * (distBetweenWalls));
        _target.z = characterPos.z;

        _isSliding = true;
        DisableSprite();
        StartCoroutine(DoSlide());
    }

    public float GetSliderEnergy()
    {
        return _slideEnergy;
    }

    private bool IsWrongDirection(Vector2 direction)
    {
        if (Math.Abs(direction.x) > MaxSameWallX)
        {
            if (direction.x >= 0 && _wall == Wall.Right)
            {
                return true;
            }

            if (direction.x < 0 && _wall == Wall.Left)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if (!_isSliding && _slideEnergy < maxSlideEnergy)
            _slideEnergy += Time.deltaTime * slideRestorationSpeed;
    }

    public void AddEnergyBar()
    {
        // _slideEnergy = Mathf.Min(_slideEnergy + 1f, maxSlideEnergy);
        _slideEnergy = maxSlideEnergy;
    }

    private IEnumerator DoSlide()
    {
        if (_slideEnergy > 1)
            _slideEnergy -= 1;

        while (!HasArrived())
        {
            if (_timeInSlide > maxTimeInSlide)
            {
                StartCoroutine(DoFall());
                break;
            }

            transform.position = Vector2.MoveTowards(transform.position, _target,
                Time.deltaTime * speed);
            _timeInSlide += Time.deltaTime;
            yield return null;
        }

        // Debug.Log("Time in slide " + _timeInSlide);
        _timeInSlide = 0;
        _isSliding = false;
        FlipPlayerModel();
        if (!_isFalling)
        {
            DisableTrail();
            EnableSprite();
        }
    }


    private void FlipPlayerModel()
    {
        var rotation1 = playerModel.transform.rotation;
        var angles = rotation1.eulerAngles;
        angles.y *= -1;
        var rotation = rotation1;
        rotation.eulerAngles = angles;
        rotation1 = rotation;
        playerModel.transform.rotation = rotation1;

        var vector3 = playerModel.transform.position;
        vector3.x = _wall == Wall.Left
            ? playerModel.transform.position.x - 1.15f
            : playerModel.transform.position.x + 1.15f;
        playerModel.transform.position = vector3;
    }

    private void StartFalling()
    {
        var velocity = new Vector2
        {
            x = speed * Mathf.Sign((_target - transform.position).x) / 2,
            y = 10
        };

        _rb.velocity = velocity;
        _rb.gravityScale = 10;
        _isFalling = true;
        
    }

    private void StopFalling()
    {
        _isFalling = false;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;
        DisableTrail();
        EnableSprite();
    }

    private IEnumerator DoFall()
    {
        StartFalling();
        while (!IsOnWall())
        {
            yield return null;
        }

        StopFalling();
    }

    public bool IsSliding()
    {
        return _isSliding;
    }

    public bool IsFalling()
    {
        return _isFalling;
    }

    private void EnableTrail()
    {
        // _trail.enabled = true;
        _trail.emitting = true;
    }

    private void DisableTrail()
    {
        //_trail.enabled = false;
        _trail.emitting = false;
    }

    private void DisableSprite()
    {
        // _spriteRenderer.enabled = 
        EnableTrail();

        playerModel.SetActive(false);
    }

    private void EnableSprite()
    {
        // _spriteRenderer.enabled = true;

        // DisableTrail();

        playerModel.SetActive(true);
    }


    private bool IsOnWall()
    {
        var dist = Math.Abs(_target.x - transform.position.x);
        return dist < MaxDistance;
    }

    private bool HasArrived()
    {
        return HasArrived(_target);
    }

    private bool HasArrived(Vector3 target)
    {
        var dist = Vector2.Distance(_target, transform.position);
        return dist < MaxDistance;
    }
}