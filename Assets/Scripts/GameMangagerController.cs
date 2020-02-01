using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMangagerController : MonoBehaviour
{
    public static GameMangagerController GAME_MANAGER;
    public static string LEVEL_BEATEN = "levelBeaten";

    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite goldenStar;

    [Header("GameOver")]
    public GameObject endScreen;
    public bool gameOver;

    private int currentEnemiesCount;
    private int startingEnemiesCount;

    void Start()
    {
        GAME_MANAGER = this;
        startingEnemiesCount = EnemyController.enemiesAlive;
        currentEnemiesCount = startingEnemiesCount;
        gameOver = false;
        endScreen.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
        {
            currentEnemiesCount = EnemyController.enemiesAlive;
            Debug.Log("Game Over: " + currentEnemiesCount);
            endScreen.SetActive(true);

            int starsEarned = 0;
            if(currentEnemiesCount <= 2)
            {
                star1.sprite = goldenStar;
                starsEarned++;
            }
            if (currentEnemiesCount <= 1)
            {
                star2.sprite = goldenStar;
                starsEarned++;
                UnlockNextLevel(SceneManager.GetActiveScene().buildIndex);
            }
            if (currentEnemiesCount <= 0)
            {
                star3.sprite = goldenStar;
                starsEarned++;
            }

            Debug.Log("Level " + SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Stars earned: " + starsEarned);
            SetStarsForLevel(starsEarned, SceneManager.GetActiveScene().buildIndex);

            EnemyController.ResetEnemies();
            gameOver = false;

        }

    }

    private void SetStarsForLevel(int stars, int currentLevel)
    {
        string prefStarsKey = "level" + currentLevel.ToString() + "stars";
        if (PlayerPrefs.GetInt(prefStarsKey, 0) < stars)
        {
            PlayerPrefs.SetInt("level" + currentLevel.ToString() + "stars", stars);
        }
    }

    private void UnlockNextLevel(int currentLevel)
    {
            PlayerPrefs.SetInt(LEVEL_BEATEN, currentLevel);
    }

    public void OnClickMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
