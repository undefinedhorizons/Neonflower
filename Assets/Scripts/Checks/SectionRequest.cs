using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionRequest : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Checking if this is a player!");
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Generate next section!");
            _gameManager.CreateNextSection();
            Deactivate();
        }
    }
}