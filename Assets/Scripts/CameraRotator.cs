using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class CameraRotator : MonoBehaviour
{
    public float RotateSpeed;
    //public Transform center;
    public PathFollower follower;
    public PathCreator pathCreator;

    private Vector3 direction;
    private Vector3 offset;
    private Vector3 follower_position;

    // Update is called once per frame

    private void Start()
    {
        follower_position = follower.GetComponent<Transform>().position;
        offset = transform.position - follower_position;
    }

    public Vector3 GetPostion()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    void Update()
    {
        direction = pathCreator.path.GetDirectionAtDistance(follower.distanceTravelled);
        follower_position = follower.GetComponent<Transform>().position;
        transform.position = follower_position + offset;
        if (Input.GetKey("left"))
        {
            transform.RotateAround(follower_position, direction, RotateSpeed * Time.deltaTime);
        }
            //transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);
        
        else if (Input.GetKey("right"))
        {
            transform.RotateAround(follower_position, direction, -RotateSpeed * Time.deltaTime);
        }
        offset = transform.position - follower_position;
    }
}

