using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    private const double Tolerance = 0.0001f;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] public float bulletSpeed = 100;
    [SerializeField] private float shotgunAngle = 90f; // angle in degrees

    [SerializeField] public int bulletsInShot = 1;

    [SerializeField] public int numOfShots = 1;
    [SerializeField] public float bulletSize = .4f;
    [SerializeField] public float timeBetweenBursts = .1f;
    protected ObjectPool<GameObject> _projectilePool;
    [SerializeField] protected GameObject projectilePrefab;

    private bool _isShooting = false;
    // [SerializeField] private float cooldown = 2f;

    public void Shoot(Vector2 direction)
    {
        if (!_isShooting)
        {
            StartCoroutine(Shooting(direction));
        }

        // var a = Vector2.SignedAngle(direction, Vector2.right);
        // if (gameObject.CompareTag("Player"))
        // {
        //     Debug.Log("Angle: " + a  + " Plus " + (a + 90) + " Minus " + (a - 90));
        // }
        //
        // var d = new Vector2(Mathf.Cos((a + shotgunAngle) * Mathf.Deg2Rad),
        //     -Mathf.Sin((a + shotgunAngle) * Mathf.Deg2Rad));
        // ShootProjectile(d);
        // d = new Vector2(Mathf.Cos((a - shotgunAngle) * Mathf.Deg2Rad),
        //     -Mathf.Sin((a - shotgunAngle) * Mathf.Deg2Rad));
        // ShootProjectile(d);
    }

    private IEnumerator Shooting(Vector2 direction)
    {
        _isShooting = true;
        ShootShotgun(direction);
        ShootProjectile(direction);
        for (var j = 0; j < numOfShots - 1; ++j)
        {
            yield return new WaitForSeconds(timeBetweenBursts);
            ShootShotgun(direction);
            ShootProjectile(direction);
        }

        _isShooting = false;
    }

    private void ShootShotgun(Vector2 direction)
    {
        var dirAngle = Vector2.SignedAngle(direction, Vector2.right) * Mathf.Deg2Rad;
        var angleRad = shotgunAngle * Mathf.Deg2Rad;
        var startAngle = dirAngle - angleRad;
        var endAngle = dirAngle + angleRad;
        float stepAngle;
        
        if (bulletsInShot > 2)
        {
            stepAngle = (endAngle - startAngle) / (bulletsInShot - 2);
        }
        else
        {
            stepAngle = (endAngle - startAngle);
        }
        
        var curAngle = startAngle;
        for (var i = 0; i < bulletsInShot - 1; ++i)
        {
            var curDirection = new Vector2(Mathf.Cos(curAngle), -Mathf.Sin(curAngle));
            
            // if (gameObject.CompareTag("Player"))
            // {
            //     Debug.Log("CurrentAngle" + curAngle);
            // }

            ShootProjectile(curDirection);
            curAngle += stepAngle;
            if (Math.Abs(curAngle - startAngle) < Tolerance)
            {
                curAngle += stepAngle;
            }
        }
    }


    private void ShootProjectile(Vector2 direction)
    {
        var projectileNumber = FindProjectile();

        if (projectileNumber == -1)
        {
            return;
        }

        projectiles[projectileNumber].transform.position = transform.position;
        // projectiles[projectileNumber].transform.position += new Vector3(0, 5);
        // Debug.Log("ShootProjectile" + direction);
        // direction.y *= -1;
        projectiles[projectileNumber].transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        projectiles[projectileNumber].GetComponent<Projectile>().ShootAt(direction, bulletSpeed);
    }

    private int FindProjectile()
    {
        for (var i = 0; i < projectiles.Length; ++i)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }

        return -1;
    }

    public void SetProjectiles(GameObject[] bullets)
    {
        projectiles = bullets;
    }
}