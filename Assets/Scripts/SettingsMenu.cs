using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SettingsMenu : MonoBehaviour
{

    private Slider player_speed_slider;
    private Slider rotate_speed_slider;

    private void OnEnable()
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        Debug.Log("start");
        foreach (Slider s in sliders)
        {
            if (s.name == "PlayerSpeed")
            {
                player_speed_slider = s;
                Debug.Log("Found " + s.name);
            }
            else
            {
                rotate_speed_slider = s;
                Debug.Log("Found " + s.name);
            }
        }
    }

    public void SetPlayerSpeed()
    {
        Debug.Log("player speed" + player_speed_slider.value);
        Follower.PlayerSpeed = player_speed_slider.value;
    }

    public void SetRotateSpeed()
    {

        Debug.Log("rotate speed" + rotate_speed_slider.value);
        CameraRotator.RotateSpeed = rotate_speed_slider.value;
    }
}
