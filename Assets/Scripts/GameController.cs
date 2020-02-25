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

    public static float PlayerSpeed = 20f;
    public static float RotateSpeed = 200f;
    public static float PlayerSpeedThreshold = 20f;
    public static bool GameOver = false;

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
            OnLevelStart();
        }
    }

    public void OnLevelStart()
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
            //UIController.Instance.GameOver();
            Debug.Log("game over");
            //GameOver = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //OnLevelStart();
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

public void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name != "Main")
        {
            UIController.Instance.SetTimerText();
        }
    }
}
