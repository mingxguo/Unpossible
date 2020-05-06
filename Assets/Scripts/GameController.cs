using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject current_level;
    private PlayerController player;

    private float start_position;
    private float count_down;

    private int player_score;
    private int obs_next_cont;
    private bool tap_control = true;
    private bool developer_mode = false;
    private bool dead;
    private bool tutorial_complete = false;

    private float player_speed;
    private float rotate_speed;
    private float start_player_speed = 18.4f;
    private float start_rotate_speed = 60f;

    // extra variables
    private long total_left_turn;
    private long total_right_turn;
    private long total_left_pressed;
    private long total_right_pressed;

    private const float player_speed_threshold = 21.85f;
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

    #region SET_UP
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main" && scene.name != "Main Web")
        {
            Debug.Log("scene loaded");
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            current_level = GameObject.FindWithTag("Level");
            if(current_level == null)
            {
                Debug.Log("level not found");
            }
            UIController.Instance.LoadUI();
            if (scene.name == "Tutorial")
            {
#if UNITY_WEBGL
                WebSessionManager.Instance.LogTutorialStart();
#endif

#if UNITY_ANDROID
                SessionManager.Instance.LogTutorialStart();
#endif
                count_down = 120;
                start_position = -5f;
            }
            else
            {
#if UNITY_WEBGL
                WebSessionManager.Instance.LogExperimentStart();
#endif

#if UNITY_ANDROID
                SessionManager.Instance.LogExperimentStart();
#endif
                count_down = 300;
                start_position = -30f;
            }
            OnLevelStart();
        }
    }

    public void OnLevelStart()
    {
        // Set game parameters
        player_score = 0;        
        obs_next_cont = 1;
        dead = false;
        total_left_pressed = 0;
        total_left_turn = 0;
        total_right_pressed = 0;
        total_right_turn = 0;
        player_speed = start_player_speed;
        rotate_speed = start_rotate_speed;
        // Set obstacles
        ActivateNextObs(1, 0);
        // Set UI
        UIController.Instance.OnLevelStart();        
        // Set player position
        player.SetStartPosition(start_position, new Vector3(1.5f, 0, 0));
    }
    #endregion

    #region OBSTACLES
    private void DeactivateCurrentObs(int cont, int half)
    {
        string name = "Obstacles" + cont + half;
        Transform obstacles = current_level.transform.Find(name);
        if (obstacles != null)
        {
            //Debug.Log(name + " deactivated");
            obstacles.gameObject.SetActive(false);
        }
    }

    private void ActivateNextObs(int cont, int half)
    {
        string name = "Obstacles" + cont + half;
        Transform obstacles = current_level.transform.Find(name);
        if (obstacles != null)
        {
            //Debug.Log(name + " activated");
            obstacles.gameObject.SetActive(true);
        }
    }
    #endregion

    #region PLAYER_CONTROL
    public void DetectedPlayerCollision(Collider col)
    {
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++player_score;
            UIController.Instance.SetScoreText(player_score);
        }
        else if (col.gameObject.name == "Middle")
        {
            DeactivateCurrentObs(obs_next_cont, 0);
            ++obs_next_cont;
            ActivateNextObs(obs_next_cont, 0);
        }
        else if (col.gameObject.name == "Origin")
        {
            DeactivateCurrentObs(obs_next_cont - 1, 1);
            ActivateNextObs(obs_next_cont, 1);
        }

        else if(!developer_mode)
        {
            dead = true;
#if UNITY_WEBGL
            WebSessionManager.Instance.LogDeath(player.GetDistance(), player_score, 
            total_left_turn, total_right_turn, total_left_pressed, total_right_pressed);
#endif

#if UNITY_ANDROID
            SessionManager.Instance.LogDeath(player.GetDistance(), player_score);
#endif
            StartCoroutine(DeathCountdownCoroutine());
        }
    }

    IEnumerator DeathCountdownCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started death");

        UIController.Instance.StartDeathCountdown();
        UIController.Instance.SetDeathCountdownText(3);
        yield return new WaitForSeconds(1.0f);
        UIController.Instance.SetDeathCountdownText(2);
        yield return new WaitForSeconds(1.0f);
        UIController.Instance.SetDeathCountdownText(1);
        yield return new WaitForSeconds(1.0f);

        dead = false;

        // Reset obstacles
        DeactivateCurrentObs(obs_next_cont, 0);
        DeactivateCurrentObs(obs_next_cont, 1);
        DeactivateCurrentObs(obs_next_cont - 1, 0);
        DeactivateCurrentObs(obs_next_cont - 1, 1);

        // Restart level
        OnLevelStart();
    }

    IEnumerator EndGame()
    {

        // Clean obstacles
        DeactivateCurrentObs(obs_next_cont, 0);
        DeactivateCurrentObs(obs_next_cont, 1);
        DeactivateCurrentObs(obs_next_cont - 1, 0);
        DeactivateCurrentObs(obs_next_cont - 1, 1);

        UIController.Instance.SetGameOverUI();
        yield return new WaitForSeconds(3.0f);

        WebSessionManager.Instance.EndGame();
    }
    #endregion

    #region GAME_PARAMETERS
    public float GetPlayerSpeed()
    {
        return player_speed;
    }

    public float GetStartPlayerSpeed()
    {
        return start_player_speed;
    }

    public void SetStartPlayerSpeed(float speed)
    {
        start_player_speed = speed;
    }
    
    public float GetRotateSpeed()
    {
        return rotate_speed;
    }

    public float GetStartRotateSpeed()
    {
        return start_rotate_speed;
    }

    public void SetStartRotateSpeed(float speed)
    {
        start_rotate_speed = speed;
    }

    public void UpdatePlayerSpeed()
    {
        if (player_speed < player_speed_threshold)
        {
            player_speed += 0.001f;
        }
        else
        {
            Debug.Log("threshold");
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

    public bool IsDeveloperMode()
    {
        return developer_mode;
    }

    public void SetDeveloperMode(bool b)
    {
        developer_mode = b;
    }

    public bool PlayerIsDead()
    {
        return dead;
    }

    public bool IsTutorialCompleted()
    {
        return tutorial_complete;
    }
    #endregion

    public void Update()
    {
        if(SceneManager.GetActiveScene().name != "Main" && SceneManager.GetActiveScene().name != "Main Web")
        {
            // General game over
            if(count_down < 0)
            {
                // Tutorial ended
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
#if UNITY_WEBGL
                    WebSessionManager.Instance.LogTutorialEnd();
#endif

#if UNITY_ANDROID
                    SessionManager.Instance.LogTutorialEnd();
#endif
                    tutorial_complete = true;
                    SceneChanger.Instance.LoadScene(SceneChanger.MAIN);
                }
                else
                {
#if UNITY_WEBGL
                    WebSessionManager.Instance.LogExperimentEnd();
#endif

#if UNITY_ANDROID
                    SessionManager.Instance.LogExperimentEnd();
#endif
                    enabled = false;
                    StartCoroutine(EndGame());
                }
            }
            // Player playing: global count down
            else if(!dead)
            {
                if (Input.GetKeyDown("left"))
                {
                    ++total_left_pressed;
                }
                else if (Input.GetKeyDown("right"))
                {
                    ++total_right_pressed;
                }
                if (Input.GetKey("left"))
                {
                    ++total_left_turn;
                }
                else if (Input.GetKey("right"))
                {
                    ++total_right_turn;
                }
                count_down -= Time.deltaTime;
                UIController.Instance.SetCountdownText(count_down);
            }
        }
    }
    
}
