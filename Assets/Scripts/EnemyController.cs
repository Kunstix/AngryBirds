using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static int enemiesAlive = 0;

    public float enemyHealth = 6F;

    public void Start()
    {
        enemiesAlive++;
        Debug.Log("Living enemy: " + enemiesAlive + " name: " + this.name);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude > enemyHealth)
        {
            Die();
            GameOver();
        }
    }

    public static void ResetEnemies()
    {
        enemiesAlive = 0;
    }

    private void GameOver()
    {
        GameMangagerController gm = GameMangagerController.GAME_MANAGER;
        if (enemiesAlive <= 0)
        {
            if (gm != null)
            {
                if (gm.endScreen.activeSelf == true)
                {
                    return;
                }
                else
                {
                    gm.gameOver = true;
                }
            }
        }
    }

    private void Die()
    {
        enemiesAlive--;
        Debug.Log("Dying enemy: " + enemiesAlive);
        Destroy(gameObject);
    }
}
