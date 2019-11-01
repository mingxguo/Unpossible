using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using SplineMesh;

public class Rotator : MonoBehaviour
{
    public float RotateSpeed;
    public Transform center;
    public float DurationInSecond;

    public Spline spline;
    private float rate = 0;

    private Vector3 direction;
    private Vector3 offset;
    private Vector3 follower_position;

    // Update is called once per frame

    private void Start()
    {
        follower_position = center.position;
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
        rate += Time.deltaTime / DurationInSecond;
        CurveSample sample = spline.GetSample(rate);
        direction = sample.tangent;

        follower_position = center.position;
        transform.position = follower_position + offset;
        if (Input.GetKey("left"))
        {
            transform.RotateAround(follower_position, sample.tangent, RotateSpeed * Time.deltaTime);
        }
        //transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);

        else if (Input.GetKey("right"))
        {
            transform.RotateAround(follower_position, sample.tangent, -RotateSpeed * Time.deltaTime);
        }
        offset = transform.position - follower_position;
    }
}

