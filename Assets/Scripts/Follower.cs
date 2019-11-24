using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float Speed = 20;
    public float DistanceTravelled = -10f;
    public float Threshold = 30;
    

    // Update is called once per frame
    void Update()
    {
        if (pathCreator != null)
        {
            DistanceTravelled += Speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);
        }
        if (Speed < Threshold)
        {
            Speed += 0.02f;
        }
    }
}
