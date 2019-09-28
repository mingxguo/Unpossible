using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float RotateSpeed;
    public Transform center;

    private Vector3 offset;

    // Update is called once per frame

    private void Start()
    {
        offset = transform.position - center.position;
    }

    void Update()
    {
        transform.position = center.position + offset;
        if (Input.GetKey("left"))
        {
            transform.RotateAround(center.position, new Vector3(0, 0, 1), RotateSpeed * Time.deltaTime);
        }
            //transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);
        
        else if (Input.GetKey("right"))
        {
            transform.RotateAround(center.position, new Vector3(0, 0, 1), -RotateSpeed * Time.deltaTime);
        }
        offset = transform.position - center.position;
    }
}

