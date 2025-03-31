using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float bulletSpeed = 100;
    [SerializeField] private float angle = .65f;
    [SerializeField] private int numOfBullets = 3;
    // [SerializeField] private float cooldown = 2f;

    public void ShootProjectile(Vector2 direction)
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
        projectiles[projectileNumber].GetComponent<Projectile>().ShootAt(direction, bulletSpeed);
    }

    public void Shoot(Vector2 direction)
    {
        var curAngle = Vector2.SignedAngle(direction, Vector2.right)  * Mathf.PI / 180;
        curAngle = Random.Range(curAngle - angle, curAngle + angle);
        var curDirection = new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
        ShootProjectile(curDirection);
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
}