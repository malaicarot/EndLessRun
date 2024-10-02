using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


public class ScoreManagement : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    public static void AddScore(int amount){
        score += amount;

        
    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;
    }
}
