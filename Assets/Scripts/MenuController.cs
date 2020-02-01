using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static string LEVEL_BEATEN = "levelBeaten";

    [Header("GameOver")]
    public GameObject levelsScreen;
    public GameObject startScreen;

    [Header("LockButtons")]
    public GameObject[] lockButtons;

    [Header("Stars")]
    public MultiDimensionalStar[] levelStars;


    public Sprite goldenStar;
    private static bool firstEntered = false;


    private void Awake()
    {
        if (!firstEntered)
        {
            Debug.Log("Menu awakes!: " + firstEntered);
            levelsScreen.SetActive(false);
            startScreen.SetActive(true);
            firstEntered = true;
        }
    }

    void Start()
    {
        DeactivateLockButtons();
        RenderStars();

    }

    public void OnClickLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void OnClickPlay()
    {
        startScreen.SetActive(false);
        levelsScreen.SetActive(true);
    }

    public void OnReset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClose()
    {
        Application.Quit();
    }

    private void DeactivateLockButtons()
    {
        int levelBeaten = PlayerPrefs.GetInt(LEVEL_BEATEN);
        for(int i = 0; i < levelBeaten; i++)
        {
            if(levelBeaten > 0)
            {
                lockButtons[i].SetActive(false);
            }
        }
    }

    private void RenderStars()
    {
        for (int level = 1; level < SceneManager.sceneCountInBuildSettings + 1; level++)
        {
            Debug.Log("Current loaded scenes: " + SceneManager.sceneCountInBuildSettings);
                int starsCount = 0;
                while (PlayerPrefs.GetInt("level" + level.ToString() + "stars", 0) != starsCount)
                {
                Debug.Log("Level " + level.ToString() + ", Stars: " + PlayerPrefs.GetInt("level" + level.ToString() + "stars", 0));
                levelStars[level-1].stars[starsCount].sprite = goldenStar;
                    starsCount++;
                }
        }
    }

    [System.Serializable]
    public class MultiDimensionalStar
    {
        public Image[] stars;
    }
}
