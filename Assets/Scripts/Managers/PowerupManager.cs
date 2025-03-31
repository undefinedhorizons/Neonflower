using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum Parameter
{
    BulletsInShot,
    BulletSpeed,

    // Knockback,
    NumOfShots,
    BulletSize,
    MaxBullets,

    // ReloadTime
    // Ricochet,
    // Explosion
}

public class PowerupEffect
{
    public Parameter Parameter = 0;
    public float Value;
}

public class PowerupData
{
    public PowerupEffect Positive = new();
    public PowerupEffect Negative = new();

    public bool Equals(PowerupData powerup)
    {
        return powerup.Positive.Parameter == Positive.Parameter && powerup.Negative.Parameter == Negative.Parameter;
    }
}

[DefaultExecutionOrder((-1))]
public class PowerupManager : MonoBehaviour
{
    private PowerupUI _powerupUI;
    private int _bullets;
    private float _bulletSpeed;
    private float _knockback;
    private float _numOfShots;
    private float _bulletSize;
    private UnsafeHashSet<int> _availableNegEffects;
    private UnsafeHashMap<int, float> _minParameters;
    private PowerupData _powerupOne = new();
    private PowerupData _powerupTwo = new();
    private PlayerGun _gun;

    private void Awake()
    {
        _powerupUI = FindObjectOfType<PowerupUI>(true);
        _gun = GameManager.Instance.GetPlayer().GetComponent<PlayerGun>();

        Instance = this;

        var numOfParams = Enum.GetValues(typeof(Parameter)).Length;
        _availableNegEffects = new UnsafeHashSet<int>(numOfParams, Allocator.Persistent);
        _minParameters = new UnsafeHashMap<int, float>(numOfParams, Allocator.Persistent);
        _minParameters.TryAdd((int)Parameter.BulletsInShot, 1);
        _minParameters.TryAdd((int)Parameter.BulletSpeed, 20);
        _minParameters.TryAdd((int)Parameter.NumOfShots, 1);
        _minParameters.TryAdd((int)Parameter.BulletSize, .2f);
        _minParameters.TryAdd((int)Parameter.MaxBullets, 1);
    }

    public static PowerupManager Instance { get; private set; }

    public void ChoosePowerup()
    {
        Time.timeScale = 0;
        UpdateAvailableNegEffects();
        CreatePowerups();
        // Debug.Log(_powerupOne.Positive.Parameter + " " + _powerupOne.Positive.Value);
        _powerupUI.Setup(_powerupOne, _powerupTwo);
    }

    public void ChoosePowerupOne()
    {
        ApplyPowerup(_powerupOne);
    }


    public void ChoosePowerupTwo()
    {
        ApplyPowerup(_powerupTwo);
    }

    public void SkipPowerup()
    {
        ClosePowerupUI();
    }

    public void ClosePowerupUI()
    {
        Time.timeScale = 1;
        _powerupUI.gameObject.SetActive(false);
    }

    private void ApplyPowerup(PowerupData powerupData)
    {
        ClosePowerupUI();
        ApplyPowerupEffect(powerupData.Positive);
        ApplyPowerupEffect(powerupData.Negative);

        UpdateAvailableNegEffects();
    }

    private void UpdateAvailableNegEffects()
    {
        foreach (Parameter effect in Enum.GetValues(typeof(Parameter)))
        {
            if (IsEffectAvailable(effect))
            {
                _availableNegEffects.Add((int)effect);
            }
        }
    }

    private bool IsEffectAvailable(Parameter effect)
    {
        return effect switch
        {
            Parameter.BulletsInShot => _gun.bulletsInShot > _minParameters[(int)effect],
            Parameter.BulletSpeed => _gun.bulletSpeed > _minParameters[(int)effect],
            Parameter.NumOfShots => _gun.numOfShots > _minParameters[(int)effect],
            Parameter.BulletSize => _gun.bulletSize > _minParameters[(int)effect],
            Parameter.MaxBullets => _gun.maxBullets > _minParameters[(int)effect],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ApplyPowerupEffect(PowerupEffect powerup)
    {
        switch (powerup.Parameter)
        {
            case Parameter.BulletsInShot:
                _gun.bulletsInShot += (int)powerup.Value;
                break;
            case Parameter.BulletSpeed:
                _gun.bulletSpeed += powerup.Value;
                break;
            case Parameter.NumOfShots:
                _gun.numOfShots += (int)powerup.Value;
                break;
            case Parameter.BulletSize:
                _gun.bulletSize += powerup.Value;
                break;
            case Parameter.MaxBullets:
                _gun.SetMaxBullets(_gun.maxBullets + (int)powerup.Value);
                break;
            // case Parameter.ReloadTime:
            //     if (_gun.reloadTime > 0.4)
            //         _gun.reloadTime -= 0.4f;
            //     break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private void CreatePowerups()
    {
        CreatePowerup(_powerupOne);
        CreatePowerup(_powerupTwo);
        
        if (_powerupOne.Equals(_powerupTwo))
        {
            MakeDifferent();
        }
    }

    private void MakeDifferent()
    {
        var isPositive = Random.Range(0, 2);

        if (isPositive == 1)
        {
            var available = new HashSet<Parameter>((Parameter[])Enum.GetValues(typeof(Parameter)));
            available.Remove(_powerupOne.Positive.Parameter);
            var availableArray = available.ToArray();
            var index = Random.Range(0, availableArray.Length);
            _powerupTwo.Positive.Parameter = availableArray[index];
            CreatePositiveEffect(_powerupTwo.Positive);
        }
        else
        {
            _availableNegEffects.Remove((int) _powerupTwo.Negative.Parameter);
            var availableArray = _availableNegEffects.ToNativeArray(Allocator.Temp);
            var index = Random.Range(0, availableArray.Length);
            _powerupTwo.Negative.Parameter = (Parameter)availableArray[index];
            CreateNegativeEffect(_powerupTwo.Negative);
        }
    }

    private void CreatePowerup(PowerupData powerupData)
    {
        powerupData.Positive.Parameter = (Parameter)Random.Range(0, Enum.GetValues(typeof(Parameter)).Length);
        _availableNegEffects.Remove((int)powerupData.Positive.Parameter);

        var negEffects = _availableNegEffects.ToNativeArray(Allocator.Temp);
        var index = Random.Range(0, negEffects.Length);
        powerupData.Negative.Parameter = (Parameter)negEffects[index];

        CreatePositiveEffect(powerupData.Positive);
        CreateNegativeEffect(powerupData.Negative);
    }

    private static void CreatePositiveEffect(PowerupEffect powerupEffect)
    {
        powerupEffect.Value = powerupEffect.Parameter switch
        {
            Parameter.BulletsInShot => 2,
            Parameter.BulletSpeed => 25,
            Parameter.NumOfShots => 1,
            Parameter.BulletSize => 0.1f,
            Parameter.MaxBullets => 2,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static void CreateNegativeEffect(PowerupEffect powerupEffect)
    {
        powerupEffect.Value = powerupEffect.Parameter switch
        {
            Parameter.BulletsInShot => -2,
            Parameter.BulletSpeed => -10,
            Parameter.NumOfShots => -1,
            Parameter.BulletSize => -0.05f,
            Parameter.MaxBullets => -1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}