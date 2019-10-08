using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    
    public Text score_text;
    public Text collision;
    public GameObject game_over_ui;
    
    private int score;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        game_over_ui.SetActive(false);
        Time.timeScale = 1f;

        score = 0;
        SetScoreText();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
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
        // Loses game.
        else
        {
            StopGame();
        }
    }

    void SetScoreText()
    {
        score_text.text = score.ToString();
    }

    void StopGame()
    {
        game_over_ui.SetActive(true);
        Time.timeScale = 0f;
    }
}
