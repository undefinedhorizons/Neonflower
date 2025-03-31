using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    private TMP_Text _tmp;
    private int _score = 0;
    private int _highscore;
    private bool isGameEnded = false;

    // Start is called before the first frame update
    private void Start()
    {
        _tmp = GetComponent<TMP_Text>();
        _highscore = PlayerPrefs.GetInt("Highscore");
        PlayerPrefs.SetInt("Highscore", _highscore);
        StartCoroutine(IncreasePoints());
    }

    private IEnumerator IncreasePoints()
    {
        while (enabled && !isGameEnded)
        {
            AddPoints(1);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void AddPoints(int points)
    {
        _score += points;
        _tmp.text = _score.ToString("D6");
    }

    public int GetScore()
    {
        return _score;
    }

    public int UpdateHighscore()
    {
        _highscore = _highscore > _score ? _highscore : _score;
        PlayerPrefs.SetInt("Highscore", _highscore);
        isGameEnded = true;
        return _highscore;
    }

    public int GetHighscore()
    {
        return _highscore;
    }
}