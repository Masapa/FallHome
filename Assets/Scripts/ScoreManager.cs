using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public double maxScore;
    private double curScore;
    public double scoreMultipler = 100.0f;
    private bool frozen = false;

    private Text scoreText;

    void Start()
    {
        curScore = maxScore;
        scoreText = GameObject.Find("ScreenSpaceCanvas/ScoreCounter").GetComponent<Text>();
    }

    void Update()
    {
        scoreText.text = "Score: " + GetScore().ToString("0");
    }

    void FixedUpdate()
    {
        if (curScore > 0.0f && !frozen) {
            curScore -= Time.fixedDeltaTime;
            if (curScore < 0.0f) {
                curScore = 0.0f;
            }
        }
    }

    public void OnPlayerDeath()
    {
        frozen = true;
    }

    public void OnLevelReset()
    {
        frozen = false;
        curScore = maxScore;
    }

    public double GetScore()
    {
        return curScore * scoreMultipler;
    }
}
