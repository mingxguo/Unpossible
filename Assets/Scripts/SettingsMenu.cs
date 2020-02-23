using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    private Slider player_speed_slider;
    private Slider rotate_speed_slider;

    private void Awake()
    {
        gameObject.SetActive(false);
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
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

    public void Accept()
    {
        GameController.Instance.SetPlayerSpeed(player_speed_slider.value);
        GameController.Instance.SetRotateSpeed(rotate_speed_slider.value);
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
