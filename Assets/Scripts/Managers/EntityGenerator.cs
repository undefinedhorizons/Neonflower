using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Linq;

public class EntityGenerator : MonoBehaviour
{
    [FormerlySerializedAs("enemies")] [SerializeField]
    private GameObject[] entities;

    [SerializeField] private GameObject[] fillerEntities;
    private const int SectionSize = 40;


    [SerializeField] private float lowerBound = 0.5f;
    [SerializeField] private float upperBound = 0.5f;
    [SerializeField] private int entitiesPerSection = 5;
    [SerializeField] private int section = 1;
    [SerializeField] private int[] difficultyIncreaseSections;
    private int _difficultyIncreaseSection;
    private int _difficultyIndex;
    private List<GameObject> _prevSectionEnemies;
    private List<GameObject> _curSectionEnemies;
    private List<GameObject> _nextSectionEnemies;
    private List<GameObject> _prevPrevSectionEnemies;

    private GameObject CreateEnemyInBounds(float lower, float upper)
    {
        var pos = Random.Range(lower, upper);
        var enemyPrefab = GetNextObject();
        var spawnPosition = new Vector3(enemyPrefab.transform.position.x, pos, 0);
        var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        return enemy;
    }

    private GameObject GetNextObject()
    {
        var chance = Random.Range(0, 1f);
        var curChance = .0f;
        foreach (var entity in entities)
        {
            curChance += entity.GetComponent<Chance>().GetChance(section);
            if (curChance >= chance)
            {
                return entity;
            }
        }

        var entityType = Random.Range(0, fillerEntities.Length);
        return fillerEntities[entityType];
    }

    private void Awake()
    {
        // _nextSectionEnemies = new List<GameObject>();
        _curSectionEnemies = new List<GameObject>();
        _prevSectionEnemies = new List<GameObject>();
        _prevPrevSectionEnemies = new List<GameObject>();

        _difficultyIncreaseSection = difficultyIncreaseSections[0];
    }

    private void Start()
    {
        LogTotalEntityChance();
    }

    private void LogTotalEntityChance()
    {
        var totalChance = entities.Sum(entity => entity.GetComponent<Chance>().GetChance());
        Debug.Log("Total entity chance is " + totalChance);
    }

    // private GameObject[] Clone(IReadOnlyList<GameObject> arrayToClone)
    // {
    //     var a = new GameObject[enemiesPerSection];
    //     for (var i = 0; i < enemiesPerSection; ++i)
    //     {
    //         a[i] = arrayToClone[i];
    //     }
    //
    //     return a;
    // }

    private void UpdateEnemyStorage()
    {
        if (_prevSectionEnemies is not null)
        {
            //Array.Copy(_prevSectionEnemies, _prevPrevSectionEnemies, entitiesPerSection);
            // Debug.Log("_prevSectionEnemies updated");
            _prevPrevSectionEnemies = _prevSectionEnemies.ToList();
        }

        if (_curSectionEnemies is not null)
        {
            _prevSectionEnemies = _curSectionEnemies.ToList();
            // Array.Copy(_curSectionEnemies, _prevSectionEnemies, entitiesPerSection);
        }

        if (_nextSectionEnemies is not null)
            _curSectionEnemies = _nextSectionEnemies.ToList();
        // Array.Copy(_nextSectionEnemies, _curSectionEnemies, entitiesPerSection);
    }

    public void CreateFirstSection()
    {
        var lower = 0;
        var upper = lower + SectionSize / entitiesPerSection;
        CreateSection(lower, upper);
    }

    private void DisablePreviousEnemies()
    {
        if (_prevPrevSectionEnemies.Any())
        {
            Debug.Log("Enemies to disable:" + _prevSectionEnemies.Count + " from section: " + (section - 2));
            foreach (var e in _prevPrevSectionEnemies)
            {
                e.SetActive(false);
            }
        }
    }

    private void CreateSection(int lower, int upper)
    {
        UpdateEnemyStorage();
        entitiesPerSection = GetEntitiesPerSection();
        _nextSectionEnemies = new List<GameObject>(entitiesPerSection);
        for (var i = 0; i < entitiesPerSection; ++i)
        {
            var enemy = CreateEnemyInBounds(lower + lowerBound, upper - upperBound);
            // Debug.Log(_nextSectionEnemies.Capacity + " " + i);
            _nextSectionEnemies.Add(enemy);
            lower = upper;
            upper += SectionSize / entitiesPerSection;
        }

        DisablePreviousEnemies();
    }

    private int GetEntitiesPerSection()
    {
        if (_difficultyIndex == difficultyIncreaseSections.Length)
        {
            return entitiesPerSection;
        }

        var difficultyIncreaseSection = difficultyIncreaseSections[_difficultyIndex];
        if (difficultyIncreaseSection == section)
        {
            entitiesPerSection++;
            _difficultyIndex++;
        }

        return entitiesPerSection;
    }

    public void CreateNextSection()
    {
        section += 1;
        var lower = SectionSize * (section - 1);
        var upper = lower + SectionSize / entitiesPerSection;
        CreateSection(lower, upper);
    }
}