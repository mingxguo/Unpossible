using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{ 
    private Text score_text;
    private Text timer_text;
    private GameObject game_over_ui;

    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            score_text = GameObject.FindWithTag("score").GetComponent<Text>();
            timer_text = GameObject.FindWithTag("timer").GetComponent<Text>();
            game_over_ui = GameObject.FindWithTag("Finish");
            game_over_ui.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetScoreText(int score)
    {
        score_text.text = score.ToString();
    }

    public void SetTimerText()
    {
        int seconds = (int)(Time.timeSinceLevelLoad % 60f);
        int milliseconds = (int)(Time.timeSinceLevelLoad * 100f) % 100;
        timer_text.text = seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
    }

    public void GameOver()
    {
        game_over_ui.SetActive(true);
    }

}
