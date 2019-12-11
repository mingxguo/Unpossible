using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public static float PlayerSpeed = 20;
    public float DistanceTravelled;
    public float Threshold = 30;


    private void Start()
    {
        PlayerSpeed = SettingsMenu.player_speed_slider.value;
        Debug.Log(PlayerSpeed);
        DistanceTravelled = -10f;
        transform.position = pathCreator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
    }

    // Update is called once per frame
    void Update()
    {
        if (pathCreator != null)
        {
            DistanceTravelled += PlayerSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);
        }
        if (PlayerSpeed < Threshold)
        {
            PlayerSpeed += 0.02f;
        }
    }
}
