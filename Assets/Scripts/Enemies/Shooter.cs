using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float cooldown = 2.1f;
    [SerializeField] private float initialWaitTime = 0.2f;
    private GameObject _player;
    [SerializeField] private float targetShift;
    private Gun _gun;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        var gameManager = GameManager.Instance;
        _gun.SetProjectiles(gameManager.GetEnemyBullets());
        _player = gameManager.GetPlayer();
    }

    void Start()
    {
        StartCoroutine(ShooterAI());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator ShooterAI()
    {
        yield return new WaitForSeconds(initialWaitTime);
        while (enabled)
        {
            var target = _player.transform.position;
            if (target.y < transform.position.y)
            {
                target.y += targetShift;
                _gun.Shoot((target - transform.position).normalized);
            }

            yield return new WaitForSeconds(cooldown);
        }
    }
}