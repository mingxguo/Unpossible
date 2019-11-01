﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower2 : MonoBehaviour
{

    public Rotator player;
    public Transform center;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.GetPostion() - player.GetDirection();
        transform.rotation = player.GetRotation();        
    }
}
