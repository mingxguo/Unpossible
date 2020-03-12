using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Text score_text;
    private Text timer_text;
    private Text countdown_text;
    private Button back_button;
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadUI()
    {
        score_text = GameObject.FindWithTag("score").GetComponent<Text>();
        timer_text = GameObject.FindWithTag("timer").GetComponent<Text>();
        countdown_text = GameObject.FindWithTag("countdown").GetComponent<Text>();
        game_over_ui = GameObject.FindWithTag("game_over");
        back_button = GameObject.FindWithTag("back").GetComponent<Button>();
        back_button.onClick.AddListener(BackButton);
        game_over_ui.SetActive(false);
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

    public void SetCountdownText(float time)
    {
        int seconds = (int)(time % 60f);
        int minutes = (int)(time / 60f);
        countdown_text.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void GameOver()
    {
        game_over_ui.SetActive(true);
    }

    public void BackButton()
    {
        Debug.Log("Back pressed");
        SceneManager.LoadScene(0);
    }

}
