using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 3f;
    public Text score_text;
    public Text collision;
    public GameObject game_over_ui;

    private int track;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        game_over_ui.SetActive(false);
        Time.timeScale = 1f;
        track = Random.Range(0, 2);
        // Starts on the left track.
        if (track == 0)
        {
            transform.position += new Vector3(-1.5f, 0, 0);
        }
        // Starts on the right track.
        else
        {
            transform.position += new Vector3(1.5f, 0, 0);
        }
        score = 0;
        SetScoreText();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        // Changes from right track to left.
        if (Input.GetKey("left") && track == 1)
        {
            track = 0;
            transform.position += new Vector3(-3, 0, 0);
        }
        else if(Input.GetKey("right") && track == 0)
        {
            track = 1;
            transform.position += new Vector3(3, 0, 0);
        }
    }

    // Non kinematic rigidbody
    void OnCollisionEnter(Collision col)
    {
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++score;
            SetScoreText();
            Destroy(col.gameObject.transform.parent.gameObject);
        }
        // Loses game.
        else if (col.gameObject.name != "Road")
        {
            StopGame();
        }
    }

    // Kinematic rigidbody
    void OnTriggerEnter(Collider col)
    {
        // Adds point
        if (col.gameObject.name == "Point Collider")
        {
            ++score;
            SetScoreText();
            Destroy(col.gameObject.transform.parent.gameObject);
        }
        // Loses game.
        else if (col.gameObject.name != "Road")
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
