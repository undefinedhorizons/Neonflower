using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[DefaultExecutionOrder((-1))]
public class GameManager : MonoBehaviour
{
    private bool _isGameEnded = false;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private GameObject wallsPrefab;
    private GameScore _gameScore;
    private GameObject _player;
    private GunUI _gunUI;
    private EntityGenerator _entityGenerator;
    private int _currentSection = 1;
    private const int SectionSize = 40;
    [SerializeField] private GameObject[] enemyProjectiles;
    private PowerupUI _powerupUI;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        _entityGenerator = FindObjectOfType<EntityGenerator>();
        _player = FindObjectOfType<Slide>().gameObject;
        _gameScore = FindObjectOfType<GameScore>();
        _gunUI = FindObjectOfType<GunUI>();
        
        Instance = this;

        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 90;
    }

    private void Start()
    {
        _entityGenerator.CreateFirstSection();
    }

    private void FixedUpdate()
    {
        // _gameScore.AddPoints(1);
    }

    public void EndGame()
    {
        if (_isGameEnded)
            return;
        _isGameEnded = true;

        _gameScore.UpdateHighscore();
        gameOverScreen.Setup(_gameScore.GetScore(), _gameScore.GetHighscore());
    }


    public void CreateNextSection()
    {
        var spawnPosition = new Vector3(0, SectionSize * _currentSection, 0);
        _currentSection += 1;
        _entityGenerator?.CreateNextSection();
        Instantiate(wallsPrefab, spawnPosition, Quaternion.identity);
    }

    public GameObject[] GetEnemyBullets()
    {
        return enemyProjectiles;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public void AddPoints(int points)
    {
        _gameScore.AddPoints(points);
    }

    public void UpdateGunUI(int bullets)
    {
        _gunUI.SetCurrentBullets(bullets);
    }

    public bool IsGameStopped()
    {
        return Time.timeScale == 0;
    }
}