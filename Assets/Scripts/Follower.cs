using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class Follower : MonoBehaviour
{
    public Spline spline;
    public float DurationInSecond;
    private float rate = 0;
    // Start is called before the first frame update
    void Start()
    {
        rate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rate += Time.deltaTime / DurationInSecond;
        CurveSample sample = spline.GetSample(rate);
        transform.localPosition = sample.location;
        transform.localRotation = sample.Rotation;
    }
}
