using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class PlayerController: MonoBehaviour
{
    public GameObject level;
    public Text score_text;
    public Text timer_text;
    public GameObject game_over_ui;
    
    private int score;
    private Rigidbody rb;
    private float timer;
    
    int next_cont;
    int next_half;

    // Start is called before the first frame update
    void Start()
    {
        game_over_ui.SetActive(false);
        Time.timeScale = 1f;

        score = 0;
        timer = 0f;
        SetScoreText();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
        next_cont = 0;
        next_half = 1;
    }
    
    // Kinematic rigidbody
    void OnTriggerEnter(Collider col)
    {
        //collision.text = col.gameObject.name;
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++score;
            SetScoreText();
            Destroy(col.gameObject.transform.parent.gameObject);
        }
        else if(col.gameObject.name == "Middle")
        {
            next_half = 0;
            ++next_cont;
            UpdateObstacles();
        }
        else if(col.gameObject.name == "Origin")
        {
            next_half = 1;
            UpdateObstacles();
        }

        // Loses game.
        else
        {
            StopGame();
        }
    }    

    private void UpdateObstacles() {
        string name = "Obstacles" + next_cont + next_half;
        Debug.Log(name + "activated");
        GameObject obstacles = level.transform.Find(name).gameObject;
        if (obstacles != null)
        {
            obstacles.SetActive(true);
        }
    }
    void SetScoreText()
    {
        score_text.text = score.ToString();
    }

    private void FixedUpdate()
    {
        int seconds = (int)(Time.timeSinceLevelLoad % 60f);
        int milliseconds = (int)(Time.timeSinceLevelLoad * 100f) % 100;
        timer_text.text = seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
    }

    public void StopGame()
    {
        game_over_ui.SetActive(true);
        Time.timeScale = 0f;
    }
    
}
