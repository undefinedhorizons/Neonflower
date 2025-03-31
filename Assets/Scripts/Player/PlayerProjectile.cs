using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Projectile _projectile;
    private GameManager _gameManager;

    private void Awake()
    {
        _projectile = GetComponent<Projectile>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Enemy killed!");
            _gameManager.AddPoints(collision.gameObject.GetComponent<Enemy>().points);
            collision.gameObject.SetActive(false);
            _projectile.Deactivate();
        }
    }
}