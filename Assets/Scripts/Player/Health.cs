using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Slide _slide;
    private GameManager _gameManager;
    [SerializeField] private bool isInGodMode = false;

    private void Awake()
    {
        _slide = GetComponent<Slide>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            // Debug.Log("Collision with enemy!");
            if (_slide.IsSliding())
            {
                var enemy = collision.gameObject;
                // Debug.Log("Enemy killed!");
                // if (enemy.GetComponent<Enemy>() is null)
                // {
                //     Debug.Log("Enemy is null at " + enemy.gameObject.transform.position + " " + collision.gameObject.name);
                // }

                _gameManager.AddPoints(enemy.GetComponent<Enemy>().points);
                collision.gameObject.SetActive(false);
                var player = _gameManager.GetPlayer();
                player.GetComponent<PlayerGun>().RefillAmmo();
                player.GetComponent<Slide>().AddEnergyBar();
                return;
            }

            // FindObjectOfType<GameManager>().EndGame();
            if (isInGodMode is false)
            {
                _gameManager.EndGame();
                transform.gameObject.SetActive(false);
            }
        }
    }
}