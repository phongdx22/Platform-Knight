using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] float deathDelay = 1.0f;
    [SerializeField] int deathScoreReduction = 500;
    
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI statisticsText;

    void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = "LIFE: " + playerLives.ToString();
        scoreText.text = "SCORE: " + playerScore.ToString();

        livesText.enabled = false;
        scoreText.enabled = false;
        statisticsText.enabled = false;
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            StartCoroutine(ReduceLife());
        }
        else
        {
            StartCoroutine(ResetGameSession());
        }
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSeconds(deathDelay);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        LoadMainMenu();
        Destroy(gameObject);
    }

    IEnumerator ReduceLife()
    {
        yield return new WaitForSecondsRealtime(deathDelay);
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        AddScore(-deathScoreReduction);
        livesText.text = "LIFE: " + playerLives.ToString();
    }

    public void AddScore(int score)
    {
        playerScore += score;
        if (playerScore < 0)
        {
            playerScore = 0;
        }
        scoreText.text = "SCORE: " + playerScore.ToString();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level 1");
        livesText.enabled = true;
        scoreText.enabled = true;
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("Level 2");
        livesText.enabled = true;
        scoreText.enabled = true;
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene("Level 3");
        livesText.enabled = true;
        scoreText.enabled = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        Destroy(gameObject);
    }

    public void LoadLevelSelector()
    {
        SceneManager.LoadScene("Level Select");
        Destroy(gameObject);
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
        Destroy(gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
