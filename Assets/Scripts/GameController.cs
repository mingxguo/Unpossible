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
    private bool tap_control;
    private bool dead;

    private float player_speed;
    private float rotate_speed;

    private const float player_speed_threshold = 20f;
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
            gameObject.SetActive(false); 
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main")
        {
            Debug.Log("scene loaded");
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            current_level = GameObject.FindWithTag("Level");
            UIController.Instance.LoadUI();
            if (scene.name == "Tutorial")
            {
                count_down = 120;
                start_position = -5f;
            }
            else
            {
                count_down = 300;
                start_position = -20f;
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
        tap_control = true;

        player_speed = 15f;
        rotate_speed = 50f;
        // Set obstacles
        ActivateNextObs(1, 0);
        // Set UI
        UIController.Instance.OnLevelStart();        
        // Set player position
        player.SetStartPosition(start_position, new Vector3(1.5f, 0, 0));
    }
    
    public void DetectedPlayerCollision(Collider col)
    {
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++player_score;
            UIController.Instance.SetScoreText(player_score);
            //Destroy(col.gameObject.transform.parent.gameObject);
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
        
        // TODO: trigger death event
        else
        {
            dead = true;
            StartCoroutine(DeathCountdownCoroutine());
        }
    }

    private void DeactivateCurrentObs(int cont, int half)
    {
        string name = "Obstacles" + cont + half;
        Transform obstacles = current_level.transform.Find(name);
        if (obstacles != null)
        {
            Debug.Log(name + " deactivated");
            obstacles.gameObject.SetActive(false);
        }
    }

    private void ActivateNextObs(int cont, int half)
    {
        string name = "Obstacles" + cont + half;
        Transform obstacles = current_level.transform.Find(name);
        if (obstacles != null)
        {
            Debug.Log(name + " activated");
            obstacles.gameObject.SetActive(true);
        }
    }

    public float GetPlayerSpeed()
    {
        return player_speed;
    }

    public void SetPlayerSpeed(float speed)
    {
        player_speed = speed;
    }
    
    public float GetRotateSpeed()
    {
        return rotate_speed;
    }

    public void SetRotateSpeed(float speed)
    {
        rotate_speed = speed;
    }

    public void UpdatePlayerSpeed()
    {
        if (player_speed < player_speed_threshold)
        {
            player_speed += 0.0005f;
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

    public bool PlayerIsDead()
    {
        return dead;
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

    public void Update()
    {
        if(SceneManager.GetActiveScene().name != "Main")
        {
            // General game over
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
            // Player playing: global count down
            else if(!dead)
            {
                count_down -= Time.deltaTime;
                UIController.Instance.SetCountdownText(count_down);
            }
        }
    }
}
