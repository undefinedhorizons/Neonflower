using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerGun : Gun
{
    [SerializeField] public int maxBullets = 5;
    private int _bullets;
    [SerializeField] public float reloadTime = 2f;

    private void Awake()
    {
        _bullets = maxBullets;
        // _projectilePool = new ObjectPool<GameObject>()
    }


    public new void Shoot(Vector2 direction)
    {
        if (_bullets == 0)
        {
            return;
        }

        base.Shoot(direction);
        --_bullets;
        GameManager.Instance.UpdateGunUI(_bullets);

        // if (_bullets == 0)
        // {
        //     StartCoroutine(Reloading());
        // }
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        _bullets = maxBullets;
        GameManager.Instance.UpdateGunUI(_bullets);
    }

    public int GetMaxBullets()
    {
        return maxBullets;
    }

    public void SetMaxBullets(int bullets)
    {
        maxBullets = bullets;
        RefillAmmo();
    }
    
    public void RefillAmmo()
    {
        _bullets = maxBullets;
        GameManager.Instance.UpdateGunUI(_bullets);
    }
}