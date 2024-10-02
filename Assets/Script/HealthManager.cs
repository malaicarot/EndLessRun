using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private int maxHealth = 3;
    private int currentHealth;
    public Sprite fullHeart;

    public Sprite emptyHeart;

    public Image[] hearts;

    public bool isDead = false;

    private PauseMenusController gameOver;

    void Start()
    {
        currentHealth = maxHealth;
        gameOver = FindObjectOfType<PauseMenusController>();
        UpdateHearts();
        
    }

    public void TakeDame(int amount){
        currentHealth -= amount;

        if(currentHealth < 0){
            currentHealth = 0;
        }
        UpdateHearts();
        if (currentHealth == 0)
        {
            isDead = true;
            StartCoroutine(GameOver());
        }

    }

    public IEnumerator GameOver(){
        yield return new WaitForSeconds(4f);
        gameOver.GameOverMenus();
    }

    void UpdateHearts()
    {
        for(int i = 0; i < hearts.Length; i++){
            if(i < currentHealth){
                hearts[i].sprite = fullHeart;
            } else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        
    }
}
