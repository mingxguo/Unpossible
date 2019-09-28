using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    public GameObject player;
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position - new Vector3(0, 0, 5);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 newPos = playerTransform.position - new Vector3(0, 0, 5);
        transform.position = newPos;

        //transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }
}
