using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject level;
    public EndOfPathInstruction endOfPathInstruction;
    public static float PlayerSpeed = 20;
    public float DistanceTravelled;
    public float Threshold = 30;
    
    private PathCreator path_creator;
    

    private void Start()
    {
        path_creator = level.GetComponent<PathCreator>();
        if(path_creator == null) { Debug.Log("error"); }

        //PlayerSpeed = SettingsMenu.player_speed_slider.value;
        PlayerSpeed = 20f;
        Debug.Log(PlayerSpeed);
        DistanceTravelled = -10f;
        transform.position = path_creator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
    }

    // Update is called once per frame
    void Update()
    {
        //Update follower position
        if (path_creator != null)
        {
            DistanceTravelled += PlayerSpeed * Time.deltaTime;
            transform.position = path_creator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
            transform.rotation = path_creator.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);
        }

        //Update speed
        if (PlayerSpeed < Threshold)
        {
            PlayerSpeed += 0.02f;
        }
    }
}
