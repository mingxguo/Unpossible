using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public static Slider player_speed_slider;
    public static Slider rotate_speed_slider;

    private void Awake()
    {
        gameObject.SetActive(false);
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
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerSpeed()
    {
        //Debug.Log("player speed" + player_speed_slider.value);
        //Follower.PlayerSpeed = player_speed_slider.value;
    }

    public void SetRotateSpeed()
    {

        //Debug.Log("rotate speed" + rotate_speed_slider.value);
        //CameraRotator.RotateSpeed = rotate_speed_slider.value;
    }

    public void Accept()
    {
        Follower.PlayerSpeed = player_speed_slider.value;
        CameraRotator.RotateSpeed = rotate_speed_slider.value;
        gameObject.SetActive(false);
        MainMenu.SetActive(true);

    }
}
