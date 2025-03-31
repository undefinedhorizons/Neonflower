using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float bulletSpeed = 100;
    // [SerializeField] private float cooldown = 2f;

    public void ShootProjectile(Vector2 direction)
    {
        var projectileNumber = FindProjectile();

        if (projectileNumber == -1)
        {
            return;
        }

        // projectiles[projectileNumber].transform.position = transform.position;
        // projectiles[projectileNumber].transform.position += new Vector3(0, 5);
        // Debug.Log("ShootProjectile" + direction);
        // direction.y *= -1;
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
