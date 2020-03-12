using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject current_level;
    private int player_score;
    private int obs_next_cont;
    private int obs_next_half;
    private float count_down = 200f;
    private bool tap_control = true;

    public static float PlayerSpeed = 20f;
    public static float RotateSpeed = 50f;
    public static float PlayerSpeedThreshold = 20f;
    public static bool GameOver = false;

    public static int XResolution = 1920;

    private static GameController _instance;
    public static GameController Instance {
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
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Main")
        {
            // set game parameters
            player_score = 0;
            obs_next_cont = 1;
            obs_next_half = 1;
            // Set UI
            UIController.Instance.LoadUI();
            UIController.Instance.SetScoreText(0);
            current_level = GameObject.FindWithTag("Level");
            
        }
        else if(scene.name == "Tutorial")
        {
            count_down = 120;
        }
        else
        {
            count_down = 300;
        }
    }

    public void OnLevelStart()
    {
        count_down = 300f;
    }

    public void DetectedPlayerCollision(Collider col)
    {
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++player_score;
            UIController.Instance.SetScoreText(player_score);
            Destroy(col.gameObject.transform.parent.gameObject);
        }
        else if (col.gameObject.name == "Middle")
        {
            obs_next_half = 0;
            ++obs_next_cont;
            UpdateObstacles();
        }
        else if (col.gameObject.name == "Origin")
        {
            obs_next_half = 1;
            UpdateObstacles();
        }

        // Loses game.
        // TODO: trigger event
        else
        {
            
            Debug.Log("game over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateObstacles()
    {
        string name = "Obstacles" + obs_next_cont + obs_next_half;
        Transform obstacles = current_level.transform.Find(name);
        if (obstacles != null)
        {
            Debug.Log(name + " activated");
            obstacles.gameObject.SetActive(true);
        }
    }

    public void SetPlayerSpeed(float speed)
    {
        PlayerSpeed = speed;
    }

    public void SetRotateSpeed(float speed)
    {
        RotateSpeed = speed;
    }

    public void UpdatePlayerSpeed()
    {
        if (PlayerSpeed < PlayerSpeedThreshold)
        {
            PlayerSpeed += 0.001f;
        }
    }

    public bool IsTapControl()
    {
        return tap_control;
    }

    public void SetTapControl(bool b)
    {
        tap_control = b;
    }

public void FixedUpdate()
    {
        //if(SceneManager.GetActiveScene().name != "Main")
        //{
        //    UIController.Instance.SetTimerText();
        //}

        if(SceneManager.GetActiveScene().name != "Main")
        {
            if(count_down < 0)
            {
                // Tutorial ended
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    Debug.Log("tutorial ended");
                    SceneManager.LoadScene(0);
                }
                else
                {
                    UIController.Instance.GameOver();
                }
                // TODO: log event
            }
            else
            {
                count_down -= Time.deltaTime;
                UIController.Instance.SetCountdownText(count_down);
            }
        }
    }
}
