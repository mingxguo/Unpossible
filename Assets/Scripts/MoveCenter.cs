using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCenter : MonoBehaviour
{
    public float speed;
    public bool move;

    // Update is called once per frame
    void Update()
    {
        speed+=0.02f;
        if (move)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
