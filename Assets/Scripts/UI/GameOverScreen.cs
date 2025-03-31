using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOverScreen : MonoBehaviour
{
    [FormerlySerializedAs("_tmpGameScore")] [SerializeField] private TMP_Text tmpGameScore;
    [SerializeField] private TMP_Text _tmpHighscore;

    public void Setup(int gameScore, int highscore)
    {
        gameObject.SetActive(true);

        tmpGameScore.text = "Your Score:\n" + gameScore;
        _tmpHighscore.text = "Highscore:\n" + highscore;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}